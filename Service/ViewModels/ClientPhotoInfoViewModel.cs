namespace Service.ViewModels
{
    /// <summary>
    /// Фотоинформация клиента
    /// </summary>
    public class ClientPhotoInfoViewModel
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
        /// Id фото
        /// </summary>
        public string FileId { get; set; }

        /// <summary>
        /// Id фото
        /// </summary>
        public string FileUniqueId { get; set; }

        /// <summary>
        /// Размер фото
        /// </summary>
        public long? FileSize { get; set; }

        /// <summary>
        /// Ширина фото
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Высота фото
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Массив байтов
        /// </summary>
        public byte[]? Photo { get; set; }
    }
}
