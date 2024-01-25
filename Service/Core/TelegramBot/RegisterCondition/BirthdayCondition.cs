using NLog;
using Service.Abstract.TelegramBot;
using Service.Extensions;
using System.Globalization;
using Telegram.Bot.Types;

namespace Service.Core.TelegramBot.RegisterCondition
{
    /// <summary>
    /// Условие регистрации даты рождения
    /// </summary>
    public class BirthdayCondition : BaseRegisterCondition, ICondition
    {
        private bool isDone;
        public bool IsDone => isDone;

        private bool isStarted = false;

        public bool IsStarted => isStarted;

        public object Info => birthday;

        public bool IsIgnoredNextCondition => false;

        public bool IsCanPass { get; set; } = true;

        public int Order { get; set; } = -1;

        private string birthday = string.Empty;

        private List<ICondition> _conditions = new List<ICondition>();
        public List<ICondition> Conditions => _conditions;

        private DataManager _dataManager;
        private Dictionary<string, string> _message;
        private bool _isOptional;
        private const string DATE_PATTERN = "dd.MM.yyyy HH:mm";

        public BirthdayCondition(DataManager dataManager, bool isOptional = true)
        {
            _dataManager = dataManager;
            _message = _dataManager.GetData<CommandExecutor>().Messages;
            _isOptional = isOptional;
        }

        public async Task Execute(Update update)
        {
            if (!isStarted)
            {
                long chatId = Get.GetChatId(update);
                long userId = Get.GetUserId(update);
                await _dataManager.GetData<TelegramBotManager>().SendTextMessage(chatId, _message[MessageKey.ENTER_BIRTHDAY], replyMarkup: _isOptional ? GetSkipReplyButtons(IsCanPass, _message[ReplyButton.SKIP]) : null);
                isStarted = true;
                _dataManager.GetData<ILogger>().Debug($"User:{userId}; Condition:{nameof(BirthdayCondition)} -  Start condition");
            }
        }

        public async Task<bool> CheckCondition(Update update)
        {
            long chatId = Get.GetChatId(update);
            long userId = Get.GetUserId(update);
            string messageText = Get.GetText(update);
            if (messageText == _message[ReplyButton.SKIP] && IsCanPass)
            {
                isDone = true;
                _dataManager.GetData<ILogger>().Debug($"User:{userId}; Condition:{nameof(BirthdayCondition)} -  Skip");
            }
            else if (IsValidDateOfBirth(messageText))
            {
                DateTime parsedDate = DateTime.ParseExact(messageText, DATE_PATTERN, CultureInfo.InvariantCulture);
                birthday = parsedDate.ToString("yyyy-MM-dd HH-mm");
                isDone = true;
            }
            else
            {
                await _dataManager.GetData<TelegramBotManager>().SendTextMessage(chatId, _message[MessageKey.WRONG_FORMAT_BIRTHDAY]);
                _dataManager.GetData<ILogger>().Debug($"User:{userId}; Condition:{nameof(BirthdayCondition)} -  Wrong format {messageText}");
            }
            return isDone;
        }

        private bool IsValidDateOfBirth(string dateOfBirth) => !dateOfBirth.IsNull() && DateTime.TryParseExact(dateOfBirth, DATE_PATTERN, null, DateTimeStyles.None, out _);
    }
}


