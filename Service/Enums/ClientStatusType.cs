namespace Service.Enums
{
    using System.ComponentModel;

    /// <summary>
    /// Статус клиента
    /// </summary>
    public enum ClientStatusType
    {
        [Description("Активна")]
        Active = 0,
        [Description("Спящий")]
        Sleep = 1,
    }
}
