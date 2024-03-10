namespace Service.ViewModels
{
    using System.Collections.Generic;

    /// <summary>
    /// Сводная информация по совпадениям
    /// </summary>
    public class ClientMatchInfoViewModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public int ClientMatchInfoId { get; set; }

        /// <summary>
        /// Id клиента
        /// </summary>
        public int ClientId { get; set; }

        /// <summary>
        /// Кол-во лайков
        /// </summary>
        public double Likes { get; set; }

        /// <summary>
        /// Кол-во лайков с описанием
        /// </summary>
        public double LetterLikes { get; set; }

        /// <summary>
        /// Кол-во дизлайков
        /// </summary>
        public double Dislikes { get; set; }

        /// <summary>
        /// Последний показ
        /// </summary>
        public DateTime? LastShowMatches { get; set; }

        /// <summary>
        /// Не проверенные совпадения
        /// </summary>
        public ICollection<ClientMatchUncheckedViewModel> UncheckedClientMatchs { get; set; }

        /// <summary>
        /// Проверенные совпадения
        /// </summary>
        public ICollection<ClientMatchCheckedViewModel> CheckedClientMatchs { get; set; }
    }
}
