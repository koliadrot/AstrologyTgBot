using Data.Core;
using Data.Core.Models;
using Microsoft.EntityFrameworkCore;
using Service.Abstract;
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
                        IsInfo = condition.IsInfo,

                    };
                    registerConditions.Add(newRegisterCondition);
                }

                var messages = new List<TelegramBotParamMessageViewModel>();
                var constructorMessages = new Dictionary<string, string>();

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
                        IsSystem = message.IsSystem
                    };
                    messages.Add(newMessage);
                    if (!message.IsSystem)
                    {
                        constructorMessages.Add(message.MessageName, message.MessageValue);
                    }
                }

                viewModel = new TelegramBotParamsViewModel
                {
                    TelegramBotId = telegramParams.TelegramBotId,
                    BotName = telegramParams.BotName,
                    BotAbout = telegramParams.BotAbout,
                    BotDescription = telegramParams.BotDescription,
                    BotUserName = telegramParams.BotUserName,
                    TokenApi = telegramParams.TokenApi,
                    WebHookUrl = telegramParams.WebHookUrl,
                    TosUrl = telegramParams.TosUrl,
                    BotCommands = botCommands,
                    RegisterConditions = registerConditions,
                    Messages = messages,
                    HelloText = telegramParams.HelloText,
                    Menu = telegramParams.Menu,
                    LastStatus = telegramParams.LastStatus,
                    ConstructorMessages = constructorMessages
                };
            }
            else
            {
                viewModel = new TelegramBotParamsViewModel();
            }

            return viewModel;
        }

        public void SaveStatusTelegramBot(TelegramBotStatusType status, int id = 0)
        {
            var telegramParams = id == 0 ? _bonusDbContext.TelegramParams.FirstOrDefault() : _bonusDbContext.TelegramParams.FirstOrDefault(x => x.TelegramBotId == id);
            if (telegramParams != null)
            {
                telegramParams.LastStatus = status.ToString();
            }
            _bonusDbContext.SaveChanges();
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
                    BotAbout = viewModel.BotAbout,
                    BotDescription = viewModel.BotDescription,
                    BotUserName = viewModel.BotUserName,
                    TokenApi = viewModel.TokenApi,
                    WebHookUrl = viewModel.WebHookUrl,
                    TosUrl = viewModel.TosUrl,
                    HelloText = viewModel.HelloText,
                };
                _bonusDbContext.TelegramParams.Add(entity);
            }
            else
            {
                entity.TelegramBotId = viewModel.TelegramBotId;
                entity.BotName = viewModel.BotName;
                entity.BotAbout = viewModel.BotAbout;
                entity.BotDescription = viewModel.BotDescription;
                if (isSuperAdmin)
                {
                    entity.BotUserName = viewModel.BotUserName;
                    entity.TokenApi = viewModel.TokenApi;
                    entity.WebHookUrl = viewModel.WebHookUrl;
                }
                entity.TosUrl = viewModel.TosUrl;
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
                if (viewModel.ConstructorMessages.Any())
                {

                    foreach (var item in _bonusDbContext.TelegramBotParamMessages.Where(x => !x.IsSystem))
                    {
                        if (!viewModel.ConstructorMessages.ContainsKey(item.MessageName))
                        {
                            _bonusDbContext.TelegramBotParamMessages.Remove(item);
                        }
                    }
                    foreach (var item in viewModel.ConstructorMessages)
                    {
                        TelegramBotParamMessage message = await _bonusDbContext.TelegramBotParamMessages.Where(x => !x.IsSystem).FirstOrDefaultAsync(x => x.MessageName == item.Key);
                        if (message == null)
                        {
                            TelegramBotParamMessageViewModel messageView = new TelegramBotParamMessageViewModel()
                            {
                                TelegramBotId = viewModel.TelegramBotId,
                                MessageName = item.Key,
                                MessageDescription = item.Key,
                                MessageValue = item.Value,
                                MessageValueDefault = item.Value
                            };
                            await CreateTelegramBotMessage(messageView);
                        }
                        else
                        {
                            message.MessageValue = item.Value;
                            message.MessageValueDefault = item.Value;
                        }
                    }
                }
                else
                {
                    foreach (var item in _bonusDbContext.TelegramBotParamMessages.Where(x => !x.IsSystem))
                    {
                        _bonusDbContext.TelegramBotParamMessages.Remove(item);
                    }
                }
                await _bonusDbContext.SaveChangesAsync();
            }
        }

        public async Task CreateTelegramBotMessage(TelegramBotParamMessageViewModel viewModel)
        {
            TelegramBotParamMessage message = _bonusDbContext.TelegramBotParamMessages.Find(viewModel.MessageId);
            var telegramBot = _bonusDbContext.TelegramParams.FirstOrDefault(x => x.TelegramBotId == viewModel.TelegramBotId);
            if (message == null && telegramBot != null)
            {
                message = new TelegramBotParamMessage()
                {
                    TelegramBotId = telegramBot.TelegramBotId,
                    MessageName = viewModel.MessageName,
                    MessageValue = viewModel.MessageValue,
                    MessageDescription = viewModel.MessageDescription,
                    MessageValueDefault = viewModel.MessageValueDefault
                };
                _bonusDbContext.TelegramBotParamMessages.Add(message);
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
                    condition.IsInfo = viewModel.IsInfo;
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
        #endregion
    }
}