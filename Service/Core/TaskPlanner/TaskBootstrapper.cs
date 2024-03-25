namespace Service.Core.TaskPlanner
{
    using Microsoft.Extensions.Options;
    using Quartz;
    using Service.Abstract.TaskPlanner;
    using Service.Core.TaskPlanner.Tasks;

    /// <summary>
    /// Загрузчик задач
    /// </summary>
    public class TaskBootstrapper : IConfigureOptions<QuartzOptions>
    {
        private List<ITask> _tasks = null;

        private List<ITask> GetTasks()
        {
            if (_tasks == null)
            {
                _tasks = new List<ITask>()
                {
                    new ClientUncheckMatchTask(),
                    new OfferShowClientsTask(),
                    new RecollectNewClientsTask()
                };
            }
            return _tasks;
        }

        public void Configure(QuartzOptions options)
        {
            foreach (var task in GetTasks())
            {
                task.Init(options);
            }
        }
    }
}
