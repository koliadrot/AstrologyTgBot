namespace Service.Core.TelegramBot.ConditionKeyboard
{
    using Service.Abstract.TelegramBot;
    using Service.Core.TelegramBot.Messages;
    using Service.Core.TelegramBot.RegisterCondition;
    using Service.Extensions;
    using Service.ViewModels;
    using System.Collections.Generic;
    using Telegram.Bot.Types.ReplyMarkups;


    /// <summary>
    /// Кнопка описания
    /// </summary>
    public class AboutMeKeyboardCondition : IConditionKeyboard
    {
        public string ConditionName => nameof(AboutMeCondition);

        private Dictionary<string, string> _messages;
        private DataManager _dataManager;

        public AboutMeKeyboardCondition(DataManager dataManager, Dictionary<string, string> messages)
        {
            _messages = messages;
            _dataManager = dataManager;
        }

        public InlineKeyboardButton? GetKeyboardButton(List<InlineKeyboardButton> inlineKeyboardButtons, long userId, string source) => null;

        public bool CheckCondition(ClientViewModel user) => !user.AboutMe.IsNull();

        public void UpdateMessage(ClientViewModel user, EditBaseMessage? mediaMessage)
        {
            if (!user.AboutMe.IsNull() && mediaMessage != null)
            {
                mediaMessage.Text += $"\n{user.AboutMe}";
            }
        }
    }
}
