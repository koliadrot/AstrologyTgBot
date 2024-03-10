namespace Service.ViewModels
{
    using Data.Core.Interfaces;

    /// <summary>
    /// Видеоинформация клиента
    /// </summary>
    public class ClientVideoInfoViewModel : BaseVideoInfoViewModel, IAvatarable
    {
        /// <summary>
        /// Id видео-кружок
        /// </summary>
        public int ClientVideoInfoId { get; set; }

        /// <summary>
        /// Id медиа информации
        /// </summary>
        public int ClientMediaInfoId { get; set; }

        /// <summary>
        /// Id альбом группы
        /// </summary>
        public string? MediaGroupId { get; set; }

        /// <summary>
        /// Это аватар?
        /// </summary>
        public bool? IsAvatar { get; set; }
    }
}
