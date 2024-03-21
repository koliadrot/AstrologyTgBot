namespace Service.Abstract.TaskPlanner
{
    using Quartz;

    /// <summary>
    /// Задача для планировщика
    /// </summary>
    public interface ITask
    {
        /// <summary>
        /// Иници-на ли задача
        /// </summary>
        bool IsInit { get; }

        /// <summary>
        /// Ключ задачи
        /// </summary>
        JobKey Key { get; }

        /// <summary>
        /// Иници-я
        /// </summary>
        /// <param name="options"></param>
        void Init(QuartzOptions options);
    }
}
