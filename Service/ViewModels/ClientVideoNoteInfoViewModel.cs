namespace Service.ViewModels
{
    /// <summary>
    /// Видео-Кружок клиента
    /// </summary>
    public class ClientVideoNoteInfoViewModel
    {
        /// <summary>
        /// Id видео-кружок
        /// </summary>
        public int ClientVideoNoteInfoId { get; set; }

        /// <summary>
        /// Id медиа информации
        /// </summary>
        public int ClientMediaInfoId { get; set; }

        /// <summary>
        /// Id альбом группы
        /// </summary>
        public string? MediaGroupId { get; set; }

        /// <summary>
        /// Id видео-кружок
        /// </summary>
        public string FileId { get; set; }

        /// <summary>
        /// Id видео-кружок
        /// </summary>
        public string FileUniqueId { get; set; }

        /// <summary>
        /// Размер видео-кружок
        /// </summary>
        public long? FileSize { get; set; }

        /// <summary>
        /// Длина видео-кружок
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Длительность
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// Массив байтов видео-кружка
        /// </summary>
        public byte[]? VideoNote { get; set; }

        /// <summary>
        /// Id превью видео-кружок
        /// </summary>
        public string? ThumbnailFileId { get; set; }

        /// <summary>
        /// Id превью видео-кружок
        /// </summary>
        public string? ThumbnailFileUniqueId { get; set; }

        /// <summary>
        /// Размер превью видео-кружок
        /// </summary>
        public long? ThumbnailFileSize { get; set; }

        /// <summary>
        /// Ширина превью видео-кружок
        /// </summary>
        public int? ThumbnailWidth { get; set; }

        /// <summary>
        /// Высота превью видео-кружок
        /// </summary>
        public int? ThumbnailHeight { get; set; }

        /// <summary>
        /// Массив байтов превью видео-кружок
        /// </summary>
        public byte[]? Thumbnail { get; set; }
    }
}
