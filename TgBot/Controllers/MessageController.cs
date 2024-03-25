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

        /// <summary>
        /// Повторное обновление
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Отправка сообщений
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(GlobalTelegramSettings.SEND_MESSAGE)]
        public async Task<IActionResult> Send([FromBody] Update update, string password = "")
        {
            if (password == GlobalTelegramSettings.API_PASSWORD)
            {
                Message message = default;
                if (_telegramBotManager.IsStarted && !_telegramBotManager.IsDropPendingUpdates)
                {
                    long chatId = Get.GetChatId(update);
                    string messageText = Get.GetText(update);
                    message = await _telegramBotManager.SendTextMessage(chatId, messageText);
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

        /// <summary>
        /// Отправка уведомления об предложении посмотреть анкеты
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(GlobalTelegramSettings.OFFER_SHOW_FIND_CLIENTS_NOTIFY)]
        public async Task<IActionResult> OfferShowFindClientsNotify([FromBody] Update update, string password = "")
        {
            if (password == GlobalTelegramSettings.API_PASSWORD)
            {
                Message message = default;
                if (_telegramBotManager.IsStarted && !_telegramBotManager.IsDropPendingUpdates && await _updateDistributor.HasNotify(update))
                {
                    message = await _updateDistributor.SendNotify(update);
                }
                _updateDistributor.Dispose(update);

                return Ok(message);
            }
            else
            {
                _logger.Error($"Failed send {nameof(OfferShowFindClientsNotify)} notify TG bot! Wrong password!");
                return StatusCode(((int)HttpStatusCode.Forbidden));
            }
        }

        /// <summary>
        /// Отправка уведомления о не просмотренных лайках
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(GlobalTelegramSettings.NEW_LIKES_NOTIFY)]
        public async Task<IActionResult> NewLikesNotify([FromBody] Update update, string password = "")
        {
            if (password == GlobalTelegramSettings.API_PASSWORD)
            {
                Message message = default;
                if (_telegramBotManager.IsStarted && !_telegramBotManager.IsDropPendingUpdates && await _updateDistributor.HasNotify(update))
                {
                    message = await _updateDistributor.SendNotify(update);
                }
                _updateDistributor.Dispose(update);

                return Ok(message);
            }
            else
            {
                _logger.Error($"Failed send {nameof(NewLikesNotify)} notify TG bot! Wrong password!");
                return StatusCode(((int)HttpStatusCode.Forbidden));
            }
        }

        /// <summary>
        /// Пересобирает список актуальных клиентов анкет
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(GlobalTelegramSettings.RECOLLECT_FIND_CLIENTS)]
        public async Task<IActionResult> RecollectFindClients([FromBody] Update update, string password = "")
        {
            if (password == GlobalTelegramSettings.API_PASSWORD)
            {
                if (_telegramBotManager.IsStarted && !_telegramBotManager.IsDropPendingUpdates && await _updateDistributor.HasSupportCommands(update))
                {
                    await _updateDistributor.ExecuteSupportCommands(update);
                }
                _updateDistributor.Dispose(update);

                return Ok();
            }
            else
            {
                _logger.Error($"Failed {nameof(RecollectFindClients)} TG bot! Wrong password!");
                return StatusCode(((int)HttpStatusCode.Forbidden));
            }
        }
    }
}
