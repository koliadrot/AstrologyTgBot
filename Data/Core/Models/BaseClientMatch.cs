namespace Data.Core.Models
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public class BaseClientMatch
    {
        public int ClientMatchInfoId { get; set; }
        [ForeignKey(nameof(ClientMatchInfoId))]
        public ClientMatchInfo ClientMatchInfo { get; set; }

        public DateTime? DateMatch { get; set; }

        public string? MatchType { get; set; }

        public DateTime? AnswearDateMatch { get; set; }

        public string? AnswearMatchType { get; set; }

        public string? MatchTelegramId { get; set; }

        public string? LoveLetterText { get; set; }
    }
}
