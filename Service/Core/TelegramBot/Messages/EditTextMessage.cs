namespace Service.Core.TelegramBot.Messages
{
    using Telegram.Bot.Types.Enums;
    using Telegram.Bot.Types.ReplyMarkups;

    /// <summary>
    /// Редактируемое текстовое сообщение
    /// </summary>
    public class EditTextMessage : EditBaseMessage
    {
        /// <summary>
        /// Кливиатура ответа
        /// </summary>
        public InlineKeyboardMarkup? ReplyMarkup = null;

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
