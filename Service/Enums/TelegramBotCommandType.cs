using System.ComponentModel;

namespace Service.Enums
{
    /// <summary>
    /// Типы у телеграм бота команда
    /// </summary>
    public enum TelegramBotCommandType
    {
        [Description("Системный")]
        Custom = 0,
        [Description("Информационный")]
        Info = 1,
    }
}
