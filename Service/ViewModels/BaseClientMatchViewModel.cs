namespace Service.ViewModels
{
    using System;

    /// <summary>
    /// Базовая модель совпадения клиента
    /// </summary>
    public class BaseClientMatchViewModel
    {
        /// <summary>
        /// Id сводной инфрмации клиента
        /// </summary>
        public int ClientMatchInfoId { get; set; }

        /// <summary>
        /// Время совпадения
        /// </summary>
        public DateTime? DateMatch { get; set; }

        /// <summary>
        /// Тип совпадения
        /// </summary>
        public string? MatchType { get; set; }

        /// <summary>
        /// Время ответа клиента на совпадения
        /// </summary>
        public DateTime? AnswearDateMatch { get; set; }

        /// <summary>
        /// Ответ клиента на совпадение
        /// </summary>
        public string? AnswearMatchType { get; set; }

        /// <summary>
        /// Id телеграмм клиента который выставил совпадение
        /// </summary>
        public string? MatchTelegramId { get; set; }

        /// <summary>
        /// Дополнительная информация
        /// </summary>
        public string? LoveLetterText { get; set; }
    }
}
