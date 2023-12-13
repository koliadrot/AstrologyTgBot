namespace Data.Core.Models
{
    using System.ComponentModel.DataAnnotations;

    public class TelegramBotCommand
    {
        [Key]
        public int BotCommandId { get; set; }
        public int TelegramBotId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? CommandName { get; set; }
        public string? CommandType { get; set; }
        public bool IsAuth { get; set; }
        public bool IsDefault { get; set; }
        public bool IsEnable { get; set; }
        public bool IsPublic { get; set; }
        public string? AdditionalData { get; set; }
    }
}
