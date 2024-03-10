namespace Service.Core.TelegramBot.Messages
{
    using Service.Support;
    using Telegram.Bot.Types.Enums;
    using Telegram.Bot.Types.ReplyMarkups;

    /// <summary>
    /// Медиа сообщение
    /// </summary>
    public class MediaMessage
    {
        /// <summary>
        /// Id чата куда отправлять
        /// </summary>
        public long СhatId = default;

        /// <summary>
        /// Медиа файл
        /// </summary>
        public InputMediaCustom? File = default;

        /// <summary>
        /// Текст описания
        /// </summary>
        public string? Caption = default;

        /// <summary>
        /// Кливиатура ответа
        /// </summary>
        public IReplyMarkup? ReplyMarkup = null;

        /// <summary>
        /// Тип парсинга текста
        /// </summary>
        public ParseMode? ParseMode = null;
    }
}