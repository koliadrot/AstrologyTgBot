namespace Service.ViewModels.TelegramModels
{
    /// <summary>
    /// Условие регистрации
    /// </summary>
    public class TelegramBotRegisterConditionViewModel
    {
        /// <summary>
        /// Уникальный номер условия
        /// </summary>
        public int RegisterConditionId { get; set; }

        /// <summary>
        /// Уникальный номер телеграм бота
        /// </summary>
        public int TelegramBotId { get; set; }

        /// <summary>
        /// Название условия
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Техническое название условия
        /// </summary>
        public string ConditionName { get; set; }

        /// <summary>
        /// Можно ли пропустить условие?
        /// </summary>
        public bool IsCanPass { get; set; }

        /// <summary>
        /// Условие обязательное?
        /// </summary>
        public bool IsNecessarily { get; set; }

        /// <summary>
        /// Автивировано ли условие?
        /// </summary>
        public bool IsEnable { get; set; }

        /// <summary>
        /// Порядоковый номер в списке условий
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Информационное условие
        /// </summary>
        public bool IsInfo { get; set; }
    }
}
