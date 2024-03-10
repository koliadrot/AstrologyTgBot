namespace Service.Core.TelegramBot.Messages
{
    using Telegram.Bot.Types.Enums;
    using Telegram.Bot.Types.ReplyMarkups;

    /// <summary>
    /// модель отредактированного медиа сообщения
    /// </summary>
    public class EditMediaMessage : EditBaseMessage
    {
        /// <summary>
        /// Кливиатура ответа
        /// </summary>
        public InlineKeyboardMarkup? ReplyMarkup = null;

        /// <summary>
        /// Тип парсинга текста
        /// </summary>
        public ParseMode? ParseMode = null;
    }
}
