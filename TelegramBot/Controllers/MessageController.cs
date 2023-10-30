using Microsoft.AspNetCore.Mvc;
using Service.Abstract;
using Service.Core.TelegramBot;
using Telegram.Bot.Types;

namespace TelegramBot.Controllers
{
    [ApiController]
    [Route(GlobalTelegramSettings.BASE_MESSAGE)]
    public class MessageController : ControllerBase
    {
        private readonly TelegramBotManager _telegramBotManager;
        private readonly UpdateDistributor _updateDistributor;

        public MessageController(
            ICustomerManager customerManager,
            ISettingsManager settingsManager,
            TelegramBotManager telegramBotManager,
            UpdateDistributor updateDistributor
        )
        {
            _updateDistributor = updateDistributor;
            _updateDistributor.Init(customerManager, settingsManager, telegramBotManager);
            _telegramBotManager = telegramBotManager;
        }

        [Route(GlobalTelegramSettings.UPDATE_MESSAGE)]
        public async Task<IActionResult> Update(Update update)
        {
            if (_telegramBotManager.IsStarted && !_telegramBotManager.IsDropPendingUpdates)
            {
                await _updateDistributor.GetUpdate(update);
            }
            _updateDistributor.Dispose(update);
            return Ok();
        }

        [Route("test")]
        public async Task<IActionResult> Test()
        {
            return Ok();
        }
    }
}
