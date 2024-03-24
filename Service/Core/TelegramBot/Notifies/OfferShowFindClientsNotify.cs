namespace Service.Core.TelegramBot.Notifies
{
    using Service.Abstract.TelegramBot;
    using Service.Core.TelegramBot.Commands;
    using Service.ViewModels;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Telegram.Bot.Types;
    using Telegram.Bot.Types.ReplyMarkups;

    /// <summary>
    /// Предложить посмотреть анкеты
    /// </summary>
    public class OfferShowFindClientsNotify : INotify
    {
        private readonly DataManager _dataManager;
        private readonly Dictionary<string, string> _messages;

        public OfferShowFindClientsNotify(DataManager dataManager)
        {
            _dataManager = dataManager;
            _messages = _dataManager.GetData<CommandExecutor>().Messages;
        }

        public string Name => nameof(OfferShowFindClientsNotify);

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
                    isInteraction = inlineNotifyEntity == nameof(OfferShowFindClientsNotify);
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
                    if (inlineNotifyEntity == nameof(OfferShowFindClientsNotify))
                    {
                        if (inlineData.Text == _messages[ReplyButton.WATCH_FIND_CLIENTS])
                        {
                            FindApplicationCommand findApplicationCommand = _dataManager.GetData<CommandExecutor>().GetCommandByType(typeof(FindApplicationCommand)) as FindApplicationCommand;
                            await findApplicationCommand?.Execute(update);
                        }
                    }
                }
            }
        }

        public async Task<Message?> Send(ClientViewModel clientViewModel)
        {
            Message message = null;
            if (clientViewModel != null && long.TryParse(clientViewModel.TelegramId, out long targetUserId))
            {
                List<InlineKeyboardButton> inlineKeyboardButtons = new List<InlineKeyboardButton>();
                InlineKeyboardMarkup replyKeyboard = new InlineKeyboardMarkup(new[]
                {
                    inlineKeyboardButtons
                });
                inlineKeyboardButtons.Add(InlineKeyboardButton.WithCallbackData(_messages[ReplyButton.WATCH_FIND_CLIENTS], inlineKeyboardButtons.GetInlineId(nameof(OfferShowFindClientsNotify))));

                message = await _dataManager.GetData<TelegramBotManager>().SendTextMessage(targetUserId, _messages[MessageKey.SEND_NOTIFY_FIND_CLIENTS], replyMarkup: replyKeyboard);
            }
            return message;
        }
    }
}
