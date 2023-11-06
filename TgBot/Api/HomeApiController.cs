namespace TgBot.Api
{
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using Service.Abstract;
    using Service.Core.TelegramBot;
    using Service.Extensions;
    using Service.ViewModels.TelegramModels;

    [Authorize]
    public class HomeApiController : Controller
    {
        private readonly ISettingsManager _settingsManager;

        public HomeApiController(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }

        [HttpPost]
        public async Task<JsonResult> UpdateTelegramBot(TelegramBotParamsViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                    return Json(new { success = false, errors });
                }
                //bool isSuperAdmin = _accessManager.CheckUserPermission(User.Identity.Name, nameof(AdminController));
                bool isSuperAdmin = true;
                await _settingsManager.UpdateTelegramBot(viewModel, isSuperAdmin);
                return await SendPostTelegramBot(viewModel.TelegramBotId, GlobalTelegramSettings.RESET_BOT);
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
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                    return Json(new { success = false, errors });
                }
                await _settingsManager.UpdateTelegramBotMenu(viewModel);
                return await SendPostTelegramBot(viewModel.TelegramBotId, GlobalTelegramSettings.RESET_BOT);
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
                    await _settingsManager.SendPostTelegramBot(telegramBot.WebHookUrl, GlobalTelegramSettings.RESET_BOT);
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
                    await _settingsManager.SendPostTelegramBot(telegramBot.WebHookUrl, GlobalTelegramSettings.RESET_BOT);
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
                    await _settingsManager.SendPostTelegramBot(telegramBot.WebHookUrl, GlobalTelegramSettings.RESET_BOT);
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
                return await SendPostTelegramBot(telegramBotId, GlobalTelegramSettings.START_BOT);
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
                return await SendPostTelegramBot(telegramBotId, GlobalTelegramSettings.STOP_BOT);
            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = $"Произошла ошибка при остановке телеграмм бота." });
            }
        }

        [HttpPost]
        public async Task<JsonResult> ResetTelegramBot(int telegramBotId)
        {
            try
            {
                return await SendPostTelegramBot(telegramBotId, GlobalTelegramSettings.RESET_BOT);
            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = $"Произошла ошибка при перезагрузке телеграмм бота." });
            }
        }

        private async Task<JsonResult> SendPostTelegramBot(int telegramBotId, string route)
        {
            using (var client = new HttpClient())
            {
                var telegramBot = _settingsManager.GetTelegramBot(telegramBotId);
                var response = await _settingsManager.SendPostTelegramBot(telegramBot.WebHookUrl, route);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeAnonymousType(content, new { isStarted = false });

                    return Json(new { success = true, result });
                }
                else
                {
                    return Json(new { success = false });
                }
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
                    await _settingsManager.SendPostTelegramBot(telegramBot.WebHookUrl, GlobalTelegramSettings.RESET_BOT);
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
                var result = _settingsManager.GetTelegramBotMessages().Where(x => !x.IsButton);
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
                    await _settingsManager.SendPostTelegramBot(telegramBot.WebHookUrl, GlobalTelegramSettings.RESET_BOT);
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
