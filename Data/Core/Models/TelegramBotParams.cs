namespace Data.Core.Models
{
    using System.ComponentModel.DataAnnotations;

    public class TelegramBotParams
    {
        [Key]
        public int TelegramBotId { get; set; }
        public string BotName { get; set; }
        public string BotUserName { get; set; }
        public string TokenApi { get; set; }
        public string WebHookUrl { get; set; }
        public string TosUrl { get; set; }
        public bool AcceptPromotionsBySms { get; set; }
        public bool AcceptElectronicReceipts { get; set; }
        public string HelloText { get; set; }
        public string Menu { get; set; }
        public virtual ICollection<TelegramBotCommand> BotCommands { get; set; } = new List<TelegramBotCommand>();
    }
}
