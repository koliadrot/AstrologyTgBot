using NLog;
using Service.Abstract;
using Service.Abstract.TelegramBot;
using Service.Extensions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Service.Core.TelegramBot.RegisterCondition
{
    /// <summary>
    /// Услвоие ознакомления с правилами программы и обработкой персональных данных
    /// </summary>
    public class TermOfUseCondition : ICondition
    {
        private bool isDone = false;
        public bool IsDone => isDone;

        private bool isStarted = false;

        public bool IsStarted => isStarted;

        public object Info => null;

        public bool IsIgnoredNextCondition => false;

        public bool IsCanPass { get; set; } = false;

        public int Order { get; set; } = -1;

        private List<ICondition> _conditions = new List<ICondition>();
        public List<ICondition> Conditions => _conditions;

        private string _conditionsLink = string.Empty;

        private DataManager _dataManager;
        private Dictionary<string, string> _message;
        private List<string> _supportMiniComands = new List<string>();
        private const string DEFAULT_TERMS_UF_USE = "Прочитав указанный текст, я соглашаюсь, что условия политики бота.";


        public TermOfUseCondition(DataManager dataManager)
        {
            _dataManager = dataManager;
            _message = _dataManager.GetData<CommandExecutor>().Messages;
            _supportMiniComands.Add(_message[ReplyButton.I_AGREE]);
            _supportMiniComands.Add(_message[ReplyButton.READ]);
        }

        public async Task Execute(Update update)
        {
            if (!isStarted)
            {
                long chatId = Get.GetChatId(update);
                long userId = Get.GetUserId(update);
                string termOfUseDatabase = _dataManager.GetData<ISettingsManager>().GetTelegramBot().TosUrl;
                _conditionsLink = termOfUseDatabase.IsNull() ? DEFAULT_TERMS_UF_USE : termOfUseDatabase;

                List<KeyboardButton> inlineKeyboardButtons = new List<KeyboardButton>();
                ReplyKeyboardMarkup replyKeyboard = new ReplyKeyboardMarkup(new[]
                {
                    inlineKeyboardButtons
                 });
                replyKeyboard.ResizeKeyboard = true;
                replyKeyboard.OneTimeKeyboard = true;
                foreach (var miniCommand in _supportMiniComands)
                {
                    if (miniCommand == _message[ReplyButton.READ] && _conditionsLink.IsValidLink())
                    {
                        var url = new WebAppInfo() { Url = _conditionsLink };
                        inlineKeyboardButtons.Add(KeyboardButton.WithWebApp(_message[ReplyButton.READ], url));
                    }
                    else
                    {
                        inlineKeyboardButtons.Add(new KeyboardButton(miniCommand));
                    }

                }
                await _dataManager.GetData<TelegramBotManager>().SendTextMessage(chatId, _message[MessageKey.READ_TERMS_OF_USE], replyKeyboard);
                isStarted = true;
                _dataManager.GetData<ILogger>().Debug($"User:{userId}; Condition:{nameof(TermOfUseCondition)} -  Start condition");
            }
        }

        public async Task<bool> CheckCondition(Update update)
        {
            long chatId = Get.GetChatId(update);
            long userId = Get.GetUserId(update);
            string messageText = Get.GetText(update);
            if (_supportMiniComands.Contains(messageText))
            {
                if (messageText == _message[ReplyButton.I_AGREE])
                {
                    isDone = true;
                    _dataManager.GetData<ILogger>().Debug($"User:{userId}; Condition:{nameof(TermOfUseCondition)} -  Skip");
                }
                else
                {
                    List<KeyboardButton> inlineKeyboardButtons = new List<KeyboardButton>();
                    ReplyKeyboardMarkup replyKeyboard = new ReplyKeyboardMarkup(new[]
                    {
                        inlineKeyboardButtons
                    });
                    replyKeyboard.ResizeKeyboard = true;
                    replyKeyboard.OneTimeKeyboard = true;
                    inlineKeyboardButtons.Add(new KeyboardButton(_message[ReplyButton.I_AGREE]));

                    //NOTE:Максимальное число символов, которое может отправить телеграм бот
                    int maxMessageLength = 4096;

                    List<string> messages = new List<string>();
                    for (int i = 0; i < _conditionsLink.Length; i += maxMessageLength)
                    {
                        int length = Math.Min(maxMessageLength, _conditionsLink.Length - i);
                        string messagePart = _conditionsLink.Substring(i, length);
                        messages.Add(messagePart);
                    }


                    string lastMessage = messages[messages.Count - 1];
                    messages.RemoveAt(messages.Count - 1);

                    foreach (string message in messages)
                    {
                        await _dataManager.GetData<TelegramBotManager>().SendTextMessage(chatId, message);
                    }

                    await _dataManager.GetData<TelegramBotManager>().SendTextMessage(chatId, lastMessage, replyKeyboard);
                }
            }
            else
            {
                isStarted = false;
                await Execute(update);
                _dataManager.GetData<ILogger>().Debug($"User:{userId}; Condition:{nameof(TermOfUseCondition)} -  Wrong format");
            }

            return isDone;
        }
    }
}

