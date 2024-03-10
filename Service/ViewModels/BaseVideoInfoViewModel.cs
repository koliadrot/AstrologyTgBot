namespace Service.ViewModels
{
    /// <summary>
    /// Базовая реализация видео
    /// </summary>
    public class BaseVideoInfoViewModel
    {
        /// <summary>
        /// Id видео
        /// </summary>
        public string FileId { get; set; }

        /// <summary>
        /// Id видео
        /// </summary>
        public string FileUniqueId { get; set; }

        /// <summary>
        /// Размер видео
        /// </summary>
        public long? FileSize { get; set; }

        /// <summary>
        /// Ширина видео
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Высота видео
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Длительность
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// Тип видео
        /// </summary>
        public string? MimeType { get; set; }

        /// <summary>
        /// Массив байтов видео
        /// </summary>
        public byte[]? Video { get; set; }

        /// <summary>
        /// Id превью видео
        /// </summary>
        public string? ThumbnailFileId { get; set; }

        /// <summary>
        /// Id превью видео
        /// </summary>
        public string? ThumbnailFileUniqueId { get; set; }

        /// <summary>
        /// Размер превью видео
        /// </summary>
        public long? ThumbnailFileSize { get; set; }

        /// <summary>
        /// Ширина превью видео
        /// </summary>
        public int? ThumbnailWidth { get; set; }

        /// <summary>
        /// Высота превью видео
        /// </summary>
        public int? ThumbnailHeight { get; set; }

        /// <summary>
        /// Массив байтов превью видео
        /// </summary>
        public byte[]? Thumbnail { get; set; }
    }
}
