namespace Service.Core.TelegramBot.RegisterCondition
{
    using NLog;
    using Service.Abstract.TelegramBot;
    using Service.Enums;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Telegram.Bot.Types;
    using Telegram.Bot.Types.ReplyMarkups;


    /// <summary>
    /// Цель поиска
    /// </summary>
    public class SearchGoalCondition : BaseRegisterCondition, ICondition
    {
        private bool isDone = false;

        public bool IsDone => isDone;

        private bool isStarted = false;

        public bool IsStarted => isStarted;

        public object Info => _goal;

        public bool IsIgnoredNextCondition => false;

        public bool IsCanPass { get; set; } = true;

        public int Order { get; set; } = -1;

        private string _goal;

        private List<ICondition> _conditions = new List<ICondition>();
        public List<ICondition> Conditions => _conditions;

        private DataManager _dataManager;
        private Dictionary<string, string> _message;
        private List<string> _supportMiniComands = new List<string>();
        private bool _isOptional;

        public SearchGoalCondition(DataManager dataManager, bool isOptional = true)
        {
            _dataManager = dataManager;
            _message = _dataManager.GetData<CommandExecutor>().Messages;
            _supportMiniComands.Add(_message[ReplyButton.RELATIONSHIP]);
            _supportMiniComands.Add(_message[ReplyButton.COMMUNICATION]);
            _supportMiniComands.Add(_message[ReplyButton.ANYWAY]);
            _isOptional = isOptional;
        }

        public async Task Execute(Update update)
        {
            if (!isStarted)
            {
                long chatId = Get.GetChatId(update);
                long userId = Get.GetUserId(update);
                List<KeyboardButton> inlineKeyboardButtons = new List<KeyboardButton>();
                ReplyKeyboardMarkup replyKeyboard = new ReplyKeyboardMarkup(new[]
                {
                    inlineKeyboardButtons
                });
                replyKeyboard.ResizeKeyboard = true;
                replyKeyboard.OneTimeKeyboard = true;
                foreach (var miniCommand in _supportMiniComands)
                {
                    inlineKeyboardButtons.Add(new KeyboardButton(miniCommand));
                }
                if (IsCanPass && _isOptional)
                {
                    inlineKeyboardButtons.Add(_message[ReplyButton.SKIP]);
                }
                await _dataManager.GetData<TelegramBotManager>().SendTextMessage(chatId, _message[MessageKey.ENTER_SEARCH_GOAL], replyKeyboard);
                isStarted = true;
                _dataManager.GetData<ILogger>().Debug($"User:{userId}; Condition:{nameof(SearchGoalCondition)} -  Start condition");
            }
        }

        public async Task<bool> CheckCondition(Update update)
        {
            long userId = Get.GetUserId(update);
            string messageText = Get.GetText(update);
            if (messageText == _message[ReplyButton.SKIP] && IsCanPass)
            {
                isDone = true;
                _dataManager.GetData<ILogger>().Debug($"User:{userId}; Condition:{nameof(SearchGoalCondition)} -  Skip");
            }
            else if (_supportMiniComands.Contains(messageText))
            {
                if (messageText == _message[ReplyButton.COMMUNICATION])
                {
                    _goal = GoalType.Communication.ToString();
                }
                else if (messageText == _message[ReplyButton.RELATIONSHIP])
                {
                    _goal = GoalType.Relationship.ToString();
                }
                else
                {
                    _goal = GoalType.AnyWay.ToString();
                }
                isDone = true;
            }
            else
            {
                isStarted = false;
                await Execute(update);
                _dataManager.GetData<ILogger>().Debug($"User:{userId}; Condition:{nameof(SearchGoalCondition)} -  Wrong format");
            }
            return IsDone;
        }
    }
}
