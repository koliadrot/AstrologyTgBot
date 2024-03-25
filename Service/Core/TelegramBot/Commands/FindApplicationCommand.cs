namespace Service.Core.TelegramBot.Commands
{
    using Service.Abstract;
    using Service.Abstract.Filtrable;
    using Service.Abstract.TelegramBot;
    using Service.Core.TelegramBot.FindCondition;
    using Service.Core.TelegramBot.Notifies;
    using Service.Enums;
    using Service.Extensions;
    using Service.Support;
    using Service.ViewModels;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Telegram.Bot.Types;
    using Telegram.Bot.Types.ReplyMarkups;

    /// <summary>
    /// Поиск анкет
    /// </summary>
    public class FindApplicationCommand : ICommand, IUpdater, IListener, ISupportCommandExecutor
    {
        public string Name { get; set; } = "/findapplication";

        public string Description { get; set; } = "Просмотр анкет";

        public string ShortDescription { get; set; } = "Анкеты";

        public bool IsAuth { get; set; } = true;
        public bool IsDefault { get; set; } = false;

        public TelegramBotCommandType CommandType => TelegramBotCommandType.Custom;
        public bool IsStartMenu => false;

        private IUpdater _updater;
        public IUpdater CurrentUpdater => _updater;

        public string Key => nameof(FindApplicationCommand);

        private List<ICondition> _conditions = new List<ICondition>();

        private bool _isInitFindClients = false;

        private ReplyKeyboardMarkup _replyKeyboard;
        private List<string> _supportMiniComands = new List<string>();
        private List<IClientFitrable> _clientFilters = new List<IClientFitrable>();

        private NewLikesNotify? _newLikesNotify;
        private ClientViewModel? _currentClient;
        private ClientViewModel? _myClient;
        private List<ClientViewModel> _findClients = new List<ClientViewModel>();

        private readonly DataManager _dataManager;
        private readonly Dictionary<string, string> _messages;
        private readonly TelegramSupport _telegramSupport;

        public FindApplicationCommand(DataManager dataManager)
        {
            _dataManager = dataManager;
            _messages = _dataManager.GetData<CommandExecutor>().Messages;
            _telegramSupport = new TelegramSupport(dataManager);
            _newLikesNotify = _dataManager.GetData<CommandExecutor>()?.GetNotifyByType(typeof(NewLikesNotify)) as NewLikesNotify;

            _supportMiniComands.Add(_messages[ReplyButton.Like]);
            _supportMiniComands.Add(_messages[ReplyButton.LoveLetter]);
            _supportMiniComands.Add(_messages[ReplyButton.Dislike]);
            _supportMiniComands.Add(_messages[ReplyButton.Profile]);
        }

        public async Task SendStartMessage(Update update)
        {
            long chatId = Get.GetChatId(update);

            if (_messages.TryGetValue(ShortDescription, out string startMessage))
            {
                await _dataManager.GetData<TelegramBotManager>().SendTextMessage(chatId, startMessage, replyMarkup: GetReplyMarkup());
            }
            else
            {
                await _dataManager.GetData<TelegramBotManager>().SendTextMessage(chatId, _messages[MessageKey.DEFAULT_FIND_APPLICATION], replyMarkup: GetReplyMarkup());
            }
        }
        public async Task Execute(Update update, string[] arg = null)
        {
            long userId = Get.GetUserId(update);
            if (_dataManager.GetData<ICustomerManager>().ExistTelegram(userId))
            {
                _dataManager.GetData<CommandExecutor>().StartListen(this);
                await SendStartMessage(update);
                _isInitFindClients = false;
                _currentClient = null;
                InitFindClients(update);
                await SendNextApplication(update);
            }
        }

        private void InitFindClients(Update update, ClientViewModel? excludeClientViewModel = null)
        {
            if (_findClients == null || !_findClients.Any() || !_isInitFindClients)
            {
                _findClients = _dataManager.GetData<ICustomerManager>().GetClients(excludeClientViewModel);
                CollectClientFilter(update);
                _findClients = _findClients.Filter(_clientFilters).ToList();
                _isInitFindClients = true;
            }
        }

        private void CollectClientFilter(Update update)
        {
            if (_clientFilters == null || !_clientFilters.Any())
            {
                _myClient = _dataManager.GetData<ICustomerManager>().GetClientByTelegram(Get.GetUserId(update).ToString());
                _clientFilters = _dataManager.GetData<ICustomerManager>().GetFindClientFilters(_myClient);
            }
        }

        private async Task<Message?> SendNextApplication(Update update, ClientViewModel? excludeClientViewModel = null)
        {
            if (_currentClient == null)
            {
                InitFindClients(update, excludeClientViewModel);
                if (_findClients?.Count == 0)
                {
                    long chatId = Get.GetChatId(update);
                    return await _dataManager.GetData<TelegramBotManager>().SendTextMessage(chatId, _messages[MessageKey.NO_ACTUAL_FIND_CLIENTS]);
                }
                _currentClient = _findClients[0];
            }
            Message message = null;
            if (long.TryParse(_currentClient.TelegramId, out long targetUserId))
            {
                if (_dataManager.GetData<ICustomerManager>().AnyTargetClientUncheckedMatch(_myClient.ClientMatchInfo.ClientMatchInfoId, _currentClient.TelegramId))
                {
                    CheckMatchCommand checkMatchCommand = _dataManager.GetData<CommandExecutor>().GetCommandByType(typeof(CheckMatchCommand)) as CheckMatchCommand;
                    if (checkMatchCommand != null)
                    {
                        message = await checkMatchCommand.SendTargetMatchApplication(update, targetUserId, this);
                    }
                }
                if (message == null)
                {
                    message = await _telegramSupport.SendUserApplication(update, targetUserId, nameof(FindApplicationCommand));
                }
            }
            return message;
        }

        /// <summary>
        /// Показать следующую анкеты исключая клиента
        /// </summary>
        /// <param name="update"></param>
        /// <param name="excludeClientViewModel"></param>
        /// <returns></returns>
        public async Task<Message?> SendNextApplicationExcludeClient(Update update, ClientViewModel? excludeClientViewModel = null)
        {
            _dataManager.GetData<CommandExecutor>().StartListen(this);
            await SendStartMessage(update);
            _currentClient = null;
            return await SendNextApplication(update, excludeClientViewModel);
        }

        /// <summary>
        /// Показывает анкету конкретного клиента
        /// </summary>
        /// <param name="update"></param>
        /// <param name="excludeClientViewModel"></param>
        /// <returns></returns>
        public async Task<Message?> SendApplicationTargetClient(Update update, ClientViewModel? targetClientViewModel)
        {
            _dataManager.GetData<CommandExecutor>().StartListen(this);
            await SendStartMessage(update);
            _currentClient = targetClientViewModel;
            return await SendNextApplication(update);
        }

        private async Task<Message?> SendUserMedia(long chatId, long userId) => await _telegramSupport.SendUserMedia(chatId, userId);
        private async Task<Message?> EditMoreInfoApplication(Update update, long userId) => await _telegramSupport.EditMoreInfoApplication(update, userId, nameof(FindApplicationCommand));
        private async Task<Message?> EditLessInfoApplication(Update update, long userId) => await _telegramSupport.EditLessInfoApplication(update, userId, nameof(FindApplicationCommand));

        private IReplyMarkup GetReplyMarkup()
        {
            if (_replyKeyboard == null)
            {
                List<KeyboardButton> inlineKeyboardButtons = new List<KeyboardButton>();
                _replyKeyboard = new ReplyKeyboardMarkup(new[]
                {
                    inlineKeyboardButtons
                });

                foreach (var command in _supportMiniComands)
                {
                    inlineKeyboardButtons.Add(new KeyboardButton(command));
                }
                _replyKeyboard.ResizeKeyboard = true;
                _replyKeyboard.IsPersistent = true;
            }
            return _replyKeyboard;
        }

        public async Task GetUpdate(Update update)
        {
            if (CurrentUpdater == null)
            {
                long chatId = Get.GetChatId(update);
                string messageText = Get.GetText(update);
                var callbackQuery = update.CallbackQuery;
                if (_supportMiniComands.Contains(messageText))
                {
                    if (messageText == _messages[ReplyButton.Like])
                    {
                        await SendLike(update);
                        await SendNextApplication(update);
                    }
                    else if (messageText == _messages[ReplyButton.LoveLetter])
                    {
                        _conditions = new List<ICondition>
                        {
                            new LoveLetterCondition(_dataManager,this,async ()=> await SendLoveLetter(update))
                        };
                        foreach (var condition in _conditions)
                        {
                            await condition.Execute(update);
                            break;
                        }
                    }
                    else if (messageText == _messages[ReplyButton.Dislike])
                    {
                        await SendDislike(update);
                        await SendNextApplication(update);
                    }
                    else
                    {
                        _dataManager.GetData<CommandExecutor>().StopListen(this);
                        await _dataManager.GetData<CommandExecutor>().MenuCommand.Execute(update);
                    }
                }
                else
                {
                    await _dataManager.GetData<TelegramBotManager>().SendTextMessage(chatId, _messages[MessageKey.NO_VARITANT_CHOICE], GetReplyMarkup());
                }
            }
            else
            {
                foreach (ICondition condition in _conditions)
                {
                    if (!condition.IsDone)
                    {
                        if (condition.IsStarted)
                        {
                            if (await condition.CheckCondition(update))
                            {
                                if (condition.IsIgnoredNextCondition)
                                {
                                    break;
                                }
                                continue;
                            }
                            else
                            {
                                return;
                            }
                        }
                        else
                        {
                            await condition.Execute(update);
                            return;
                        }
                    }
                }
                _updater = null;
                if (_conditions.Any(x => x.IsCancel))
                {
                    await SendApplicationTargetClient(update, _currentClient);
                }
                else
                {
                    await SendNextApplication(update);
                }
            }
        }

        public void StartListen(IUpdater updater) => _updater = updater;

        public void StopListen(IUpdater updater)
        {
            if (CurrentUpdater == updater)
            {
                _updater = null;
            }
        }


        /// <summary>
        /// Отправить Лайк
        /// </summary>
        public async Task SendLike(Update update) => await SendMatсh(MatchType.Like, update);

        /// <summary>
        /// Отправить любовное письмо
        /// </summary>
        public async Task SendLoveLetter(Update update) => await SendMatсh(MatchType.LoveLetter, update);

        /// <summary>
        /// Отправить Дизлайк
        /// </summary>
        public async Task SendDislike(Update update) => await SendMatсh(MatchType.Dislike, update);

        private async Task SendMatсh(MatchType matchType, Update update)
        {
            if (_currentClient != null)
            {
                long chatId = Get.GetChatId(update);

                ClientMatchUncheckedViewModel matchViewModel = new ClientMatchUncheckedViewModel();
                matchViewModel.DateMatch = DateTime.Now;
                matchViewModel.ClientMatchInfoId = _currentClient.ClientMatchInfo.ClientMatchInfoId;
                matchViewModel.MatchTelegramId = _myClient.TelegramId;
                matchViewModel.MatchType = matchType.ToString();
                if (matchType == MatchType.Dislike)
                {
                    matchViewModel.AnswearMatchType = matchType.ToString();
                    matchViewModel.AnswearDateMatch = DateTime.Now;
                }
                else
                {
                    await _dataManager.GetData<TelegramBotManager>().SendTextMessage(chatId, _messages[MessageKey.SEND_LOVE_LETTER], GetReplyMarkup());
                }
                BuildUserData(matchViewModel);
                _dataManager.GetData<ICustomerManager>().CreateClientMatch(matchViewModel);

                if (matchViewModel.MatchType != MatchType.Dislike.ToString() && _dataManager.GetData<ICustomerManager>().HasClientNewLikes(_currentClient.ClientMatchInfo))
                {
                    await _newLikesNotify?.Send(_currentClient);
                }
                _findClients.Remove(_currentClient);
                _currentClient = null;
            }
        }

        private void BuildUserData(ClientMatchUncheckedViewModel matchViewModel)
        {
            var userData = new Dictionary<int, ICondition>();
            foreach (var condition in _conditions)
            {
                BotExtensions.ProcessCondition(userData, condition);
            }

            foreach (var condition in userData)
            {
                if (condition.Value is LoveLetterCondition)
                {

                    if (condition.Value.Info is string)
                    {
                        matchViewModel.LoveLetterText = (string)condition.Value.Info;
                    }
                    else if (condition.Value.Info is ClientMatchUncheckedVideoInfoViewModel)
                    {
                        matchViewModel.ClientMatchUncheckedVideoInfo = (ClientMatchUncheckedVideoInfoViewModel)condition.Value.Info;
                    }
                    else if (condition.Value.Info is ClientMatchUncheckedVideoNoteInfoViewModel)
                    {
                        matchViewModel.ClientMatchUncheckedVideoNoteInfo = (ClientMatchUncheckedVideoNoteInfoViewModel)condition.Value.Info;
                    }
                }
            }
        }

        public async Task SupportExecute(Update update) => _isInitFindClients = false;
    }
}
