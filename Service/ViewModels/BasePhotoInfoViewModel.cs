namespace Service.ViewModels
{
    /// <summary>
    /// Базовая реализация фото
    /// </summary>
    public class BasePhotoInfoViewModel
    {
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
