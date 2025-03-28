using GoodMoodPerfumeBot.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace GoodMoodPerfumeBot.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BotController : ControllerBase
    {
        private readonly ITelegramBotClient botClient;
        private readonly UpdateHandler updateHandler;

        public BotController(ITelegramBotClient client, UpdateHandler handler)
        {
            botClient = client;
            updateHandler = handler;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Update update, CancellationToken ct)
        {
            try
            {
                await updateHandler.HandleUpdateAsync(botClient, update, ct);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return Ok();
        }

    }
}
