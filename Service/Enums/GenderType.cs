namespace Service.Enums
{
    using System.ComponentModel;

    /// <summary>
    /// Типы гендеров
    /// </summary>
    public enum GenderType
    {
        [Description("Мужской")]
        Man = 0,
        [Description("Женский")]
        Woman = 1,
        [Description("Не определен")]
        NoGender = 2,
    }
}
