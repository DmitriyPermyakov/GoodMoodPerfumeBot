using GoodMoodPerfumeBot.Models;
using GoodMoodPerfumeBot.UserRoles;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace GoodMoodPerfumeBot.Services
{
    public class CommandService
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly ITelegramBotClient botClient;

        public CommandService(IServiceScopeFactory factory, ITelegramBotClient botClient)
        {
            this.scopeFactory = factory;
            this.botClient = botClient;
        }


        public async Task SetFirstCommandAsync()
        {
            var command = new List<BotCommand>()
            {
                new BotCommand("/set_owner", "Установить владельца")
            };
            
            await botClient.SetMyCommands(command, new BotCommandScopeAllPrivateChats());
        }
        public async Task SetOwnerAsync(long chatId, long userId)
        {
            AppUser owner = null;

            using (var scope = scopeFactory.CreateScope())
            {
                IUserService userService = scope.ServiceProvider.GetService<IUserService>();
                owner = userService.GetAdmin();
                if(owner != null)                {
                    await this.botClient.SendMessage(userId, "Вы не можете установить владельца");
                } else
                {
                    await userService.CreateAsync(userId, chatId, SharedData.UserRoles.Owner);
                    await this.botClient.SendMessage(chatId, "Теперь вы владелец бота, и вам доступны новые команды!");
                    await SetOwnerCommandsAsync(chatId);
                }

            }
        }

        public async Task SetAdminAsync(long adminChatId, long adminUserId)
        {
            AppUser admin = null;
            AppUser owner = null;
            using (var scope = this.scopeFactory.CreateScope())
            {
                var userSerivce = scope.ServiceProvider.GetRequiredService<IUserService>();
                admin = userSerivce.GetAdmin();
                owner = userSerivce.GetOwner();
                if(admin != null)
                {
                    await botClient.SendMessage(owner.ChatId, "Удалите текущего администратора");
                    return;
                } else
                {
                    await userSerivce.UpdateUserAsync(adminUserId, adminChatId, SharedData.UserRoles.Administrator);
                    await botClient.SendMessage(owner.ChatId, "Администратор установлен");
                    await botClient.SendMessage(adminChatId, "Вы назначены администратором. Теперь вам доступны новые команды");
                    await SetAdminCommandsAsync(adminUserId);
                }
            }
        }

        public async Task DeleteAdminAsync(long ownerChatId)
        {
            AppUser admin = null;

            using (var scope = this.scopeFactory.CreateScope())
            {
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                admin = userService.GetAdmin();
                if(admin == null)
                {
                    await botClient.SendMessage(ownerChatId, "Администратор не был установлен");
                    return;
                } else
                {
                    long id = (long)admin.TelegramUserId;
                    await userService.UpdateUserAsync(id, admin.ChatId, SharedData.UserRoles.Member);
                    await botClient.SendMessage(ownerChatId, "Администратор был удален. Назначьте нового");
                    await botClient.SendMessage(admin.TelegramUserId, "Вы были удалены из администраторов");
                    await this.SetEmptyCommandsAsync(admin.ChatId);
                }
            }
        }

        private async Task SetEmptyCommandsAsync(long? chatId)
        {
            var commands = new List<BotCommand>();

            await botClient.SetMyCommands(commands, new BotCommandScopeChat() { ChatId = chatId });
        }

        private async Task SetOwnerCommandsAsync(long chatId)
        {
            var commands = new List<BotCommand>()
            {
                new BotCommand("/set_admin", "Установить администратора бота"),
                new BotCommand("/delete_admin", "Удалить администратора"),
                new BotCommand("/set_payment_details", "Установить платежные данные")
            };

            var scope = new BotCommandScopeChat()
            {
                ChatId = chatId
            };

            await botClient.SetMyCommands(commands, scope);
        }
        public async Task SetAdminCommandsAsync(long telegramUserId)
        {
            var commands = new List<BotCommand>()
            {
                new BotCommand("/get_not_payed", "Получить все неоплаченные заказы"),
                new BotCommand("/get_not_shipped", "Получить все неотправленные заказы")
            };
            var scope = new BotCommandScopeChat()
            {
                ChatId = telegramUserId
            };

            await botClient.SetMyCommands(commands, scope);
        }
    }
}
