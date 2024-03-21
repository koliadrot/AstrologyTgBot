namespace Service.Core.TelegramBot.Notifies
{
    using Service.Abstract;
    using Service.Abstract.TelegramBot;
    using Service.Core.TelegramBot.Commands;
    using Service.ViewModels;
    using System.Threading.Tasks;
    using Telegram.Bot.Types;
    using Telegram.Bot.Types.ReplyMarkups;

    /// <summary>
    /// Уведомление о новых лайках
    /// </summary>
    public class NewLikesNotify : INotify
    {
        private readonly DataManager _dataManager;
        private readonly Dictionary<string, string> _messages;

        public NewLikesNotify(DataManager dataManager)
        {
            _dataManager = dataManager;
            _messages = _dataManager.GetData<CommandExecutor>().Messages;
        }

        public string Name => nameof(NewLikesNotify);

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
                    isInteraction = inlineNotifyEntity == nameof(NewLikesNotify);
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
                    if (inlineNotifyEntity == nameof(NewLikesNotify))
                    {
                        if (inlineData.Text == _messages[ReplyButton.WATCH_LIKES])
                        {
                            CheckMatchCommand checkMatchCommand = _dataManager.GetData<CommandExecutor>().GetCommandByType(typeof(CheckMatchCommand)) as CheckMatchCommand;
                            await checkMatchCommand?.Execute(update);
                        }
                    }
                }
            }
        }

        public async Task<Message?> Send(ClientViewModel clientViewModel)
        {
            Message message = null;
            if (clientViewModel != null && _dataManager.GetData<ICustomerManager>().HasClientNewLikes(clientViewModel.ClientMatchInfo))
            {
                int newLikes = _dataManager.GetData<ICustomerManager>().NewLikesCountByClientMatchInfo(clientViewModel.ClientMatchInfo);
                if (newLikes > 0 && long.TryParse(clientViewModel.TelegramId, out long targetUserId))
                {
                    List<InlineKeyboardButton> inlineKeyboardButtons = new List<InlineKeyboardButton>();
                    InlineKeyboardMarkup replyKeyboard = new InlineKeyboardMarkup(new[]
                    {
                        inlineKeyboardButtons
                    });
                    inlineKeyboardButtons.Add(InlineKeyboardButton.WithCallbackData(_messages[ReplyButton.WATCH_LIKES], inlineKeyboardButtons.GetInlineId(nameof(NewLikesNotify))));

                    message = await _dataManager.GetData<TelegramBotManager>().SendTextMessage(targetUserId, Get.ReplaceKeysInText(_messages[MessageKey.SEND_NOTIFY_NEW_LIKES], new Dictionary<string, string>() { { Promt.LIKES, newLikes.ToString() } }), replyMarkup: replyKeyboard);
                    _dataManager.GetData<ICustomerManager>().UpdateTimeShowNewLikes(clientViewModel.ClientMatchInfo);
                }
            }
            return message;
        }
    }
}
