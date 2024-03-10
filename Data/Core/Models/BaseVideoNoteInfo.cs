namespace Data.Core.Models
{
    public class BaseVideoNoteInfo
    {
        public string FileId { get; set; }

        public string FileUniqueId { get; set; }

        public long? FileSize { get; set; }

        public int Length { get; set; }

        public int Duration { get; set; }

        public byte[]? VideoNote { get; set; }

        public string? ThumbnailFileId { get; set; }

        public string? ThumbnailFileUniqueId { get; set; }

        public long? ThumbnailFileSize { get; set; }

        public int? ThumbnailWidth { get; set; }

        public int? ThumbnailHeight { get; set; }

        public byte[]? Thumbnail { get; set; }
    }
}
