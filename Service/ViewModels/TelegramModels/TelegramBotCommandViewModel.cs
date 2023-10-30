namespace Service.ViewModels.TelegramModels
{
    /// <summary>
    /// Команды телеграм бота
    /// </summary>
    public class TelegramBotCommandViewModel
    {
        /// <summary>
        /// Уникальный номер команды
        /// </summary>
        public int BotCommandId { get; set; }

        /// <summary>
        /// Уникальный номер телеграмм бота,
        /// которому принадлежит команда
        /// </summary>
        public int TelegramBotId { get; set; }

        /// <summary>
        /// Название команды
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание команды
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Техническое имя команды (/start)
        /// </summary>
        public string CommandName { get; set; }

        /// <summary>
        /// Включена ли команда
        /// </summary>
        public bool IsEnable { get; set; }

        /// <summary>
        /// Тип комманды
        /// см в TelegramBotCommandType.cs
        /// Custom - многофункциональный
        /// Link - ссылочный
        /// </summary>
        public string CommandType { get; set; }

        /// <summary>
        /// Команда доступна только после авторизации
        /// </summary>
        public bool IsAuth { get; set; }

        /// <summary>
        /// Команда доступна всегда
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// Публичная команда
        /// </summary>
        public bool IsPublic { get; set; }

        /// <summary>
        /// Дополнительная информация к команде
        /// </summary>
        public string AdditionalData { get; set; }
    }
}
