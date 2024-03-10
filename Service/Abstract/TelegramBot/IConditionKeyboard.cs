namespace Service.Abstract.TelegramBot
{
    using Service.Core.TelegramBot.Messages;
    using Service.ViewModels;
    using Telegram.Bot.Types.ReplyMarkups;

    /// <summary>
    /// Условие кнопок
    /// </summary>
    public interface IConditionKeyboard
    {
        /// <summary>
        /// Имя условия
        /// </summary>
        string ConditionName { get; }

        /// <summary>
        /// Проверка условия
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        bool CheckCondition(ClientViewModel user);

        /// <summary>
        /// Обновление сообщения с картинкой
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        void UpdateMessage(ClientViewModel user, EditBaseMessage? mediaMessage);

        /// <summary>
        /// Получить кнопку клавиатуры
        /// </summary>
        InlineKeyboardButton? GetKeyboardButton(List<InlineKeyboardButton> inlineKeyboardButtons, long userId, string source);
    }
}
