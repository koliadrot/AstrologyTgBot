﻿namespace Data.Core.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class ClientPhotoInfo
    {
        public int ClientPhotoInfoId { get; set; }

        public int ClientMediaInfoId { get; set; }

        [ForeignKey("ClientMediaInfoId")]
        public ClientMediaInfo ClientMediaInfo { get; set; }

        public string? MediaGroupId { get; set; }

        public string FileId { get; set; }

        public string FileUniqueId { get; set; }

        public long? FileSize { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public byte[]? Photo { get; set; }
    }
}
