using GoodMoodPerfumeBot.Models;
using GoodMoodPerfumeBot.Shared;
using GoodMoodPerfumeBot.UserRoles;
using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace GoodMoodPerfumeBot.Services
{
    public class UpdateHandler
    {
        private readonly IServiceScopeFactory scopeFactory;
        private IMemoryCache cache;
        private bool IsEnteringPaymentDetails = false;
        private AppUser owner = null;
        public UpdateHandler(IServiceScopeFactory scopeFactory, IMemoryCache cache)
        {
            this.scopeFactory = scopeFactory;
            this.cache = cache;
        }
        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();            

            await (update switch
            {
                { Message: { Photo: { } } message  } => OnPhoto(botClient, message),
                { Message: { ForwardFrom: { } } message } => OnForwardedMessage(botClient, message),
                { Message: {  } message } => OnMessage(botClient, message),
                { CallbackQuery: { } query } => OnCallbackQuery(botClient, query),                
                _ => DefaultHandler()
                
                
            });
        }    

        private Task DefaultHandler()
        {
            Console.WriteLine("default handler");
            return Task.CompletedTask;
        }       

        private async Task OnForwardedMessage(ITelegramBotClient botClient, Message message)
        {
            if (message.From.IsBot)
                return;

            using (var scope = this.scopeFactory.CreateScope())
            {

                if (!IsAdmin(scope, message.From.Id))
                    return;

                var chatId = message.Chat.Id;
                var from = message.ForwardFrom.Id;

            
                var orderService = scope.ServiceProvider.GetService<IOrderService>();
                var orders = await orderService.GetAllUserOrdersAsync(from);

                for(int i = 0; i < orders.Count(); i++ )
                {
                    var orderMessage = CreateOrderMessage(orders[i], from);
                    await botClient.SendMediaGroup(chatId, orderMessage.photos);
                    await botClient.SendMessage(chatId, orderMessage.message, parseMode: ParseMode.Html, replyMarkup: orderMessage.keyboard);
                }
            }
        }       

        private async Task OnPhoto(ITelegramBotClient botClient, Message message)
        {
            if (message.From.IsBot)
                return;

            var chatId = message.Chat.Id;
            long userId = message.From.Id;
            string photoId = message.Photo[0].FileId;

            Order order = null;
            AppUser admin = null;
            using (var scope = scopeFactory.CreateScope())
            {
                var orderService = scope.ServiceProvider.GetService<IOrderService>();
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();

                admin = userService.GetAdmin();
                order = await orderService.GetOrderByUserIdAsync(userId);
                if ( order.PayPhotoId != null)
                {
                    await botClient.SendMessage(chatId, "Вы уже отправляли фото");
                    return;
                }
                order.PayPhotoId = photoId;
                await orderService.SetOrderPayPhoto(order);
            }

            var orderMessage = CreateOrderMessage(order, userId);

            await botClient.SendMediaGroup(admin.TelegramUserId, orderMessage.photos);
            await botClient.SendMessage(admin.TelegramUserId, orderMessage.message, parseMode: ParseMode.Html, replyMarkup: orderMessage.keyboard);
            await botClient.SendMessage(chatId, "Ваш чек отправлен на проверку администратору");
        }



        private async Task OnCallbackQuery(ITelegramBotClient botClient, CallbackQuery query)
        {
            if (query.Data.Contains("cancel_removing_admin"))
            {
                await botClient.DeleteMessage(query.Message.Chat.Id, query.Message.Id);
                await botClient.AnswerCallbackQuery(query.Id);
                return;
            }
            else if (query.Data.Contains("remove_admin"))
            {
                await botClient.DeleteMessage(query.Message.Chat.Id, query.Message.Id);
                await DeleteAdminAsync(query.Message.Chat.Id);
                await botClient.AnswerCallbackQuery(query.Id);
                return;
            }

            await ProcessOrderMessage(botClient, query);  
        }

        private async Task OnMessage(ITelegramBotClient botClient, Message message)
        {

            if (message.From.IsBot)
                return;
            var chatId = message.Chat.Id;
            var userId = message.From.Id;

            var messageText = message.Text;

            if (!string.IsNullOrEmpty(messageText))
            {

                if(messageText.StartsWith("/start"))
                {
                    var startString = messageText.Split(" ");
                    if(startString.Length > 1)
                    {
                        var payload = startString[1];
                        if (payload.Equals("set_administrator"))
                            await SetAdminAsync(chatId, userId);
                    }
                }

                switch (messageText)
                {
                    case "/get_payment_details":
                        await GetPaymentDetails(botClient, chatId);
                        break;
                    case "/get_not_shipped":
                        await GetNotShippedOrdersAsync(botClient, message);
                        break;
                    case "/get_not_payed":
                        await GetNotPayedOrdersAsync(botClient, message);
                        break;
                    case "/set_admin":
                        await CreateLinkForSettingAdminAsync(botClient, chatId, userId);
                        break;
                    case "/delete_admin":
                        await CreateRemoveAdminButtons(botClient, chatId, userId);
                        break;
                    case "/set_payment_details":
                        this.IsEnteringPaymentDetails = true;
                        break;
                    case "/set_owner":
                        await SetOwnerAsync(chatId, userId);
                        break;

                }
                
            }

            if (this.IsEnteringPaymentDetails == true)
                await this.SetPaymentDetails(botClient, message);

        }

        private async Task GetPaymentDetails(ITelegramBotClient botClient, long chatId)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var payment = scope.ServiceProvider.GetRequiredService<PaymentDetailsService>();
                var paymentDetails = await payment.GetDetailsAsync();
                if (paymentDetails == null)
                    await botClient.SendMessage(chatId, "Не удалось получить платежные данные");
                else
                    await botClient.SendMessage(chatId, $"Телефон для перевода:\n<pre>{paymentDetails.Phone}</pre>\n Номер карты:\n<pre>{paymentDetails?.CardNumber}</pre>", ParseMode.Html);
                
            }
        }

        private async Task SetPaymentDetails(ITelegramBotClient botClient, Message message)
        {
            var chatId = message.Chat.Id;
            if(this.owner == null)
            {
                using (var scope = scopeFactory.CreateScope())
                {
                    var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                    this.owner = userService.GetOwner();
                    if (this.owner == null)
                    {
                        this.IsEnteringPaymentDetails = false;
                        await botClient.SendMessage(chatId, "Не удалось получить профиль владельца");
                        return;
                    }
                }
            }

            if(this.owner.TelegramUserId != message.From.Id)
            {
                this.IsEnteringPaymentDetails = false;
                await botClient.SendMessage(chatId, "Вы не можете изменять платежные данные");
                return;
            }
                
            var userState = this.cache.Get<UserState>(this.owner.TelegramUserId) ?? new UserState();

            switch(userState.Step)
            {
                case Step.None:
                    userState.Step = Step.WaitingForPhone;
                    this.cache.Set(this.owner.TelegramUserId, userState, TimeSpan.FromMinutes(10));
                    await botClient.SendMessage(chatId, "Введите номер телефона");
                    break;
                case Step.WaitingForPhone:
                    userState.PaymentDetails.Phone = message.Text.Trim();
                    userState.Step = Step.WaitingForCardNumber;
                    this.cache.Set(this.owner.TelegramUserId, userState);
                    await botClient.SendMessage(chatId, "Введите номер счета");
                    break;
                case Step.WaitingForCardNumber:
                    userState.PaymentDetails.CardNumber = message.Text.Trim();
                    userState.Step = Step.Completed;
                    this.IsEnteringPaymentDetails = false;
                    await this.CreateOrUpdatePaymentDetails(userState.PaymentDetails);
                    this.cache.Remove(this.owner.TelegramUserId);
                    await botClient.SendMessage(chatId, "Платежные данные сохранены");
                    break;

            }
        }

        private async Task ProcessOrderMessage(ITelegramBotClient botClient, CallbackQuery query)
        {
            var data = query.Data.Split("#");
            var value = data[0];
            int orderId = int.Parse(data[1]);
            long userId = long.Parse(data[2]);



            using (var scope = scopeFactory.CreateScope())
            {
                var orderService = scope.ServiceProvider.GetService<IOrderService>();
                var order = await orderService.GetOrderByIdAsync(orderId);

                if (order != null)
                {
                    if (value.Equals(SharedData.OrderStatus.Shipped))
                    {
                        if (order.PayStatus.Equals(SharedData.PayStatus.NotPayed))
                        {
                            await botClient.AnswerCallbackQuery(query.Id, "Заказ ещё не оплачен", showAlert: true);
                            return;
                        }
                        await orderService.SetOrderStatusShippedAsync(orderId);

                        var button = InlineKeyboardButton.WithUrl("Час с клиентом", $"tg://user?id={order.AppUser.TelegramUserId}");                  
                        InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup(button);

                        string messageToAdmin = CreateMessageForAdmin(order);
                        await botClient.EditMessageText(query.Message.Chat.Id, query.Message.Id, messageToAdmin, replyMarkup: keyboard);

                        await botClient.SendMessage(userId, "Ваш заказ отправлен");
                        await botClient.AnswerCallbackQuery(query.Id, "Заказ помечен отправленным", showAlert: true);

                    }
                    else if (value.Equals(SharedData.PayStatus.Payed))
                    {
                        await orderService.SetOrderStatusPayedAsync(orderId);

                        string messageToAdmin = CreateMessageForAdmin(order);

                        await botClient.SendMessage(userId, "Оплата проверена");

                        var buttons = new InlineKeyboardButton[][]
                        {
                            new InlineKeyboardButton[]
                            {
                                    InlineKeyboardButton.WithUrl("Час с клиентом", $"tg://user?id={order.AppUser.TelegramUserId}")
                            },
                            new InlineKeyboardButton[]
                            {
                                InlineKeyboardButton.WithCallbackData("Отметить отправленным", SharedData.OrderStatus.Shipped + "#" + order.Id.ToString() + "#" + userId)
                            }
                        };
                        InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup(buttons);

                        await botClient.EditMessageText(query.Message.Chat.Id, query.Message.Id, messageToAdmin, replyMarkup: keyboard);
                        await botClient.AnswerCallbackQuery(query.Id, "Заказ помечен оплаченным", showAlert: true);
                    }
                }
                else
                {
                    await botClient.AnswerCallbackQuery(query.Id, "Заказ не найден", showAlert: true);
                }
            }


        }
        private string CreateMessageForAdmin(Order order)
        {
            string orderPositions = string.Empty;

            for (int i = 0; i < order.OrderItems.Count(); i++)
            {
                orderPositions = string.Concat(orderPositions, $"Позиция: {i + 1}\n" +
                     $"Название: {order.OrderItems[i].Product.Name}\n" +
                     $"Количество: {order.OrderItems[i].Quantity}шт.\n\n");

            }

            var messageToAdmin = $"Заказ №{order.Id}\n\n" + $"Адрес:\n{order.Address}\n\n" + orderPositions + $"{SharedData.PayStatus.PayMessage[order.PayStatus]}\n" +
                $"{SharedData.OrderStatus.ShipMessage[order.OrderStatus]}";

            return messageToAdmin;
        }

        private (string message, List<IAlbumInputMedia> photos, InlineKeyboardMarkup keyboard) CreateOrderMessage(Order order, long userId)
        {
            var photoToAdmin = new List<IAlbumInputMedia>();

            if(!string.IsNullOrEmpty(order.PayPhotoId))
                photoToAdmin.Add(new InputMediaPhoto(order.PayPhotoId));

            foreach (var item in order.OrderItems)
            {
                photoToAdmin.Add(new InputMediaPhoto(item.Product.ImageUrl));
            }
            var messageToAdmin = CreateMessageForAdmin(order);

            var buttons = new List<List<InlineKeyboardButton>>();

            buttons.Add(new List<InlineKeyboardButton>()
            {
                InlineKeyboardButton.WithUrl("Час с клиентом", $"tg://user?id={order.AppUser.TelegramUserId}")
            });

            if (order.PayStatus.Equals(SharedData.PayStatus.NotPayed))
            {
                buttons.Add(new List<InlineKeyboardButton>()
                {
                    InlineKeyboardButton.WithCallbackData("Отметить оплаченным", SharedData.PayStatus.Payed + "#" + order.Id.ToString() + "#" + userId)
                });

                buttons.Add(new List<InlineKeyboardButton>()
                {
                    InlineKeyboardButton.WithCallbackData("Отметить отправленным", SharedData.OrderStatus.Shipped + "#" + order.Id.ToString() + "#" + userId)
                });              

            }
            else if (order.OrderStatus.Equals(SharedData.OrderStatus.NotShipped))
            {
                buttons.Add(new List<InlineKeyboardButton>()
                {
                    InlineKeyboardButton.WithCallbackData("Отметить отправленным", SharedData.OrderStatus.Shipped + "#" + order.Id.ToString() + "#" + userId)
                });
            }

            InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup(buttons);

            return (messageToAdmin, photoToAdmin, keyboard);
        }

        private async Task CreateRemoveAdminButtons(ITelegramBotClient botClient, long chatId, long userId)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                var owner = userService.GetOwner();
                if (owner.TelegramUserId != userId)
                    return;

            }
            InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup();
            var buttons = new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("Отмена", "cancel_removing_admin"),
                InlineKeyboardButton.WithCallbackData("Подтвердить", "remove_admin")
            };

            keyboard.AddButtons(buttons);

            await botClient.SendMessage(chatId, "Вы действительно хотите удалить администратора?", replyMarkup: keyboard);
        }

        private async Task DeleteAdminAsync(long chatId)
        {
            using (var scope = this.scopeFactory.CreateScope())
            {
                var commandService = scope.ServiceProvider.GetRequiredService<CommandService>();
                await commandService.DeleteAdminAsync(chatId);
            }
        }
        private async Task CreateLinkForSettingAdminAsync(ITelegramBotClient botClient, long chatId, long userId)
        {
            using (var scope = this.scopeFactory.CreateScope())
            {
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                AppUser owner = userService.GetOwner();

                if (owner?.TelegramUserId != userId || owner == null)
                    return;
                else
                {
                    AppUser admin = userService.GetAdmin();
                    if (admin != null)
                    {
                        await botClient.SendMessage(chatId, "Администратор уже установлен. Чтобы установить нового, удалите текущего");
                        return;
                    }
                    var bot = await botClient.GetMe();
                    string link = $"https://t.me/{bot.Username}?start=set_administrator";
                    await botClient.SendMessage(chatId, "Перешлите сгенерированную ссылку администратору");
                    await botClient.SendMessage(chatId, $"<a href=\"{link}\">Стать администратором GoodMoodPerfumeBot</a>", parseMode: ParseMode.Html);
                }
            }
            
        }

        private async Task SetAdminAsync(long chatId, long userId)
        {

            using (var scope = this.scopeFactory.CreateScope())
            {
                var commandService = scope.ServiceProvider.GetRequiredService<CommandService>();
                await commandService.SetAdminAsync(chatId, userId);
            }

        }

        private async Task SetOwnerAsync(long chatId, long userId)
        {
            using (var scope = this.scopeFactory.CreateScope() )
            {
                var commandService = scope.ServiceProvider.GetRequiredService<CommandService>();
                await commandService.SetOwnerAsync(userId, chatId);
            }
        }

        private async Task GetNotPayedOrdersAsync(ITelegramBotClient botClient, Message message)
        {
            using (var scope = this.scopeFactory.CreateScope())
            {
                if (!IsAdmin(scope, message.From.Id))
                    return;

                var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();

                var orders = await orderService.GetNotPayedOrdersAsync();

                if (orders.Count < 1)
                    await botClient.SendMessage(message.Chat.Id, "Неоплаченных заказов не найдено");

                foreach(var order in orders)
                {
                    var orderMessage = CreateOrderMessage(order, (long)order.AppUser.TelegramUserId);
                    await botClient.SendMediaGroup(message.Chat.Id, orderMessage.photos);
                    await botClient.SendMessage(message.Chat.Id, orderMessage.message, parseMode: ParseMode.Html, replyMarkup: orderMessage.keyboard);
                }
            }
        }

        private async Task GetNotShippedOrdersAsync(ITelegramBotClient botClient, Message message)
        {
            using (var scope = this.scopeFactory.CreateScope())
            {
                if (!IsAdmin(scope, message.From.Id))
                    return;

                var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();

                var orders = await orderService.GetNotShippedOrdersAsync();

                if (orders.Count < 1)
                    await botClient.SendMessage(message.Chat.Id, "Неотправленных заказов не найдено");

                foreach (var order in orders)
                {
                    var orderMessage = CreateOrderMessage(order, (long)order.AppUser.TelegramUserId);
                    await botClient.SendMediaGroup(message.Chat.Id, orderMessage.photos);
                    await botClient.SendMessage(message.Chat.Id, orderMessage.message, parseMode: ParseMode.Html, replyMarkup: orderMessage.keyboard);
                }
            }
        }

        private bool IsAdmin(IServiceScope scope, long userId)
        {
            var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
            var admin = userService.GetAdmin();
            if (admin == null || userId != admin.TelegramUserId)
                return false;

            return true;
        }
        private async Task CreateOrUpdatePaymentDetails(PaymentDetails details)
        {
            using (var scope = this.scopeFactory.CreateScope())
            {
                var payments = scope.ServiceProvider.GetRequiredService<PaymentDetailsService>();
                await payments.CreateOrUpdateAsync(details);
            }
        }
    }
}
