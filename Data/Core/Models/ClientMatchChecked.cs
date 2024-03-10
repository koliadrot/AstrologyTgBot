namespace Data.Core.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ClientMatchChecked : BaseClientMatch
    {
        [Key]
        public int ClientMatchCheckedId { get; set; }

        public virtual ClientMatchCheckedVideoNoteInfo? ClientMatchCheckedVideoNoteInfo { get; set; }
        public virtual ClientMatchCheckedVideoInfo? ClientMatchCheckedVideoInfo { get; set; }
    }
}
