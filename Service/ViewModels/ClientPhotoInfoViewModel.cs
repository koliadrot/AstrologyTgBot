namespace Service.ViewModels
{
    using Data.Core.Interfaces;

    /// <summary>
    /// Фотоинформация клиента
    /// </summary>
    public class ClientPhotoInfoViewModel : BasePhotoInfoViewModel, IAvatarable
    {
        /// <summary>
        /// Id фото
        /// </summary>
        public int ClientPhotoInfoId { get; set; }

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
