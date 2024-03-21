namespace Service.Core.TelegramBot.Notifies
{
    using Service.Abstract.TelegramBot;
    using Service.Core.TelegramBot.Commands;
    using Service.Support;
    using Service.ViewModels;
    using Telegram.Bot.Types;
    using Telegram.Bot.Types.Enums;
    using Telegram.Bot.Types.ReplyMarkups;

    /// <summary>
    /// Уведомление о совпадении симпатии
    /// </summary>
    public class MatchNotify : INotify
    {
        private readonly DataManager _dataManager;
        private readonly Dictionary<string, string> _messages;
        private readonly TelegramSupport _telegramSupport;

        public string Name => nameof(MatchNotify);

        public MatchNotify(DataManager dataManager)
        {
            _dataManager = dataManager;
            _telegramSupport = new TelegramSupport(dataManager);
            _messages = _dataManager.GetData<CommandExecutor>().Messages;
        }

        public bool IsInteraction(Update update)
        {
            bool isInteraction = false;
            var callbackQuery = update.CallbackQuery;
            if (callbackQuery != null)
            {
                var selectedCallbackData = callbackQuery.Data;
                var inlineData = callbackQuery.Message.ReplyMarkup?.InlineKeyboard.SelectMany(x => x).FirstOrDefault(x => x.CallbackData == selectedCallbackData);
                if (inlineData != null)
                {
                    string inlineNotifyEntity = inlineData.CallbackData.Split(':')[1];
                    isInteraction = inlineNotifyEntity == nameof(MatchNotify)
                        || inlineNotifyEntity == nameof(FindApplicationCommand)
                        || inlineNotifyEntity == nameof(CheckMatchCommand)
                        || inlineNotifyEntity == nameof(GetClientApplicationCommand)
                        || inlineNotifyEntity == nameof(MyApplicationCommand);
                }
            }
            return isInteraction;
        }

        public async Task InlineAction(Update update)
        {
            var callbackQuery = update.CallbackQuery;
            if (callbackQuery != null)
            {
                var selectedCallbackData = callbackQuery.Data;
                var inlineData = callbackQuery.Message.ReplyMarkup?.InlineKeyboard.SelectMany(x => x).FirstOrDefault(x => x.CallbackData == selectedCallbackData);
                if (inlineData != null)
                {
                    string inlineNotifyEntity = inlineData.CallbackData.Split(':')[1];
                    if (inlineNotifyEntity == nameof(MatchNotify) || inlineNotifyEntity == nameof(FindApplicationCommand) || inlineNotifyEntity == nameof(CheckMatchCommand) || inlineNotifyEntity == nameof(GetClientApplicationCommand) || inlineNotifyEntity == nameof(MyApplicationCommand))
                    {
                        var inlineInfo = inlineData.CallbackData.Split(':')[2];
                        if (long.TryParse(inlineInfo, out long userId))
                        {
                            if (inlineData.Text == _messages[ReplyButton.SHOW_APPLICATION])
                            {
                                await SendApplication(update, userId);
                            }
                            else if (inlineData.Text == _messages[ReplyButton.MORE_DETAILS])
                            {
                                await EditLessInfoApplication(update, userId);
                            }
                            else if (inlineData.Text == _messages[ReplyButton.LESS_DETAILS])
                            {
                                await EditMoreInfoApplication(update, userId);
                            }
                            else if (inlineData.Text == _messages[ReplyButton.MORE_MEDIA])
                            {
                                long chatId = Get.GetChatId(update);
                                await SendUserMedia(chatId, userId);
                            }
                        }
                    }
                }
            }
        }

        public async Task Send(ClientViewModel myClient, ClientViewModel partnerClient)
        {
            if (partnerClient != null && myClient != null && long.TryParse(partnerClient.TelegramId, out long partnerTelegramId) && long.TryParse(myClient.TelegramId, out long myTelegramId))
            {
                await _dataManager.GetData<TelegramBotManager>().SendTextMessage(partnerTelegramId, Get.ReplaceKeysInText(_messages[MessageKey.SEND_MATCH_MY_CLIENT], new Dictionary<string, string>() { { Promt.USER_NAME, Get.GetValidUserName(myClient.UserName, ParseMode.Markdown) } }), parseMode: ParseMode.Markdown, replyMarkup: GetKeyboardMarkup(myClient.TelegramId));
                await _dataManager.GetData<TelegramBotManager>().SendTextMessage(myTelegramId, Get.ReplaceKeysInText(_messages[MessageKey.SEND_MATCH_OTHER_CLIENT], new Dictionary<string, string>() { { Promt.USER_NAME, Get.GetValidUserName(partnerClient.UserName, ParseMode.Markdown) } }), parseMode: ParseMode.Markdown, replyMarkup: GetKeyboardMarkup(partnerClient.TelegramId));
            }
        }

        private async Task<Message?> SendApplication(Update update, long userId) => await _telegramSupport.SendUserApplication(update, userId, nameof(MatchNotify));
        private async Task<Message?> SendUserMedia(long chatId, long userId) => await _telegramSupport.SendUserMedia(chatId, userId);
        private async Task<Message?> EditMoreInfoApplication(Update update, long userId) => await _telegramSupport.EditMoreInfoApplication(update, userId, nameof(MatchNotify));
        private async Task<Message?> EditLessInfoApplication(Update update, long userId) => await _telegramSupport.EditLessInfoApplication(update, userId, nameof(MatchNotify));

        private InlineKeyboardMarkup GetKeyboardMarkup(string userId)
        {
            List<InlineKeyboardButton> inlineKeyboardButtons = new List<InlineKeyboardButton>();
            InlineKeyboardMarkup replyKeyboard = new InlineKeyboardMarkup(new[]
            {
                inlineKeyboardButtons
            });
            inlineKeyboardButtons.Add(InlineKeyboardButton.WithCallbackData(_messages[ReplyButton.SHOW_APPLICATION], inlineKeyboardButtons.GetInlineId($"{nameof(MatchNotify)}:{userId}")));
            return replyKeyboard;
        }
    }
}
