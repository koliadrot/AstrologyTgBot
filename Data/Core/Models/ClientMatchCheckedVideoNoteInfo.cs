namespace Data.Core.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class ClientMatchCheckedVideoNoteInfo : BaseVideoNoteInfo
    {
        public int ClientMatchCheckedVideoNoteInfoId { get; set; }

        public int ClientMatchCheckedId { get; set; }

        [ForeignKey(nameof(ClientMatchCheckedId))]
        public ClientMatchChecked ClientMatchChecked { get; set; }
    }
}
