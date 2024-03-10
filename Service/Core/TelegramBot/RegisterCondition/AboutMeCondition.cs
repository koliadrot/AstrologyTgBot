namespace Service.Core.TelegramBot.RegisterCondition
{
    using NLog;
    using Service.Abstract.TelegramBot;
    using Service.Extensions;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Telegram.Bot.Types;

    /// <summary>
    /// О себе
    /// </summary>
    public class AboutMeCondition : BaseRegisterCondition, ICondition
    {
        private bool isDone;
        public bool IsDone => isDone;

        private bool isStarted = false;

        public bool IsStarted => isStarted;

        public bool IsCancel { get; private set; } = false;

        public object Info => _description;

        public bool IsIgnoredNextCondition => false;

        public bool IsCanPass { get; set; } = true;

        public int Order { get; set; } = -1;

        private List<ICondition> _conditions = new List<ICondition>();
        public List<ICondition> Conditions => _conditions;

        private string _description = string.Empty;

        private DataManager _dataManager;
        private Dictionary<string, string> _message;
        private bool _isOptional;

        private const int MAX_LIMIT_CHARACTERS = 1024;

        public AboutMeCondition(DataManager dataManager, bool isOptional = true)
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
                await _dataManager.GetData<TelegramBotManager>().SendTextMessage(chatId, _message[MessageKey.ENTER_ABOUT_ME], replyMarkup: _isOptional ? GetSkipReplyButtons(IsCanPass, _message[ReplyButton.SKIP]) : null);
                isStarted = true;
                _dataManager.GetData<ILogger>().Debug($"User:{userId}; Condition:{nameof(AboutMeCondition)} -  Start condition");
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
                _dataManager.GetData<ILogger>().Debug($"User:{userId}; Condition:{nameof(AboutMeCondition)} -  Skip");
            }
            else if (IsValid(messageText))
            {
                _description = messageText;
                isDone = true;
            }
            else
            {
                await _dataManager.GetData<TelegramBotManager>().SendTextMessage(chatId, _message[MessageKey.WHRONG_ABOUT_ME]);
                _dataManager.GetData<ILogger>().Debug($"User:{userId}; Condition:{nameof(AboutMeCondition)} -  Wrong format");
            }
            return isDone;
        }

        private bool IsValid(string messageText) => !messageText.IsNull() && messageText.Length <= MAX_LIMIT_CHARACTERS;
    }
}
