namespace Data.Core.Models
{
    using System.ComponentModel.DataAnnotations;

    public class TelegramBotParamMessage
    {
        [Key]
        public int MessageId { get; set; }
        public int TelegramBotId { get; set; }
        public string MessageName { get; set; }
        public string MessageDescription { get; set; }
        public string MessageValue { get; set; }
        public string MessageValueDefault { get; set; }
        public bool IsButton { get; set; }
        public bool IsSystem { get; set; }
    }
}
