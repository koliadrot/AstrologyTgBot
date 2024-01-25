namespace Data.Core.Models
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ClientMediaInfo
    {
        public int ClientMediaInfoId { get; set; }

        public int ClientId { get; set; }

        [ForeignKey("ClientId")]
        public Client Client { get; set; }

        [DisplayName("Фотографии")]
        public virtual ICollection<ClientPhotoInfo> ClientPhotoInfos { get; set; } = new List<ClientPhotoInfo>();

        [DisplayName("Видео")]
        public virtual ICollection<ClientVideoInfo> ClientVideoInfos { get; set; } = new List<ClientVideoInfo>();
    }
}
