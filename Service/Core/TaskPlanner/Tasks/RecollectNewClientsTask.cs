namespace Service.Core.TaskPlanner.Tasks
{
    using Quartz;
    using Service.Abstract;
    using Service.Abstract.TaskPlanner;
    using Service.Core.TelegramBot.Commands;
    using Service.ViewModels.Communication;
    using System.Threading.Tasks;


    /// <summary>
    /// Обновление списка клиентов для просмотра анкет
    /// </summary>
    public class RecollectNewClientsTask : ITask
    {
        private int DELAY_TASK = 15;
        private int START_DELAY = 10;

        private JobKey? _jobKey = null;
        public JobKey Key
        {
            get
            {
                if (_jobKey == null)
                {
                    _jobKey = JobKey.Create(nameof(RecollectNewClientsTask));
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
                .AddJob<RecollectNewClientsJob>(jobBuiler => jobBuiler.WithIdentity(Key).Build())
                .AddTrigger(trigger =>
                {
                    trigger.ForJob(Key).WithSimpleSchedule(schedule => schedule.WithIntervalInMinutes(DELAY_TASK).RepeatForever()).StartAt(DateBuilder.FutureDate(START_DELAY, IntervalUnit.Second)).Build();
                });
            }
        }

    }

    [DisallowConcurrentExecution]
    public class RecollectNewClientsJob : IJob
    {
        private readonly ICommunicationManager _communicationManager;
        private readonly ICustomerManager _customerManager;

        public RecollectNewClientsJob(ICustomerManager customerManager, ICommunicationManager communicationManager)
        {
            _communicationManager = communicationManager;
            _customerManager = customerManager;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            //NOTE:тут клиент не так важен, просто требуется модель клиента
            var client = _customerManager.GetClients().FirstOrDefault();
            await _communicationManager.GetCurrentCommunication().SendMessage(client, new SendCommunicationInfo() { Message = nameof(FindApplicationCommand) }, string.Empty, string.Empty);
        }
    }
}
