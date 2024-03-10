namespace Data.Core.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class ClientMatchUncheckedVideoNoteInfo : BaseVideoNoteInfo
    {
        public int ClientMatchUncheckedVideoNoteInfoId { get; set; }

        public int ClientMatchUncheckedId { get; set; }

        [ForeignKey(nameof(ClientMatchUncheckedId))]
        public ClientMatchUnchecked ClientMatchUnchecked { get; set; }
    }
}
