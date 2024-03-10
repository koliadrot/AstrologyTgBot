namespace Service.Enums
{
    using System.ComponentModel;

    /// <summary>
    /// Типы совпадений в поисках пар
    /// </summary>
    public enum MatchType
    {
        [Description("Нравится")]
        Like = 0,
        [Description("Любовное письмо")]
        LoveLetter = 1,
        [Description("Не нравится")]
        Dislike = 2,
    }
}
