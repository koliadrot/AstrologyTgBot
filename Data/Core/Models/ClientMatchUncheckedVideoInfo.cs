namespace Data.Core.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class ClientMatchUncheckedVideoInfo : BaseVideoInfo
    {
        public int ClientMatchUncheckedVideoInfoId { get; set; }

        public int ClientMatchUncheckedId { get; set; }

        [ForeignKey(nameof(ClientMatchUncheckedId))]
        public ClientMatchUnchecked ClientMatchUnchecked { get; set; }
    }
}
