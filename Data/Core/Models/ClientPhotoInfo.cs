namespace Data.Core.Models
{
    using Data.Core.Interfaces;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ClientPhotoInfo : BasePhotoInfo, IAvatarable
    {
        public int ClientPhotoInfoId { get; set; }

        public int ClientMediaInfoId { get; set; }

        [ForeignKey(nameof(ClientMediaInfoId))]
        public ClientMediaInfo ClientMediaInfo { get; set; }

        public string? MediaGroupId { get; set; }

        [DisplayName("Аватар")]
        public bool? IsAvatar { get; set; }
    }
}
