namespace Service.ViewModels.TelegramModels
{
    /// <summary>
    /// Сообщение телеграмм бота
    /// </summary>
    public class TelegramBotParamMessageViewModel
    {
        /// <summary>
        /// Уникальный номер сообщения
        /// </summary>
        public int MessageId { get; set; }

        /// <summary>
        /// Уникальный номр бота
        /// </summary>
        public int TelegramBotId { get; set; }

        /// <summary>
        /// Уникальное имя сообщения
        /// </summary>
        public string MessageName { get; set; }

        /// <summary>
        /// Описание сообщения
        /// </summary>
        public string MessageDescription { get; set; }

        /// <summary>
        /// Значение сообщения
        /// </summary>
        public string MessageValue { get; set; }

        /// <summary>
        /// Значение сообщения по умолчанию
        /// </summary>
        public string MessageValueDefault { get; set; }

        /// <summary>
        /// Данное сообщение является кнопкой под строкой ввода в чате телеграмма
        /// </summary>
        public bool IsButton { get; set; }
    }
}
