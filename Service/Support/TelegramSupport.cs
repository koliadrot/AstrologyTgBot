namespace Service.Support
{
    using Service.Abstract;
    using Service.Abstract.TelegramBot;
    using Service.Core;
    using Service.Core.TelegramBot;
    using Service.Core.TelegramBot.ConditionKeyboard;
    using Service.Core.TelegramBot.Messages;
    using Service.Enums;
    using Service.Extensions;
    using Service.ViewModels;
    using Telegram.Bot.Types;
    using Telegram.Bot.Types.Enums;
    using Telegram.Bot.Types.ReplyMarkups;

    /// <summary>
    /// Телеграм команда расширения
    /// </summary>
    public class TelegramSupport
    {
        private TelegramBotManager _telegramBotManager;
        private DataManager _dataManager;
        private Dictionary<string, string> _messages;
        private List<IConditionKeyboard> _applicationKeyboards = new List<IConditionKeyboard>();

        /// <summary>
        /// Ограничение по длительности видео роликов
        /// </summary>
        public const int LIMIT_VIDEO_DURATION = 16;

        public TelegramSupport(DataManager dataManager)
        {
            _dataManager = dataManager;
            _messages = _dataManager.GetData<CommandExecutor>().Messages;
        }

        public TelegramSupport(TelegramBotManager telegramBotManager, Dictionary<string, string> messegaes)
        {
            _telegramBotManager = telegramBotManager;
            _messages = messegaes;
        }

        private void InitApplicationKeyboards()
        {
            _applicationKeyboards.Clear();
            List<IConditionKeyboard> newKeyboards = new List<IConditionKeyboard>
            {
                new AboutMeKeyboardCondition(_dataManager,_messages),
                new MediaKeyboardCondition(_dataManager,_messages)
            };
            var conditions = _dataManager.GetData<ISettingsManager>().GetTelegramBotRegisterConditions().Where(x => x.IsInfo);
            foreach (var condition in conditions)
            {
                var keyboard = newKeyboards.FirstOrDefault(x => x.ConditionName == condition.ConditionName);
                if (keyboard != null)
                {
                    _applicationKeyboards.Add(keyboard);
                }
            }
        }

        /// <summary>
        /// Отправить анкету клиента
        /// </summary>
        /// <param name="update">Инфо от кого пришел запрос</param>
        /// <param name="userId">Id клиента анкеты, которую надо отправить</param>
        /// <returns></returns>
        public async Task<Message?> SendUserApplication(Update update, long userId, string source)
        {
            long chatId = Get.GetChatId(update);
            return await SendUserApplication(chatId, userId, source);
        }

        /// <summary>
        /// Отправить анкету клиента
        /// </summary>
        /// <param name="chatId">Id в какой чат отправить анкету</param>
        /// <param name="userId">Id клиента анкеты, которую надо отправить</param>
        /// <returns></returns>
        public async Task<Message?> SendUserApplication(long chatId, long userId, string source)
        {
            InitApplicationKeyboards();
            CustomerManager customerManager = _dataManager == null ? new CustomerManager() : _dataManager.GetData<ICustomerManager>() as CustomerManager;
            _telegramBotManager = _dataManager != null ? _dataManager.GetData<TelegramBotManager>() : _telegramBotManager;

            var user = customerManager.GetClientByTelegram(userId.ToString());

            int age = Get.GetAge(user.Birthday.Value);
            string caption = $"{user.FirstName} - {age}, {user.SearchCity}";
            var avatar = customerManager.GetAvatarFileByUserId(userId);

            MediaMessage mediaMessage = new MediaMessage();
            mediaMessage.СhatId = chatId;
            mediaMessage.Caption = caption;
            mediaMessage.File = avatar;

            InlineKeyboardMarkup replyKeyboard = null;
            if (HasUserAdditionlInfo(user))
            {
                List<InlineKeyboardButton> inlineKeyboardButtons = new List<InlineKeyboardButton>();
                replyKeyboard = new InlineKeyboardMarkup(new[]
                {
                    inlineKeyboardButtons
                });
                inlineKeyboardButtons.Add(InlineKeyboardButton.WithCallbackData(_messages[ReplyButton.MORE_DETAILS], inlineKeyboardButtons.GetInlineId($"{source}:{userId}")));
            }
            mediaMessage.ReplyMarkup = replyKeyboard;
            return await _telegramBotManager.SendMediaMessage(mediaMessage);
        }

        /// <summary>
        /// Отправляет совпадение анкеты
        /// </summary>
        /// <param name="update"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Message?> SendMatchApplication(Update update, long userId, IReplyMarkup? replyMarkup = null, params string[] additionalTexts)
        {
            CustomerManager customerManager = _dataManager == null ? new CustomerManager() : _dataManager.GetData<ICustomerManager>() as CustomerManager;
            _telegramBotManager = _dataManager != null ? _dataManager.GetData<TelegramBotManager>() : _telegramBotManager;
            long chatId = Get.GetChatId(update);
            long myClientTelegramId = Get.GetUserId(update);
            ClientViewModel myClient = myClientTelegramId != userId ? _dataManager.GetData<ICustomerManager>().GetClientByTelegram(myClientTelegramId.ToString()) : null;
            Message message = null;
            if (myClient != null)
            {
                ClientMatchUncheckedViewModel matchViewModel = customerManager.GetTargetClientUncheckedMatch(myClient.ClientMatchInfo.ClientMatchInfoId, userId.ToString());
                if (matchViewModel != null)
                {
                    string additionalText = string.Empty;
                    //NOTE:реализовать систему с пробросом дополнительных услоовий для новых текстов
                    foreach (var text in additionalTexts)
                    {
                        if (!text.IsNull())
                        {
                            additionalText += $"\n{text}";
                        }
                    }
                    if (matchViewModel.MatchType == MatchType.Like.ToString())
                    {
                        message = await _telegramBotManager.SendTextMessage(chatId, _messages[MessageKey.SEND_LIKE_MATCH] + additionalText, replyMarkup: replyMarkup);
                    }
                    else if (matchViewModel.MatchType == MatchType.LoveLetter.ToString() && !matchViewModel.LoveLetterText.IsNull())
                    {
                        message = await _telegramBotManager.SendTextMessage(chatId, Get.ReplaceKeysInText(_messages[MessageKey.SEND_LOVE_LETTER_MATCH] + additionalText, new Dictionary<string, string>() { { Promt.MESSAGE, matchViewModel.LoveLetterText } }), replyMarkup: replyMarkup);
                    }
                    else if (matchViewModel.MatchType == MatchType.LoveLetter.ToString() && (matchViewModel.ClientMatchUncheckedVideoNoteInfo != null || matchViewModel.ClientMatchUncheckedVideoInfo != null))
                    {
                        MediaType mediaType = matchViewModel.ClientMatchUncheckedVideoNoteInfo != null ? MediaType.VideoNote : MediaType.Video;
                        var loveVideoFileId = mediaType == MediaType.VideoNote ? new InputFileId(matchViewModel.ClientMatchUncheckedVideoNoteInfo.FileId) : new InputFileId(matchViewModel.ClientMatchUncheckedVideoInfo.FileId);
                        var loveVideoFile = new InputMediaCustom(loveVideoFileId, mediaType);

                        MediaMessage loveMediaMessage = new MediaMessage();
                        loveMediaMessage.СhatId = chatId;
                        loveMediaMessage.Caption = _messages[MessageKey.SEND_VIDEO_MATCH] + additionalText;
                        loveMediaMessage.File = loveVideoFile;
                        loveMediaMessage.ReplyMarkup = replyMarkup;
                        message = await _telegramBotManager.SendMediaMessage(loveMediaMessage);
                    }
                    customerManager.SetWatchClientMatch(matchViewModel);
                    customerManager.UpdateTimeShowNewLikes(myClient.ClientMatchInfo);
                }
            }
            return message;
        }

        /// <summary>
        /// Отправить медиа файлы пользователя
        /// </summary>
        /// <param name="update"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Message?> SendUserMedia(long chatId, long userId)
        {
            InitApplicationKeyboards();
            CustomerManager customerManager = _dataManager == null ? new CustomerManager() : _dataManager.GetData<ICustomerManager>() as CustomerManager;
            _telegramBotManager = _dataManager != null ? _dataManager.GetData<TelegramBotManager>() : _telegramBotManager;

            var media = customerManager.GetMediaFilesByUserId(userId);
            Message message = null;
            if (media != null && media.Any())
            {
                if (media.Count == 1)
                {
                    InputMediaCustom file = media.FirstOrDefault();
                    message = await _telegramBotManager.SendMediaMessage(chatId, file);
                }
                else
                {
                    var messages = await _telegramBotManager.SendMediaGroupMessage(chatId, media);
                    message = messages?[0];
                }
            }
            customerManager?.Dispose();
            return message;
        }

        /// <summary>
        /// Редактирование анкеты с подробной информацией
        /// </summary>
        /// <param name="update">Инфо от кого пришел запрос</param>
        /// <param name="userId">Id клиента анкеты, которую надо отправить</param>
        /// <returns></returns>
        public async Task<Message?> EditMoreInfoApplication(Update update, long userId, string source)
        {
            InitApplicationKeyboards();
            CustomerManager customerManager = _dataManager == null ? new CustomerManager() : _dataManager.GetData<ICustomerManager>() as CustomerManager;
            _telegramBotManager = _dataManager != null ? _dataManager.GetData<TelegramBotManager>() : _telegramBotManager;

            var user = customerManager.GetClientByTelegram(userId.ToString());

            int age = Get.GetAge(user.Birthday.Value);
            string caption = $"{user.FirstName} - {age}, {user.SearchCity}";

            List<InlineKeyboardButton> inlineKeyboardButtons = new List<InlineKeyboardButton>();
            InlineKeyboardMarkup replyKeyboard = new InlineKeyboardMarkup(new[]
            {
                inlineKeyboardButtons
            });
            inlineKeyboardButtons.Add(InlineKeyboardButton.WithCallbackData(_messages[ReplyButton.MORE_DETAILS], inlineKeyboardButtons.GetInlineId($"{source}:{userId}")));

            //NOTE: DRY  - объединить класс модели рекдактирования сообщения
            if (update.CallbackQuery.Message.Type != MessageType.Text)
            {
                EditMediaMessage editMediaMessage = new EditMediaMessage();
                editMediaMessage.Сhat = update.CallbackQuery.Message.Chat;
                editMediaMessage.MessageId = update.CallbackQuery.Message.MessageId;
                editMediaMessage.Text = caption;

                editMediaMessage.ReplyMarkup = replyKeyboard;

                customerManager?.Dispose();
                return await _telegramBotManager.EditCaptionMessage(editMediaMessage);
            }
            else
            {
                EditTextMessage editTextMessage = new EditTextMessage();
                editTextMessage.Сhat = update.CallbackQuery.Message.Chat;
                editTextMessage.MessageId = update.CallbackQuery.Message.MessageId;
                editTextMessage.Text = caption;

                editTextMessage.ReplyMarkup = replyKeyboard;

                customerManager?.Dispose();
                return await _telegramBotManager.EditTextMessage(editTextMessage);
            }

        }


        /// <summary>
        /// Редактирование анкеты с краткой информацией
        /// </summary>
        /// <param name="update">Инфо от кого пришел запрос</param>
        /// <param name="userId">Id клиента анкеты, которую надо отправить</param>
        /// <returns></returns>
        public async Task<Message?> EditLessInfoApplication(Update update, long userId, string source)
        {
            InitApplicationKeyboards();
            CustomerManager customerManager = _dataManager == null ? new CustomerManager() : _dataManager.GetData<ICustomerManager>() as CustomerManager;
            _telegramBotManager = _dataManager != null ? _dataManager.GetData<TelegramBotManager>() : _telegramBotManager;

            var user = customerManager.GetClientByTelegram(userId.ToString());

            int age = Get.GetAge(user.Birthday.Value);
            string caption = $"{user.FirstName} - {age}, {user.SearchCity}";

            //NOTE: DRY  - объединить класс модели рекдактирования сообщения
            if (update.CallbackQuery.Message.Type != MessageType.Text)
            {
                EditMediaMessage editMediaMessage = new EditMediaMessage();
                editMediaMessage.Сhat = update.CallbackQuery.Message.Chat;
                editMediaMessage.MessageId = update.CallbackQuery.Message.MessageId;
                editMediaMessage.Text = caption;

                List<InlineKeyboardButton> inlineKeyboardButtons = new List<InlineKeyboardButton>();
                InlineKeyboardMarkup replyKeyboard = new InlineKeyboardMarkup(new[]
                {
                    inlineKeyboardButtons
                });
                inlineKeyboardButtons.Add(InlineKeyboardButton.WithCallbackData(_messages[ReplyButton.LESS_DETAILS], inlineKeyboardButtons.GetInlineId($"{source}:{userId}")));

                foreach (var keyboard in _applicationKeyboards)
                {
                    if (keyboard.CheckCondition(user))
                    {
                        var button = keyboard.GetKeyboardButton(inlineKeyboardButtons, userId, source);
                        if (button != null)
                        {
                            inlineKeyboardButtons.Add(button);
                        }
                        keyboard.UpdateMessage(user, editMediaMessage);
                    }
                }

                editMediaMessage.ReplyMarkup = replyKeyboard;

                customerManager?.Dispose();
                return await _telegramBotManager.EditCaptionMessage(editMediaMessage);
            }
            else
            {
                EditTextMessage editTextMessage = new EditTextMessage();
                editTextMessage.Сhat = update.CallbackQuery.Message.Chat;
                editTextMessage.MessageId = update.CallbackQuery.Message.MessageId;
                editTextMessage.Text = caption;

                List<InlineKeyboardButton> inlineKeyboardButtons = new List<InlineKeyboardButton>();
                InlineKeyboardMarkup replyKeyboard = new InlineKeyboardMarkup(new[]
                {
                    inlineKeyboardButtons
                });
                inlineKeyboardButtons.Add(InlineKeyboardButton.WithCallbackData(_messages[ReplyButton.LESS_DETAILS], inlineKeyboardButtons.GetInlineId($"{source}:{userId}")));

                foreach (var keyboard in _applicationKeyboards)
                {
                    if (keyboard.CheckCondition(user))
                    {
                        var button = keyboard.GetKeyboardButton(inlineKeyboardButtons, userId, source);
                        if (button != null)
                        {
                            inlineKeyboardButtons.Add(button);
                        }
                        keyboard.UpdateMessage(user, editTextMessage);
                    }
                }

                editTextMessage.ReplyMarkup = replyKeyboard;

                customerManager?.Dispose();
                return await _telegramBotManager.EditTextMessage(editTextMessage);
            }
        }

        private bool HasUserAdditionlInfo(ClientViewModel user)
        {
            foreach (var keyboard in _applicationKeyboards)
            {
                if (keyboard.CheckCondition(user))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
