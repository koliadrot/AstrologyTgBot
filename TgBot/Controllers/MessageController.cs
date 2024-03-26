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
    [Route(GlobalTelegramSettings.BASE)]
    public class MessageController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly TelegramBotManager _telegramBotManager;
        private readonly UpdateDistributor _updateDistributor;

        private int countConnections = 0;

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

        /// <summary>
        /// Получение обновление
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        [Route(GlobalTelegramSettings.UPDATE)]
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

        /// <summary>
        /// Повторное обновление
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [Route(GlobalTelegramSettings.REUPDATE)]
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

        /// <summary>
        /// Отправка сообщений
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(GlobalTelegramSettings.SEND)]
        public async Task<IActionResult> Send([FromBody] Update update, string password = "")
        {
            if (password == GlobalTelegramSettings.API_PASSWORD)
            {
                Message message = default;
                if (_telegramBotManager.IsStarted && !_telegramBotManager.IsDropPendingUpdates)
                {
                    if (await _updateDistributor.HasSupportCommands(update))
                    {
                        message = await _updateDistributor.ExecuteSupportCommands(update);
                    }
                    else if (await _updateDistributor.HasNotify(update))
                    {
                        message = await _updateDistributor.SendNotify(update);
                    }
                    else
                    {
                        long chatId = Get.GetChatId(update);
                        string messageText = Get.GetText(update);
                        message = await _telegramBotManager.SendTextMessage(chatId, messageText);
                    }
                }
                _updateDistributor.Dispose(update);

                return Ok(message);
            }
            else
            {
                _logger.Error("Failed send manual message TG bot! Wrong password!");
                return StatusCode(((int)HttpStatusCode.Forbidden));
            }
        }
    }
}
