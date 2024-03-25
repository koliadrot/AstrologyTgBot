namespace Service.Abstract.TelegramBot
{
    using Telegram.Bot.Types;

    /// <summary>
    /// Вспомогающий исполнительная команда
    /// </summary>
    public interface ISupportCommandExecutor
    {
        /// <summary>
        /// Ключ
        /// </summary>
        string Key { get; }

        /// <summary>
        /// Исполнение
        /// </summary>
        /// <returns></returns>
        Task SupportExecute(Update update);
    }
}
