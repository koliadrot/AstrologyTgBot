using NLog;
using Telegram.Bot.Types;

namespace Service.Core.TelegramBot.RegisterCondition
{
    using Service.Abstract.TelegramBot;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Поиск возраста
    /// </summary>
    public class SearchAgeCondition : BaseRegisterCondition, ICondition
    {
        private bool isDone;
        public bool IsDone => isDone;

        private bool isStarted = false;

        public bool IsStarted => isStarted;

        public object Info => _firstName;

        public bool IsIgnoredNextCondition => false;

        public bool IsCanPass { get; set; } = true;

        public int Order { get; set; } = -1;

        private List<ICondition> _conditions = new List<ICondition>();
        public List<ICondition> Conditions => _conditions;

        private DataManager _dataManager;
        private Dictionary<string, string> _messages = new Dictionary<string, string>();
        private bool _isOptional;
        private string _firstName = string.Empty;



        public SearchAgeCondition(DataManager dataManager, bool isOptional = true)
        {
            _dataManager = dataManager;
            _messages = _dataManager.GetData<CommandExecutor>().Messages;
            _isOptional = isOptional;
        }

        public async Task Execute(Update update)
        {
            if (!isStarted)
            {
                long chatId = Get.GetChatId(update);
                long userId = Get.GetUserId(update);
                await _dataManager.GetData<TelegramBotManager>().SendTextMessage(chatId, _messages[MessageKey.ENTER_SEARCH_AGE], replyMarkup: _isOptional ? GetSkipReplyButtons(IsCanPass, _messages[ReplyButton.SKIP]) : null);
                isStarted = true;
                _dataManager.GetData<ILogger>().Debug($"User:{userId}; Condition:{nameof(SearchAgeCondition)} -  Start condition");
            }
        }

        public async Task<bool> CheckCondition(Update update)
        {
            long chatId = Get.GetChatId(update);
            long userId = Get.GetUserId(update);
            string messageText = Get.GetText(update);
            if (messageText == _messages[ReplyButton.SKIP] && IsCanPass)
            {
                isDone = true;
                _dataManager.GetData<ILogger>().Debug($"User:{userId}; Condition:{nameof(SearchAgeCondition)} -  Skip");
            }
            else if (IsValidAge(messageText))
            {
                _firstName = messageText;
                isDone = true;
            }
            else
            {
                await _dataManager.GetData<TelegramBotManager>().SendTextMessage(chatId, _messages[MessageKey.WRONG_FORMAT_SEARCH_AGE]);
                _dataManager.GetData<ILogger>().Debug($"User:{userId}; Condition:{nameof(SearchAgeCondition)} -  Wrong format");
            }
            return isDone;
        }

        private bool IsValidAge(string ageRange) => Regex.IsMatch(ageRange, @"^\d+-\d+$");
    }
}

