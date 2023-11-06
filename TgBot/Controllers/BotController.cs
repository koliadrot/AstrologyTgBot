namespace TgBot.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Service.Abstract;
    using Service.Core.TelegramBot;
    using System.Net;

    [ApiController]
    [Route(GlobalTelegramSettings.BASE_BOT)]
    public class BotController : ControllerBase
    {
        private readonly TelegramBotManager _telegramBotManager;
        private readonly UpdateDistributor _updateDistributor;

        public BotController(
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

        [HttpGet]
        [Route(GlobalTelegramSettings.START_BOT)]
        public async Task<IActionResult> Start(string password)
        {
            if (password == GlobalTelegramSettings.API_PASSWORD)
            {
                await _telegramBotManager.Start();
                bool isStarted = _telegramBotManager.IsStarted;
                return Ok(new { isStarted });
            }
            else
            {
                return StatusCode((int)HttpStatusCode.Forbidden);
            }
        }

        [HttpGet]
        [Route(GlobalTelegramSettings.RESET_BOT)]
        public async Task<IActionResult> Reset(string password)
        {
            if (password == GlobalTelegramSettings.API_PASSWORD)
            {
                await _telegramBotManager.Reset();
                await _updateDistributor.InitAllListener();
                bool isStarted = _telegramBotManager.IsStarted;
                return Ok(new { isStarted });
            }
            else
            {
                return StatusCode((int)HttpStatusCode.Forbidden);
            }
        }

        [HttpGet]
        [Route(GlobalTelegramSettings.STOP_BOT)]
        public IActionResult Stop(string password)
        {
            if (password == GlobalTelegramSettings.API_PASSWORD)
            {
                _telegramBotManager.Stop();
                _updateDistributor.ClearAllListener();
                bool isStarted = _telegramBotManager.IsStarted;
                return Ok(new { isStarted });
            }
            else
            {
                return StatusCode((int)HttpStatusCode.Forbidden);
            }
        }

        [HttpGet]
        [Route(GlobalTelegramSettings.GET_STATE_BOT)]
        public IActionResult GetState(string password)
        {
            if (password == GlobalTelegramSettings.API_PASSWORD)
            {
                bool isStarted = _telegramBotManager.IsStarted;
                return Ok(new { isStarted });
            }
            else
            {
                return StatusCode((int)HttpStatusCode.Forbidden);
            }
        }
    }
}
