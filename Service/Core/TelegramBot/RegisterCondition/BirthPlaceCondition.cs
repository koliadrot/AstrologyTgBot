namespace Service.Core.TelegramBot.RegisterCondition
{
    using NLog;
    using Service.Abstract.TelegramBot;
    using Service.Support;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Telegram.Bot.Types;
    using Telegram.Bot.Types.Enums;
    using Telegram.Bot.Types.ReplyMarkups;

    /// <summary>
    /// Место рождения
    /// </summary>
    public class BirthPlaceCondition : BaseRegisterCondition, ICondition, IUpdater
    {
        private bool isDone;
        public bool IsDone => isDone;

        private bool isStarted = false;

        public bool IsStarted => isStarted;

        public object Info => _place;

        public bool IsIgnoredNextCondition => false;

        public bool IsCanPass { get; set; } = true;

        public int Order { get; set; } = -1;

        private List<ICondition> _conditions = new List<ICondition>();
        public List<ICondition> Conditions => _conditions;

        private List<string> _supportMiniComands = new List<string>();

        private string _place = string.Empty;
        private string _cityName = string.Empty;
        private string _coord = string.Empty;

        private IListener _listener;
        private DataManager _dataManager;
        private Dictionary<string, string> _message;
        private bool _isOptional;

        public BirthPlaceCondition(DataManager dataManager, IListener listener, bool isOptional = true)
        {
            _dataManager = dataManager;
            _listener = listener;
            _message = _dataManager.GetData<CommandExecutor>().Messages;
            _isOptional = isOptional;
            _supportMiniComands.Add(_message[ReplyButton.YES]);
            _supportMiniComands.Add(_message[ReplyButton.NO]);
        }

        public async Task Execute(Update update)
        {
            if (!isStarted)
            {
                _place = string.Empty;
                _cityName = string.Empty;
                _coord = string.Empty;
                long chatId = Get.GetChatId(update);
                long userId = Get.GetUserId(update);
                List<KeyboardButton> inlineKeyboardButtons = new List<KeyboardButton>();
                ReplyKeyboardMarkup replyKeyboard = new ReplyKeyboardMarkup(new[]
                {
                    inlineKeyboardButtons
                });
                replyKeyboard.ResizeKeyboard = true;
                replyKeyboard.OneTimeKeyboard = true;
                inlineKeyboardButtons.Add(KeyboardButton.WithRequestLocation(_message[ReplyButton.SHARE_GEO]));
                if (_isOptional && IsCanPass)
                {
                    inlineKeyboardButtons.Add(new KeyboardButton(_message[ReplyButton.SKIP]));
                }
                await _dataManager.GetData<TelegramBotManager>().SendTextMessage(chatId, _message[MessageKey.ENTER_BIRTH_PLACE], replyKeyboard, parseMode: ParseMode.MarkdownV2);
                isStarted = true;
                _dataManager.GetData<ILogger>().Debug($"User:{userId}; Condition:{nameof(BirthPlaceCondition)} -  Start condition");
            }
        }

        public async Task<bool> CheckCondition(Update update)
        {
            long chatId = Get.GetChatId(update);
            long userId = Get.GetUserId(update);
            if (Get.GetText(update) == _message[ReplyButton.SKIP] && IsCanPass)
            {
                isDone = true;
                _dataManager.GetData<ILogger>().Debug($"User:{userId}; Condition:{nameof(BirthPlaceCondition)} -  Skip");
            }
            else if (await IsValidCity(update))
            {
                _listener.StartListen(this);
                await AcceptCity(update);
            }
            else
            {
                await _dataManager.GetData<TelegramBotManager>().SendTextMessage(chatId, _message[MessageKey.WRONG_FORMAT_BIRTH_PLACE]);
                _dataManager.GetData<ILogger>().Debug($"User:{userId}; Condition:{nameof(BirthPlaceCondition)} -  Wrong format");
            }
            return isDone;
        }

        public async Task GetUpdate(Update update)
        {
            string messageText = Get.GetText(update);
            if (_supportMiniComands.Contains(messageText))
            {
                if (messageText == _message[ReplyButton.YES])
                {
                    _listener.StopListen(this);
                    isDone = true;
                    if ((IUpdater)_listener != null)
                    {
                        await ((IUpdater)_listener).GetUpdate(update);
                    }
                }
                else
                {
                    _listener.StopListen(this);
                    isStarted = false;
                    await Execute(update);
                    return;
                }
            }
            else
            {
                await AcceptCity(update);
                return;
            }
        }

        private async Task<bool> IsValidCity(Update update)
        {
            bool result = update.Message.Location != null;
            OpenMap openMap = new OpenMap();
            _cityName = string.Empty;
            string lat = string.Empty;
            string lon = string.Empty;
            _coord = string.Empty;
            try
            {
                if (!result)
                {
                    _cityName = Get.GetText(update);
                    var results = await openMap.GetDetailsByCityName(_cityName);

                    if (results.HasValues && results.Count > 0)
                    {
                        _cityName = results[0]["addresstype"].ToString() == "city" ? results[0]["address"]["city"].ToString() : results[0]["address"]["town"].ToString();
                        lat = results[0]["lat"].ToString();
                        lon = results[0]["lon"].ToString();
                        _coord = $"{lat}:{lon}";
                        _place = $"{_cityName}-{_coord}";
                        result = true;
                    }
                }
                else
                {
                    lat = update.Message.Location.Latitude.ToString().Replace(",", ".");
                    lon = update.Message.Location.Longitude.ToString().Replace(",", ".");
                    var results = await openMap.GetDetailsByGeo(lat, lon);

                    if (results.HasValues && results.Count > 0)
                    {
                        _cityName = results["address"]["city"].ToString();
                        _coord = $"{lat}:{lon}";
                        _place = $"{_cityName}-{_coord}";
                        result = true;
                    }
                }
            }
            catch (Exception) { }

            return result;
        }

        private async Task AcceptCity(Update update)
        {
            long chatId = Get.GetChatId(update);
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
            await _dataManager.GetData<TelegramBotManager>().SendTextMessage(chatId, Get.ReplaceKeysInText(_message[MessageKey.ACCEPT_CITY], new Dictionary<string, string>() { { Promt.CITY, _cityName } }), replyMarkup: replyKeyboard);
        }
    }
}
