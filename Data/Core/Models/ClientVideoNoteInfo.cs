namespace Data.Core.Models
{
    using Data.Core.Interfaces;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ClientVideoNoteInfo : BaseVideoNoteInfo, IAvatarable
    {
        public int ClientVideoNoteInfoId { get; set; }

        public int ClientMediaInfoId { get; set; }

        [ForeignKey(nameof(ClientMediaInfoId))]
        public ClientMediaInfo ClientMediaInfo { get; set; }

        public string? MediaGroupId { get; set; }

        [DisplayName("Аватар")]
        public bool? IsAvatar { get; set; }
    }
}
