using NLog;
using Service.Abstract.TelegramBot;
using Service.Enums;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Service.Core.TelegramBot.RegisterCondition
{
    /// <summary>
    /// Пол при рождении
    /// </summary>
    public class BirthGenderCondition : BaseRegisterCondition, ICondition
    {
        private bool isDone = false;

        public bool IsDone => isDone;

        private bool isStarted = false;

        public bool IsStarted => isStarted;

        public object Info => _gender;

        public bool IsIgnoredNextCondition => false;

        public bool IsCanPass { get; set; } = true;

        public int Order { get; set; } = -1;

        private string _gender;

        private List<ICondition> _conditions = new List<ICondition>();
        public List<ICondition> Conditions => _conditions;

        private DataManager _dataManager;
        private Dictionary<string, string> _message;
        private List<string> _supportMiniComands = new List<string>();
        private bool _isOptional;

        public BirthGenderCondition(DataManager dataManager, bool isOptional = true)
        {
            _dataManager = dataManager;
            _message = _dataManager.GetData<CommandExecutor>().Messages;
            _supportMiniComands.Add(_message[ReplyButton.MAN]);
            _supportMiniComands.Add(_message[ReplyButton.WOMAN]);
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
                await _dataManager.GetData<TelegramBotManager>().SendTextMessage(chatId, _message[MessageKey.ENTER_BIRTH_GENDER], replyKeyboard);
                isStarted = true;
                _dataManager.GetData<ILogger>().Debug($"User:{userId}; Condition:{nameof(BirthGenderCondition)} -  Start condition");
            }
        }

        public async Task<bool> CheckCondition(Update update)
        {
            long userId = Get.GetUserId(update);
            string messageText = Get.GetText(update);
            if (messageText == _message[ReplyButton.SKIP] && IsCanPass)
            {
                isDone = true;
                _dataManager.GetData<ILogger>().Debug($"User:{userId}; Condition:{nameof(BirthGenderCondition)} -  Skip");
            }
            else if (_supportMiniComands.Contains(messageText))
            {
                _gender = messageText == _message[ReplyButton.MAN] ? GenderType.Man.ToString() : GenderType.Woman.ToString();
                isDone = true;
            }
            else
            {
                isStarted = false;
                await Execute(update);
                _dataManager.GetData<ILogger>().Debug($"User:{userId}; Condition:{nameof(BirthGenderCondition)} -  Wrong format");
            }
            return IsDone;
        }
    }
}

