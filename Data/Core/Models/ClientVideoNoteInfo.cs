namespace Data.Core.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class ClientVideoNoteInfo
    {
        public int ClientVideoNoteInfoId { get; set; }

        public int ClientMediaInfoId { get; set; }

        [ForeignKey("ClientMediaInfoId")]
        public ClientMediaInfo ClientMediaInfo { get; set; }

        public string? MediaGroupId { get; set; }

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
