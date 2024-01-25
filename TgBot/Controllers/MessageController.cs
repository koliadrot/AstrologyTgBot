namespace TgBot.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Service;
    using Service.Abstract;
    using Service.Core.TelegramBot;
    using System.Net;
    using Telegram.Bot.Types;
    using ILogger = NLog.ILogger;

    [ApiController]
    [Route(GlobalTelegramSettings.BASE_MESSAGE)]
    public class MessageController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly TelegramBotManager _telegramBotManager;
        private readonly UpdateDistributor _updateDistributor;

        public MessageController(
            ICustomerManager customerManager,
            ISettingsManager settingsManager,
            TelegramBotManager telegramBotManager,
            UpdateDistributor updateDistributor,
            ILogger logger
        )
        {
            _updateDistributor = updateDistributor;
            _updateDistributor.Init(customerManager, settingsManager, telegramBotManager, logger);
            _telegramBotManager = telegramBotManager;
            _logger = logger;
        }

        [Route(GlobalTelegramSettings.UPDATE_MESSAGE)]
        public async Task<IActionResult> Update(Update update)
        {
            if (_telegramBotManager.IsStarted && !_telegramBotManager.IsDropPendingUpdates && !Get.IsBot(update))
            {
                await _updateDistributor.GetUpdate(update);
                _updateDistributor.Dispose(update);
                return Ok();
            }
            else
            {
                _logger.Error("Failed update message TG bot!");
                return StatusCode(((int)HttpStatusCode.Forbidden));
            }
        }

        [Route(GlobalTelegramSettings.RE_UPDATE_MESSAGE)]
        public async Task<IActionResult> Reupdate(long userId, string password)
        {
            if (_telegramBotManager.IsStarted && !_telegramBotManager.IsDropPendingUpdates && password == GlobalTelegramSettings.API_PASSWORD)
            {
                await _updateDistributor.ReUpdate(userId);
                _updateDistributor.Dispose(userId);
                return Ok();
            }
            else
            {
                _logger.Error("Failed reUpdate message TG bot!");
                return StatusCode(((int)HttpStatusCode.Forbidden));
            }
        }

    }
}
