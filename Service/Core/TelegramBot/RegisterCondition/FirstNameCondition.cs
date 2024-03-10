using NLog;
using Service.Abstract.TelegramBot;
using Service.Extensions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Service.Core.TelegramBot.RegisterCondition
{
    /// <summary>
    /// Имя
    /// </summary>
    public class FirstNameCondition : BaseRegisterCondition, ICondition
    {
        private bool isDone;
        public bool IsDone => isDone;

        private bool isStarted = false;

        public bool IsStarted => isStarted;

        public bool IsCancel { get; private set; } = false;

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



        public FirstNameCondition(DataManager dataManager, bool isOptional = true)
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
                List<KeyboardButton> inlineKeyboardButtons = new List<KeyboardButton>();
                ReplyKeyboardMarkup replyKeyboard = new ReplyKeyboardMarkup(new[]
                {
                    inlineKeyboardButtons
                });
                replyKeyboard.ResizeKeyboard = true;
                replyKeyboard.OneTimeKeyboard = true;
                inlineKeyboardButtons.Add(new KeyboardButton(update.Message.From.FirstName));
                if (_isOptional && IsCanPass)
                {
                    inlineKeyboardButtons.Add(new KeyboardButton(_messages[ReplyButton.SKIP]));
                }
                await _dataManager.GetData<TelegramBotManager>().SendTextMessage(chatId, _messages[MessageKey.ENTER_FIRST_NAME], replyKeyboard);
                isStarted = true;
                _dataManager.GetData<ILogger>().Debug($"User:{userId}; Condition:{nameof(FirstNameCondition)} -  Start condition");
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
                _dataManager.GetData<ILogger>().Debug($"User:{userId}; Condition:{nameof(FirstNameCondition)} -  Skip");
            }
            else if (!messageText.IsNull())
            {
                _firstName = messageText;
                isDone = true;
            }
            else
            {
                await _dataManager.GetData<TelegramBotManager>().SendTextMessage(chatId, _messages[MessageKey.WRONG_FIRST_NAME]);
                _dataManager.GetData<ILogger>().Debug($"User:{userId}; Condition:{nameof(FirstNameCondition)} -  Wrong format");
            }
            return isDone;
        }
    }
}
