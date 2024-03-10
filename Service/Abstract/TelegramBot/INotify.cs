namespace Service.Abstract.TelegramBot
{
    using Telegram.Bot.Types;

    /// <summary>
    /// Уведомление
    /// </summary>
    public interface INotify
    {
        /// <summary>
        /// Взаимодействует ли пользователь с уведомлением
        /// </summary>
        /// <param name="update">Инфо</param>
        /// <returns></returns>
        bool IsInteraction(Update update);

        /// <summary>
        /// Взаимодействие inline действий с кнопками
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        Task InlineAction(Update update);
    }
}
