namespace Service.Extensions
{
    using Service.Abstract.TelegramBot;
    using System.Collections.Generic;

    /// <summary>
    /// Расширения бота
    /// </summary>
    public static class BotExtensions
    {
        /// <summary>
        /// Рекурсивно собирает значения уникальных условий
        /// </summary>
        /// <param name="userData">Словарь значений</param>
        /// <param name="condition">Условие со значением</param>
        public static void ProcessCondition(Dictionary<int, ICondition> userData, ICondition condition)
        {
            if (!userData.ContainsKey(condition.GetHashCode()))
            {
                userData[condition.GetHashCode()] = condition;

                if (condition.Conditions != null && condition.Conditions.Count > 0)
                {
                    foreach (var nestedCondition in condition.Conditions)
                    {
                        ProcessCondition(userData, nestedCondition);
                    }
                }
            }
        }
    }
}
