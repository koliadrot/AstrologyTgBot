namespace Service.Enums
{
    using System.ComponentModel;

    /// <summary>
    /// Типы статусов у телеграм бота
    /// </summary>
    public enum TelegramBotStatusType
    {
        [Description("Включен")]
        Enable = 0,
        [Description("Отключен")]
        Disable = 1,
    }
}
