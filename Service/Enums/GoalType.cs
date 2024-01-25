namespace Service.Enums
{
    using System.ComponentModel;

    /// <summary>
    /// Типы поиска
    /// </summary>
    public enum GoalType
    {
        [Description("Общение")]
        Communication = 0,
        [Description("Отношения")]
        Relationship = 1,
        [Description("Любая")]
        AnyWay = 2,
    }
}
