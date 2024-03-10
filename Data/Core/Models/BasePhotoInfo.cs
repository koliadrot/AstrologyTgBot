namespace Data.Core.Models
{
    public class BasePhotoInfo
    {
        public string FileId { get; set; }

        public string FileUniqueId { get; set; }

        public long? FileSize { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public byte[]? Photo { get; set; }
    }
}
