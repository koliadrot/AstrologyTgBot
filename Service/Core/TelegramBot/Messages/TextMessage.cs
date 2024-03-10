namespace Service.Core.TelegramBot.Messages
{
    using Telegram.Bot.Types.Enums;
    using Telegram.Bot.Types.ReplyMarkups;

    /// <summary>
    /// Текстовое сообщение
    /// </summary>
    public class TextMessage
    {
        /// <summary>
        /// Id чата куда отправлять
        /// </summary>
        public long СhatId = default;

        /// <summary>
        /// Id сообщения
        /// </summary>
        public int MessageId = default;

        /// <summary>
        /// Текст сообщения
        /// </summary>
        public string? Text = default;

        /// <summary>
        /// Кливиатура ответа
        /// </summary>
        public IReplyMarkup? ReplyMarkup = null;

        /// <summary>
        /// Показывать превью ссылок после текста
        /// </summary>
        public bool IsShowPreview = true;

        /// <summary>
        /// Тип парсинга текста
        /// </summary>
        public ParseMode? ParseMode = null;
    }
}
