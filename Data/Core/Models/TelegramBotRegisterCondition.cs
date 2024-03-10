namespace Data.Core.Models
{
    using System.ComponentModel.DataAnnotations;

    public class TelegramBotRegisterCondition
    {
        [Key]
        public int RegisterConditionId { get; set; }
        public int TelegramBotId { get; set; }
        public string Name { get; set; }
        public string ConditionName { get; set; }
        public bool IsCanPass { get; set; }
        public bool IsNecessarily { get; set; }
        public bool IsEnable { get; set; }
        public int Order { get; set; }
        public bool IsInfo { get; set; }
    }
}
