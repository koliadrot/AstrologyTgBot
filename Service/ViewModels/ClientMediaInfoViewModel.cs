namespace Service.ViewModels
{
    using System.Collections.Generic;

    /// <summary>
    /// Медиаинформация клиента с фото
    /// </summary>
    public class ClientMediaInfoViewModel
    {
        /// <summary>
        /// Id медио информации
        /// </summary>
        public int ClientMediaInfoId { get; set; }

        /// <summary>
        /// Id клиента
        /// </summary>
        public int ClientId { get; set; }

        /// <summary>
        /// Фотографии
        /// </summary>
        public virtual ICollection<ClientPhotoInfoViewModel> ClientPhotoInfos { get; set; } = new List<ClientPhotoInfoViewModel>();

        /// <summary>
        /// Видео
        /// </summary>
        public virtual ICollection<ClientVideoInfoViewModel> ClientVideoInfos { get; set; } = new List<ClientVideoInfoViewModel>();
    }
}
