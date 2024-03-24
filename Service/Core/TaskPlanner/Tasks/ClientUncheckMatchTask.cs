namespace Service.Core.TaskPlanner.Tasks
{
    using Quartz;
    using Service.Abstract;
    using Service.Abstract.TaskPlanner;
    using Service.Core.TelegramBot.Notifies;
    using Service.ViewModels.Communication;
    using System.Threading.Tasks;

    /// <summary>
    /// Задача на рассылку сообщения что есть лайки не просмотренные
    /// </summary>
    public class ClientUncheckMatchTask : ITask
    {
        private int DELAY_TASK = 30;

        private JobKey? _jobKey = null;
        public JobKey Key
        {
            get
            {
                if (_jobKey == null)
                {
                    _jobKey = JobKey.Create(nameof(ClientUncheckMatchTask));
                }
                return _jobKey;
            }
        }

        public bool IsInit { get; private set; } = false;

        public void Init(QuartzOptions options)
        {
            if (!IsInit)
            {
                options
                .AddJob<ClientUncheckMatchJob>(jobBuiler => jobBuiler.WithIdentity(Key))
                .AddTrigger(trigger =>
                {
                    trigger.ForJob(Key).WithSimpleSchedule(schedule => schedule.WithIntervalInMinutes(DELAY_TASK).RepeatForever());
                });
            }
        }

    }

    [DisallowConcurrentExecution]
    public class ClientUncheckMatchJob : IJob
    {
        private readonly ICustomerManager _customerManager;
        private readonly ICommunicationManager _communicationManager;

        public ClientUncheckMatchJob(ICustomerManager customerManager, ICommunicationManager communicationManager)
        {
            _customerManager = customerManager;
            _communicationManager = communicationManager;
        }

        public Task Execute(IJobExecutionContext context)
        {
            var clients = _customerManager.GetClients();
            foreach (var client in clients)
            {
                _communicationManager.GetCurrentCommunication().SendMessage(client, new SendCommunicationInfo() { Message = nameof(NewLikesNotify) }, string.Empty, string.Empty);
            }
            return Task.CompletedTask;
        }
    }
}
