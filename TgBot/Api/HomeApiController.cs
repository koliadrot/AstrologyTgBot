namespace TgBot.Api
{
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Service.Abstract;
    using Service.Core.TelegramBot;
    using Service.Extensions;
    using Service.ViewModels.TelegramModels;
    using ILogger = NLog.ILogger;

    [Authorize]
    public class HomeApiController : Controller
    {
        private readonly ILogger _logger;
        private readonly ISettingsManager _settingsManager;
        private readonly TelegramBotManager _telegramBotManager;
        private readonly UpdateDistributor _updateDistributor;

        public HomeApiController(ISettingsManager settingsManager, TelegramBotManager telegramBotManager, UpdateDistributor updateDistributor, ICustomerManager customerManager, ILogger logger)
        {
            _logger = logger;
            _settingsManager = settingsManager;
            _telegramBotManager = telegramBotManager;
            _updateDistributor = updateDistributor;
            _updateDistributor.Init(customerManager, settingsManager, telegramBotManager, logger);
        }

        [HttpPost]
        public async Task<JsonResult> UpdateTelegramBot(TelegramBotParamsViewModel viewModel)
        {
            try
            {
                //bool isSuperAdmin = _accessManager.CheckUserPermission(User.Identity.Name, nameof(AdminController));
                bool isSuperAdmin = true;
                await _settingsManager.UpdateTelegramBot(viewModel, isSuperAdmin);
                return await ResetTelegramBot();
            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = "Произошла ошибка при обновлении параметров телеграмм бота." });
            }
        }

        [HttpPost]
        public async Task<JsonResult> UpdateTelegramBotMenu(TelegramBotParamsViewModel viewModel)
        {
            try
            {
                await _settingsManager.UpdateTelegramBotMenu(viewModel);
                return await ResetTelegramBot();
            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = "Произошла ошибка при обновлении меню телеграмм бота." });
            }
        }

        public ActionResult GetTelegramBotCommands([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var result = _settingsManager.GetTelegramBotCommands();
                return Json(result.ToDataSourceResult(request));
            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = "Произошла ошибка при получении команд телеграмм бота." });
            }
        }

        [HttpPost]
        public async Task<JsonResult> UpdateTelegramBotCommand(TelegramBotCommandViewModel viewModel, [DataSourceRequest] DataSourceRequest dataSourceRequest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var telegramBot = _settingsManager.GetTelegramBot(viewModel.TelegramBotId);
                    var currentCommand = telegramBot.BotCommands.FirstOrDefault(x => x.BotCommandId == viewModel.BotCommandId);
                    await _settingsManager.UpdateTelegramBotCommand(viewModel);
                    await ResetTelegramBot();
                }

                var resultData = new[] { viewModel };
                return Json(resultData.AsQueryable().ToDataSourceResult(dataSourceRequest, ModelState));

            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = $"Произошла ошибка при обновлении команды {viewModel.Name} у телеграмм бота." }); ;
            }
        }

        [HttpPost]
        public async Task<JsonResult> CreateTelegramBotCommand(int telegramBotId, TelegramBotCommandViewModel viewModel, [DataSourceRequest] DataSourceRequest dataSourceRequest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    viewModel.TelegramBotId = telegramBotId;
                    await _settingsManager.CreateTelegramBotCommand(viewModel);

                    var telegramBot = _settingsManager.GetTelegramBot(telegramBotId);
                    if (viewModel.CommandName.IsNull())
                    {
                        var currentCommand = telegramBot.BotCommands.FirstOrDefault(x => x.CommandName.IsNull());
                        if (currentCommand != null)
                        {
                            viewModel.BotCommandId = currentCommand.BotCommandId;
                            viewModel.CommandName = $"/{currentCommand.CommandType.ToLower()}{currentCommand.BotCommandId}";
                            await _settingsManager.UpdateTelegramBotCommand(viewModel);
                        }
                    }
                    await ResetTelegramBot();
                }

                var resultData = new[] { viewModel };
                return Json(resultData.AsQueryable().ToDataSourceResult(dataSourceRequest, ModelState));

            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = $"Произошла ошибка при обновлении команды {viewModel.Name} у телеграмм бота." });
            }
        }

        [HttpPost]
        public async Task<JsonResult> DeleteTelegramBotCommand(TelegramBotCommandViewModel viewModel, [DataSourceRequest] DataSourceRequest dataSourceRequest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _settingsManager.DeleteTelegramBotCommand(viewModel);
                    var telegramBot = _settingsManager.GetTelegramBot(viewModel.TelegramBotId);
                    await ResetTelegramBot();
                }

                var resultData = new[] { viewModel };
                return Json(resultData.AsQueryable().ToDataSourceResult(dataSourceRequest, ModelState));

            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = $"Произошла ошибка при обновлении команды {viewModel.Name} у телеграмм бота." });
            }
        }

        [HttpPost]
        public async Task<JsonResult> StartTelegramBot(int telegramBotId)
        {
            try
            {
                await _telegramBotManager.Start();
                bool isStarted = _telegramBotManager.IsStarted;
                return Json(new { success = true, isStarted });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Произошла ошибка при старте телеграмм бота." });
            }
        }

        [HttpPost]
        public async Task<JsonResult> StopTelegramBot(int telegramBotId)
        {
            try
            {
                _telegramBotManager.Stop();
                _updateDistributor.ClearAllListener();
                bool isStarted = _telegramBotManager.IsStarted;
                return Json(new { success = true, isStarted });
            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = $"Произошла ошибка при остановке телеграмм бота." });
            }
        }

        [HttpPost]
        public async Task<JsonResult> ResetTelegramBot(int telegramBotId = 1)
        {
            try
            {
                await _telegramBotManager.Reset();
                await _updateDistributor.InitAllListener();
                bool isStarted = _telegramBotManager.IsStarted;
                return Json(new { success = true, isStarted });
            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = $"Произошла ошибка при перезагрузке телеграмм бота." });
            }
        }

        public ActionResult GetTelegramBotRegisterConditions([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var result = _settingsManager.GetTelegramBotRegisterConditions();
                return Json(result.ToDataSourceResult(request));
            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = "Произошла ошибка при получении условий регистрации телеграмм бота." });
            }
        }


        [HttpPost]
        public async Task<JsonResult> UpdateTelegramBotRegisterCondition(TelegramBotRegisterConditionViewModel viewModel, [DataSourceRequest] DataSourceRequest dataSourceRequest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var telegramBot = _settingsManager.GetTelegramBot(viewModel.TelegramBotId);
                    var currentCondition = telegramBot.RegisterConditions.FirstOrDefault(x => x.RegisterConditionId == viewModel.RegisterConditionId);
                    await _settingsManager.UpdateTelegramBotRegisterCondition(viewModel);
                    await ResetTelegramBot();
                }

                var resultData = new[] { viewModel };
                return Json(resultData.AsQueryable().ToDataSourceResult(dataSourceRequest, ModelState));

            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = $"Произошла ошибка при обновлении условия регистрации {viewModel.Name} у телеграмм бота." }); ;
            }
        }

        public ActionResult GetTelegramBotParamMessages([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var result = _settingsManager.GetTelegramBotMessages().Where(x => !x.IsButton && x.IsSystem);
                return Json(result.ToDataSourceResult(request));
            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = "Произошла ошибка при получении сообщений телеграмм бота." });
            }
        }
        public ActionResult GetTelegramBotParamButtons([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var result = _settingsManager.GetTelegramBotMessages().Where(x => x.IsButton);
                return Json(result.ToDataSourceResult(request));
            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = "Произошла ошибка при получении кнопок телеграмм бота." });
            }
        }

        public ActionResult GetTelegramBotParamUserMessages([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var result = _settingsManager.GetTelegramBotMessages().Where(x => !x.IsButton && !x.IsSystem);
                return Json(result.ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                return Json(new { success = false, message = "Произошла ошибка при получении пользовательских сообщений телеграмм бота." });
            }
        }

        [HttpPost]
        public async Task<JsonResult> UpdateTelegramBotMessage(TelegramBotParamMessageViewModel viewModel, [DataSourceRequest] DataSourceRequest dataSourceRequest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var telegramBot = _settingsManager.GetTelegramBot(viewModel.TelegramBotId);
                    var currentCondition = telegramBot.Messages.FirstOrDefault(x => x.MessageId == viewModel.MessageId);
                    await _settingsManager.UpdateTelegramBotMessage(viewModel);
                    await ResetTelegramBot();
                }

                var resultData = new[] { viewModel };
                return Json(resultData.AsQueryable().ToDataSourceResult(dataSourceRequest, ModelState));

            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = $"Произошла ошибка при обновлении сообщения {viewModel.MessageDescription} у телеграмм бота." }); ;
            }
        }
    }
}
