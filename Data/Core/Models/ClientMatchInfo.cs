namespace Data.Core.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ClientMatchInfo
    {
        [Key]
        public int ClientMatchInfoId { get; set; }

        public int ClientId { get; set; }

        [ForeignKey(nameof(ClientId))]
        public Client Client { get; set; }

        public int Likes { get; set; }

        public int LetterLikes { get; set; }

        public int Dislikes { get; set; }

        public DateTime? LastShowMatches { get; set; }

        public virtual ICollection<ClientMatchUnchecked> UncheckedClientMatchs { get; set; } = new List<ClientMatchUnchecked>();
        public virtual ICollection<ClientMatchChecked> CheckedClientMatchs { get; set; } = new List<ClientMatchChecked>();
    }
}
