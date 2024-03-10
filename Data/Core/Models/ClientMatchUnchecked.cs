namespace Data.Core.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ClientMatchUnchecked : BaseClientMatch
    {
        [Key]
        public int ClientMatchUncheckedId { get; set; }
        public bool IsWatched { get; set; }
        public virtual ClientMatchUncheckedVideoNoteInfo? ClientMatchUncheckedVideoNoteInfo { get; set; }
        public virtual ClientMatchUncheckedVideoInfo? ClientMatchUncheckedVideoInfo { get; set; }
    }
}
