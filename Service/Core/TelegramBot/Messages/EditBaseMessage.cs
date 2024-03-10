namespace Service.Core.TelegramBot.Messages
{
    using Telegram.Bot.Types;

    /// <summary>
    /// Базовая реализация сообщения редактирования
    /// </summary>
    public class EditBaseMessage
    {
        /// <summary>
        /// Id чата куда отправлять
        /// </summary>
        public Chat? Сhat = default;

        /// <summary>
        /// Id сообщения
        /// </summary>
        public int MessageId = default;

        /// <summary>
        /// Текст описания
        /// </summary>
        public string? Text = default;
    }
}
