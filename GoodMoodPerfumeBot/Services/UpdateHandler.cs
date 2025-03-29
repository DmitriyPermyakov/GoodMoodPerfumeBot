using GoodMoodPerfumeBot.Models;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace GoodMoodPerfumeBot.Services
{
    public class UpdateHandler
    {
        private readonly IServiceScopeFactory scopeFactory;

        public UpdateHandler(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }
        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            await (update switch
            {
                { Message: { } message } => OnMessage(botClient, message)
                
            });
        }

        private async Task OnMessage(ITelegramBotClient botClient, Message message)
        {
            string msg;
            Product product;
            var chatId = message.Chat.Id;

            using(var scope = this.scopeFactory.CreateScope())
            {
                var productService = scope.ServiceProvider.GetService<IProductService>();
                product = await productService.GetProductByIdAsync(1);
            }
            msg = $"Вы запросили продукт\n{product.Id}\n{product.Name}\n{product.Price}\n{product.ImageUrl}";
            await botClient.SendMessage(chatId, msg);
        }
    }
}
