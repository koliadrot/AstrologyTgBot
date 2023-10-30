using Data.Core;
using Data.Core.Models;
using Service.Abstract;
using Service.Core.TelegramBot;
using Service.Enums;
using Service.Extensions;
using Service.ViewModels.TelegramModels;

namespace Service.Core
{
    public class SettingsManager : ISettingsManager, IDisposable
    {
        private readonly ApplicationDbContext _bonusDbContext;

        public SettingsManager()
        {
            _bonusDbContext = new ApplicationDbContext();
        }

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _bonusDbContext.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~SettingsManager() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion

        #region TelegramBot
        public TelegramBotParamsViewModel GetTelegramBot(int id = 0)
        {
            var telegramParams = id == 0 ? _bonusDbContext.TelegramParams.FirstOrDefault() : _bonusDbContext.TelegramParams.FirstOrDefault(x => x.TelegramBotId == id);
            TelegramBotParamsViewModel viewModel;

            if (telegramParams != null)
            {
                telegramParams.BotCommands = _bonusDbContext.TelegramBotCommands.ToList();
                telegramParams.Messages = _bonusDbContext.TelegramBotParamMessages.ToList();
                telegramParams.BotRegisterConditions = _bonusDbContext.TelegramBotRegisterConditions.ToList();

                var botCommands = new List<TelegramBotCommandViewModel>();
                foreach (var command in telegramParams.BotCommands)
                {
                    var newBotCommand = new TelegramBotCommandViewModel()
                    {
                        BotCommandId = command.BotCommandId,
                        TelegramBotId = command.TelegramBotId,
                        Name = command.Name,
                        Description = command.Description,
                        CommandName = command.CommandName,
                        IsEnable = command.IsEnable,
                        CommandType = command.CommandType,
                        IsAuth = command.IsAuth,
                        IsDefault = command.IsDefault,
                        IsPublic = command.IsPublic,
                        AdditionalData = command.AdditionalData,
                    };
                    botCommands.Add(newBotCommand);
                }

                var registerConditions = new List<TelegramBotRegisterConditionViewModel>();
                foreach (var condition in telegramParams.BotRegisterConditions)
                {
                    var newRegisterCondition = new TelegramBotRegisterConditionViewModel()
                    {
                        TelegramBotId = condition.TelegramBotId,
                        RegisterConditionId = condition.RegisterConditionId,
                        IsCanPass = condition.IsCanPass,
                        Name = condition.Name,
                        ConditionName = condition.ConditionName,
                        Order = condition.Order,
                        IsNecessarily = condition.IsNecessarily,
                        IsEnable = condition.IsEnable,

                    };
                    registerConditions.Add(newRegisterCondition);
                }

                var messages = new List<TelegramBotParamMessageViewModel>();

                foreach (var message in telegramParams.Messages)
                {
                    var newMessage = new TelegramBotParamMessageViewModel()
                    {
                        TelegramBotId = message.TelegramBotId,
                        MessageId = message.MessageId,
                        MessageDescription = message.MessageDescription,
                        MessageName = message.MessageName,
                        MessageValue = message.MessageValue,
                        MessageValueDefault = message.MessageValueDefault,
                        IsButton = message.IsButton,

                    };
                    messages.Add(newMessage);
                }

                viewModel = new TelegramBotParamsViewModel
                {
                    TelegramBotId = telegramParams.TelegramBotId,
                    BotName = telegramParams.BotName,
                    BotUserName = telegramParams.BotUserName,
                    TokenApi = telegramParams.TokenApi,
                    WebHookUrl = telegramParams.WebHookUrl,
                    TosUrl = telegramParams.TosUrl,
                    AcceptElectronicReceipts = telegramParams.AcceptElectronicReceipts,
                    AcceptPromotionsBySms = telegramParams.AcceptPromotionsBySms,
                    BotCommands = botCommands,
                    RegisterConditions = registerConditions,
                    Messages = messages,
                    HelloText = telegramParams.HelloText,
                    Menu = telegramParams.Menu,
                };
            }
            else
            {
                viewModel = new TelegramBotParamsViewModel();
            }

            return viewModel;
        }

        public async Task UpdateTelegramBot(TelegramBotParamsViewModel viewModel, bool isSuperAdmin)
        {
            TelegramBotParams entity = _bonusDbContext.TelegramParams.Find(viewModel.TelegramBotId);
            if (entity == null)
            {
                entity = new TelegramBotParams()
                {
                    TelegramBotId = viewModel.TelegramBotId,
                    BotName = viewModel.BotName,
                    BotUserName = viewModel.BotUserName,
                    TokenApi = viewModel.TokenApi,
                    WebHookUrl = viewModel.WebHookUrl,
                    TosUrl = viewModel.TosUrl,
                    AcceptElectronicReceipts = viewModel.AcceptElectronicReceipts,
                    AcceptPromotionsBySms = viewModel.AcceptPromotionsBySms,
                    HelloText = viewModel.HelloText,
                };
                _bonusDbContext.TelegramParams.Add(entity);
            }
            else
            {
                entity.TelegramBotId = viewModel.TelegramBotId;
                entity.BotName = viewModel.BotName;
                if (isSuperAdmin)
                {
                    entity.BotUserName = viewModel.BotUserName;
                    entity.TokenApi = viewModel.TokenApi;
                    entity.WebHookUrl = viewModel.WebHookUrl;
                }
                entity.TosUrl = viewModel.TosUrl;
                entity.AcceptPromotionsBySms = viewModel.AcceptPromotionsBySms;
                entity.AcceptElectronicReceipts = viewModel.AcceptElectronicReceipts;
                entity.HelloText = viewModel.HelloText;
            }
            await _bonusDbContext.SaveChangesAsync();
        }

        public async Task UpdateTelegramBotMenu(TelegramBotParamsViewModel viewModel)
        {
            TelegramBotParams entity = _bonusDbContext.TelegramParams.Find(viewModel.TelegramBotId);
            if (entity != null)
            {
                entity.Menu = viewModel.Menu;
                await _bonusDbContext.SaveChangesAsync();
            }
        }

        public ICollection<TelegramBotCommandViewModel> GetTelegramBotCommands(int id = 0)
        {
            TelegramBotParamsViewModel telegramBot = GetTelegramBot(id);
            return telegramBot.BotCommands;
        }

        public ICollection<TelegramBotRegisterConditionViewModel> GetTelegramBotRegisterConditions(int id = 0)
        {
            TelegramBotParamsViewModel telegramBot = GetTelegramBot(id);
            return telegramBot.RegisterConditions;
        }

        public async Task UpdateTelegramBotRegisterCondition(TelegramBotRegisterConditionViewModel viewModel)
        {
            TelegramBotRegisterCondition condition = _bonusDbContext.TelegramBotRegisterConditions.Find(viewModel.RegisterConditionId);
            var telegramBot = _bonusDbContext.TelegramParams.FirstOrDefault(x => x.TelegramBotId == viewModel.TelegramBotId);
            if (condition != null && telegramBot != null)
            {
                condition.Name = viewModel.Name;
                if (!condition.IsNecessarily)
                {
                    condition.IsCanPass = viewModel.IsCanPass;
                    condition.Order = Mathf.Clamp(viewModel.Order, 0, 999);
                    condition.IsEnable = viewModel.IsEnable;
                }
                await _bonusDbContext.SaveChangesAsync();
            }
        }

        public async Task UpdateTelegramBotMessage(TelegramBotParamMessageViewModel viewModel)
        {
            TelegramBotParamMessage message = _bonusDbContext.TelegramBotParamMessages.Find(viewModel.MessageId);
            var telegramBot = _bonusDbContext.TelegramParams.FirstOrDefault(x => x.TelegramBotId == viewModel.TelegramBotId);
            if (message != null && telegramBot != null)
            {
                message.MessageValue = viewModel.MessageValue;
                await _bonusDbContext.SaveChangesAsync();
            }
        }
        public ICollection<TelegramBotParamMessageViewModel> GetTelegramBotMessages(int id = 0)
        {
            TelegramBotParamsViewModel telegramBot = GetTelegramBot(id);
            return telegramBot.Messages;
        }

        public async Task CreateTelegramBotCommand(TelegramBotCommandViewModel viewModel)
        {
            TelegramBotCommand command = _bonusDbContext.TelegramBotCommands.Find(viewModel.BotCommandId);
            var telegramBot = _bonusDbContext.TelegramParams.FirstOrDefault(x => x.TelegramBotId == viewModel.TelegramBotId);
            if (command == null && telegramBot != null)
            {
                var newBotCommand = new TelegramBotCommand()
                {
                    TelegramBotId = viewModel.TelegramBotId,
                    Name = viewModel.Name,
                    Description = viewModel.Description,
                    CommandName = viewModel.CommandName,
                    IsAuth = viewModel.IsAuth,
                    IsDefault = viewModel.IsDefault,
                    CommandType = viewModel.CommandType,
                    IsEnable = viewModel.IsEnable,
                    IsPublic = true,
                    AdditionalData = viewModel.AdditionalData,
                };
                _bonusDbContext.TelegramBotCommands.Add(newBotCommand);
                await _bonusDbContext.SaveChangesAsync();
            }
        }

        public async Task UpdateTelegramBotCommand(TelegramBotCommandViewModel viewModel)
        {
            TelegramBotCommand command = _bonusDbContext.TelegramBotCommands.Find(viewModel.BotCommandId);
            var telegramBot = _bonusDbContext.TelegramParams.FirstOrDefault(x => x.TelegramBotId == viewModel.TelegramBotId);
            if (command != null && telegramBot != null)
            {
                command.Name = viewModel.Name;
                command.Description = viewModel.Description;
                command.CommandName = viewModel.CommandName;
                if (command.CommandType != TelegramBotCommandType.Custom.ToString())
                {
                    command.IsAuth = viewModel.IsAuth;
                    command.IsDefault = viewModel.IsDefault;
                }
                if (command.IsPublic)
                {
                    command.IsEnable = viewModel.IsEnable;
                }
                command.CommandType = viewModel.CommandType;
                command.AdditionalData = viewModel.AdditionalData;
                await _bonusDbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteTelegramBotCommand(TelegramBotCommandViewModel viewModel)
        {
            TelegramBotCommand command = _bonusDbContext.TelegramBotCommands.Find(viewModel.BotCommandId);
            var telegramBot = _bonusDbContext.TelegramParams.FirstOrDefault(x => x.TelegramBotId == viewModel.TelegramBotId);
            if (command != null && telegramBot != null && command.IsPublic)
            {
                _bonusDbContext.TelegramBotCommands.Remove(command);
                await _bonusDbContext.SaveChangesAsync();
            }
        }

        public async Task<HttpResponseMessage> SendPostTelegramBot(string url, string route)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    if (!url.IsNull() && url.EndsWith("/"))
                    {
                        url = url.TrimEnd('/');
                    }
                    string post = $"{url}/{route}?password={GlobalTelegramSettings.API_PASSWORD}";
                    var response = await client.GetAsync(post);
                    return response;
                }
            }
            catch (InvalidOperationException ex)
            {
                return new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.BadRequest };
            }
        }
        #endregion
    }
}