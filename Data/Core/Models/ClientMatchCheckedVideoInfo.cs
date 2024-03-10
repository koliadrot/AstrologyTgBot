namespace Data.Core.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class ClientMatchCheckedVideoInfo : BaseVideoInfo
    {
        public int ClientMatchCheckedVideoInfoId { get; set; }

        public int ClientMatchCheckedId { get; set; }

        [ForeignKey(nameof(ClientMatchCheckedId))]
        public ClientMatchChecked ClientMatchChecked { get; set; }
    }
}
