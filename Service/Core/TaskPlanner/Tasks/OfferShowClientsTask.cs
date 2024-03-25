namespace Service.Core.TaskPlanner.Tasks
{
    using Quartz;
    using Service.Abstract;
    using Service.Abstract.Communication;
    using Service.Abstract.Filtrable;
    using Service.Abstract.TaskPlanner;
    using Service.Core.TelegramBot;
    using Service.Core.TelegramBot.Notifies;
    using Service.Extensions;
    using Service.ViewModels.Communication;

    /// <summary>
    /// Предложение посмотреть анкеты
    /// </summary>
    public class OfferShowClientsTask : ITask
    {
        private int DELAY_TASK = 24;

        private JobKey? _jobKey = null;
        public JobKey Key
        {
            get
            {
                if (_jobKey == null)
                {
                    _jobKey = JobKey.Create(nameof(OfferShowClientsTask));
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
                .AddJob<OfferShowClientsJob>(jobBuiler => jobBuiler.WithIdentity(Key).Build())
                .AddTrigger(trigger =>
                {
                    trigger.ForJob(Key).WithSimpleSchedule(schedule => schedule.WithIntervalInHours(DELAY_TASK).RepeatForever()).StartAt(DateBuilder.DateOf(0, 0, 0)).Build();
                });
            }
        }

    }

    [DisallowConcurrentExecution]
    public class OfferShowClientsJob : IJob
    {
        private readonly ICustomerManager _customerManager;
        private readonly ICommunicationManager _communicationManager;

        public OfferShowClientsJob(ICustomerManager customerManager, ICommunicationManager communicationManager)
        {
            _customerManager = customerManager;
            _communicationManager = communicationManager;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var clients = _customerManager.GetClients();
            SendCommunicationInfo sendCommunicationInfo = new SendCommunicationInfo()
            {
                Message = nameof(OfferShowFindClientsNotify),
                AdditionalParams = new Dictionary<string, string>()
                {
                    {ICommunication.TYPE_MESSAGE_KEY,GlobalTelegramSettings.OFFER_SHOW_FIND_CLIENTS_NOTIFY }
                }
            };
            foreach (var client in clients)
            {
                var myClient = client;
                var findClients = clients.Where(x => x.ClientId != myClient.ClientId);
                List<IClientFitrable> _clientFiters = _customerManager.GetFindClientFilters(myClient);
                findClients = findClients.Filter(_clientFiters).ToList();

                if (findClients.Any())
                {
                    await _communicationManager.GetCurrentCommunication().SendMessage(client, sendCommunicationInfo, string.Empty, string.Empty);
                }
            }
        }
    }
}
