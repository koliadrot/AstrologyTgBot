namespace Service.Core.TelegramBot.Commands
{
    using Service.Abstract;
    using Service.Abstract.TelegramBot;
    using Service.Core.TelegramBot.Notifies;
    using Service.Enums;
    using Service.Support;
    using Service.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Telegram.Bot.Types;
    using Telegram.Bot.Types.ReplyMarkups;

    /// <summary>
    /// Проверка совпадений
    /// </summary>
    public class CheckMatchCommand : ICommand, IUpdater
    {
        public string Name { get; set; } = "/checkmatch";

        public string Description { get; set; } = "Проверка совпадений";

        public string ShortDescription { get; set; } = "Мои лайки";

        public bool IsAuth { get; set; } = true;
        public bool IsDefault { get; set; } = false;

        public TelegramBotCommandType CommandType => TelegramBotCommandType.Custom;
        public bool IsStartMenu => false;

        private bool _isOneMoreTimeMatch = false;
        private ReplyKeyboardMarkup _replyKeyboard;
        private List<string> _supportMiniComands = new List<string>();

        private ClientViewModel? _currentClient;
        private ClientViewModel? _myClient;
        private ICommand? _additionalTargetCommand = null;

        private MatchNotify? _matchNotify;
        private readonly DataManager _dataManager;
        private readonly Dictionary<string, string> _messages;
        private readonly TelegramSupport _telegramSupport;

        public CheckMatchCommand(DataManager dataManager)
        {
            _dataManager = dataManager;
            _messages = _dataManager.GetData<CommandExecutor>().Messages;
            _telegramSupport = new TelegramSupport(dataManager);
            _matchNotify = _dataManager.GetData<CommandExecutor>()?.GetNotifyByType(typeof(MatchNotify)) as MatchNotify;

            _supportMiniComands.Add(_messages[ReplyButton.Like]);
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
                await _dataManager.GetData<TelegramBotManager>().SendTextMessage(chatId, _messages[MessageKey.DEFAULT_CHECK_MATCH], replyMarkup: GetReplyMarkup());
            }
        }

        public async Task Execute(Update update, string[] arg = null)
        {
            long userId = Get.GetUserId(update);
            if (_dataManager.GetData<ICustomerManager>().ExistTelegram(userId))
            {
                if (HasNewMatch(update))
                {
                    await SendStartMessage(update);
                }
                _currentClient = null;
                await SendNextMatchApplication(update);
                _dataManager.GetData<CommandExecutor>().StartListen(this);
            }
        }


        private async Task<Message?> SendNextMatchApplication(Update update)
        {
            _isOneMoreTimeMatch = false;
            _additionalTargetCommand = null;
            if (_currentClient == null)
            {
                long userId = Get.GetUserId(update);
                _myClient = _dataManager.GetData<ICustomerManager>().GetClientByTelegram(userId.ToString());

                _currentClient = _myClient.ClientMatchInfo != null && _myClient.ClientMatchInfo.UncheckedClientMatchs.Any() ? _dataManager.GetData<ICustomerManager>().GetClientByTelegram(_myClient.ClientMatchInfo.UncheckedClientMatchs.FirstOrDefault().MatchTelegramId) : null;
            }
            if (_currentClient != null)
            {
                Message message = null;
                if (long.TryParse(_currentClient.TelegramId, out long targetUserId))
                {
                    message = await _telegramSupport.SendUserApplication(update, targetUserId, nameof(CheckMatchCommand));
                    string additionalText = string.Empty;
                    int newLikes = _dataManager.GetData<ICustomerManager>().NewLikesCountByClientMatchInfo(_myClient.ClientMatchInfo, false);
                    int leftLikes = newLikes - 1;
                    if (leftLikes > 0)
                    {
                        additionalText = Get.ReplaceKeysInText(_messages[MessageKey.COUNT_LEFT_LIKES], new Dictionary<string, string>() { { Promt.LIKES, leftLikes.ToString() } });
                    }
                    await _telegramSupport.SendMatchApplication(update, targetUserId, additionalTexts: additionalText);
                }
                return message;
            }
            else
            {
                long chatId = Get.GetChatId(update);
                ReplyKeyboardMarkup replyKeyboard = null;

                var findApplicationCommand = _dataManager.GetData<CommandExecutor>().Menu.SelectMany(x => x.Value).SelectMany(x => x.Value).FirstOrDefault(x => x is FindApplicationCommand) as FindApplicationCommand;
                if (findApplicationCommand != null)
                {
                    List<KeyboardButton> inlineKeyboardButtons = new List<KeyboardButton>();
                    replyKeyboard = new ReplyKeyboardMarkup(new[]
                    {
                        inlineKeyboardButtons
                    });
                    replyKeyboard.ResizeKeyboard = true;
                    replyKeyboard.OneTimeKeyboard = true;
                    inlineKeyboardButtons.Add(new KeyboardButton(_messages[ReplyButton.FIND_APPLICATIONS]));
                }
                return await _dataManager.GetData<TelegramBotManager>().SendTextMessage(chatId, _messages[MessageKey.NO_CLIENT_MATCHS], replyMarkup: replyKeyboard);
            }
        }

        /// <summary>
        /// Отправить конкретную анкету с совпадением
        /// </summary>
        /// <param name="update"></param>
        /// <param name="targetUserId"></param>
        /// <returns></returns>
        public async Task<Message?> SendTargetMatchApplication(Update update, long targetUserId, ICommand? command = null)
        {
            long userId = Get.GetUserId(update);
            _myClient = _dataManager.GetData<ICustomerManager>().GetClientByTelegram(userId.ToString());

            _currentClient = _dataManager.GetData<ICustomerManager>().GetClientByTelegram(targetUserId.ToString());
            Message message = null;
            if (_currentClient != null)
            {
                _isOneMoreTimeMatch = true;
                _additionalTargetCommand = command;
                _dataManager.GetData<CommandExecutor>().StartListen(this);
                message = await _telegramSupport.SendUserApplication(update, targetUserId, nameof(CheckMatchCommand));
                await _telegramSupport.SendMatchApplication(update, targetUserId, GetReplyMarkup());
            }
            return message;
        }

        private async Task<Message?> SendUserMedia(long chatId, long userId) => await _telegramSupport.SendUserMedia(chatId, userId);
        private async Task<Message?> EditMoreInfoApplication(Update update, long userId) => await _telegramSupport.EditMoreInfoApplication(update, userId, nameof(CheckMatchCommand));
        private async Task<Message?> EditLessInfoApplication(Update update, long userId) => await _telegramSupport.EditLessInfoApplication(update, userId, nameof(CheckMatchCommand));

        private bool HasNewMatch(Update update)
        {
            long userId = Get.GetUserId(update);
            _myClient = _dataManager.GetData<ICustomerManager>().GetClientByTelegram(userId.ToString());

            return _myClient.ClientMatchInfo != null && _myClient.ClientMatchInfo.UncheckedClientMatchs.Any();
        }

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
            long chatId = Get.GetChatId(update);
            string messageText = Get.GetText(update);
            var callbackQuery = update.CallbackQuery;
            if (_supportMiniComands.Contains(messageText))
            {
                if (messageText == _messages[ReplyButton.Like])
                {
                    if (_isOneMoreTimeMatch)
                    {
                        await SendLike(update);
                    }
                    else
                    {
                        await SendLike(update);
                        await SendNextMatchApplication(update);
                    }
                }
                else if (messageText == _messages[ReplyButton.Dislike])
                {
                    if (_isOneMoreTimeMatch)
                    {
                        await SendDislike(update);
                    }
                    else
                    {
                        await SendDislike(update);
                        await SendNextMatchApplication(update);
                    }
                }
                else
                {
                    _dataManager.GetData<CommandExecutor>().StopListen(this);
                    await _dataManager.GetData<CommandExecutor>().MenuCommand.Execute(update);
                }
            }
            else if (messageText == _messages[ReplyButton.FIND_APPLICATIONS])
            {
                var findApplicationCommand = _dataManager.GetData<CommandExecutor>().Menu.SelectMany(x => x.Value).SelectMany(x => x.Value).FirstOrDefault(x => x is FindApplicationCommand) as FindApplicationCommand;
                if (findApplicationCommand != null)
                {
                    await findApplicationCommand.Execute(update);
                }
            }
            else
            {
                await _dataManager.GetData<TelegramBotManager>().SendTextMessage(chatId, _messages[MessageKey.NO_VARITANT_CHOICE], GetReplyMarkup());
            }
        }


        /// <summary>
        /// Отправить Лайк
        /// </summary>
        public async Task SendLike(Update update) => await SendMatсh(MatchType.Like, update);

        /// <summary>
        /// Отправить Дизлайк
        /// </summary>
        public async Task SendDislike(Update update) => await SendMatсh(MatchType.Dislike, update);

        private async Task SendMatсh(MatchType matchType, Update update)
        {
            if (_currentClient != null)
            {
                ClientMatchUncheckedViewModel matchViewModel = _dataManager.GetData<ICustomerManager>().GetTargetClientUncheckedMatch(_myClient.ClientMatchInfo.ClientMatchInfoId, _currentClient.TelegramId);
                if (matchViewModel != null)
                {
                    matchViewModel.AnswearDateMatch = DateTime.Now;
                    matchViewModel.AnswearMatchType = matchType.ToString();
                    if (matchType == MatchType.Like)
                    {
                        await _matchNotify?.Send(_myClient, _currentClient);
                    }
                }
                _dataManager.GetData<ICustomerManager>().UpdateClientMatch(matchViewModel);
            }
            if (_isOneMoreTimeMatch)
            {
                _isOneMoreTimeMatch = false;
                if (_additionalTargetCommand != null)
                {
                    var findApplicationCommand = _additionalTargetCommand as FindApplicationCommand;
                    if (findApplicationCommand != null)
                    {
                        await findApplicationCommand.SendNextApplicationExcludeClient(update, _currentClient);
                    }
                    else
                    {
                        await _additionalTargetCommand.Execute(update);
                    }
                    _additionalTargetCommand = null;
                }
                else
                {
                    _dataManager.GetData<CommandExecutor>().StopListen(this);
                }
            }
            _currentClient = null;
        }
    }
}
