namespace Data.Core.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class ClientVideoInfo
    {
        public int ClientVideoInfoId { get; set; }

        public int ClientMediaInfoId { get; set; }

        [ForeignKey("ClientMediaInfoId")]
        public ClientMediaInfo ClientMediaInfo { get; set; }

        public string? MediaGroupId { get; set; }

        public string FileId { get; set; }

        public string FileUniqueId { get; set; }

        public long? FileSize { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public int Duration { get; set; }

        public string? MimeType { get; set; }

        public byte[]? Video { get; set; }

        public string? ThumbnailFileId { get; set; }

        public string? ThumbnailFileUniqueId { get; set; }

        public long? ThumbnailFileSize { get; set; }

        public int? ThumbnailWidth { get; set; }

        public int? ThumbnailHeight { get; set; }

        public byte[]? Thumbnail { get; set; }
    }
}
