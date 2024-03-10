namespace Service.Core.TelegramBot.ConditionKeyboard
{
    using Service.Abstract;
    using Service.Abstract.TelegramBot;
    using Service.Core.TelegramBot.Messages;
    using Service.Core.TelegramBot.RegisterCondition;
    using Service.ViewModels;
    using System.Collections.Generic;
    using Telegram.Bot.Types.ReplyMarkups;

    /// <summary>
    /// Кнопка медиафайлов
    /// </summary>
    public class MediaKeyboardCondition : IConditionKeyboard
    {
        public string ConditionName => nameof(MediaCondition);

        private Dictionary<string, string> _messages;
        private DataManager _dataManager;

        public MediaKeyboardCondition(DataManager dataManager, Dictionary<string, string> messages)
        {
            _messages = messages;
            _dataManager = dataManager;
        }

        public InlineKeyboardButton? GetKeyboardButton(List<InlineKeyboardButton> inlineKeyboardButtons, long userId, string source) => InlineKeyboardButton.WithCallbackData(_messages[ReplyButton.MORE_MEDIA], inlineKeyboardButtons.GetInlineId($"{source}:{userId}"));

        public bool CheckCondition(ClientViewModel user) => _dataManager.GetData<ICustomerManager>().GetMediaFilesByUserId(user).Any();

        public void UpdateMessage(ClientViewModel user, EditBaseMessage? mediaMessage)
        {
            //NOTE:Пока что ничего не делает
        }
    }
}
