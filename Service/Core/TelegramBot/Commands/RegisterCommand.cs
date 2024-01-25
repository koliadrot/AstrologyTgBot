namespace Service.Core.TelegramBot.Commands
{
    using NLog;
    using Service.Abstract;
    using Service.Abstract.TelegramBot;
    using Service.Core.TelegramBot.RegisterCondition;
    using Service.Enums;
    using Service.Extensions;
    using Service.ViewModels;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Telegram.Bot.Types;

    /// <summary>
    /// Комманда регистрации
    /// </summary>
    public class RegisterCommand : ICommand, IUpdater, IListener
    {
        public string Name { get; set; } = "/register";

        public string Description { get; set; } = "Регистрация в системе";
        public string ShortDescription { get; set; } = "Регистрация";

        public bool IsAuth { get; set; } = false;
        public bool IsDefault { get; set; } = false;

        private IUpdater _updater;
        public IUpdater CurrentUpdater => _updater;

        public bool IsStartMenu => false;

        private string _telegramId;
        private string _userName;
        private bool _isReferalProgramEnable = default;
        private DataManager _dataManager;
        private Dictionary<string, string> _messages = new Dictionary<string, string>();

        private List<ICondition> _registrationConditions;
        private ClientViewModel _registerViewModel;

        public TelegramBotCommandType CommandType => TelegramBotCommandType.Custom;

        public RegisterCommand(DataManager dataManager)
        {
            _dataManager = dataManager;
            _messages = _dataManager.GetData<CommandExecutor>().Messages;
        }

        public async Task SendStartMessage(Update update)
        {
            if (_messages.TryGetValue(ShortDescription, out string startMessage))
            {
                long chatId = Get.GetChatId(update);
                await _dataManager.GetData<TelegramBotManager>().SendTextMessage(chatId, startMessage);
            }
        }

        public async Task Execute(Update update, string[] arg = null)
        {
            _registerViewModel = new ClientViewModel();
            _updater = null;
            _registrationConditions = null;
            long userId = Get.GetUserId(update);
            long chatId = Get.GetChatId(update);
            _telegramId = userId.ToString();
            _userName = Get.GetUserName(update);

            if (!_dataManager.GetData<ICustomerManager>().ExistTelegram(userId))
            {
                await SendStartMessage(update);

                _registrationConditions = new List<ICondition>();
                _registrationConditions.AddRange(new List<ICondition>()
                {
                    new TermOfUseCondition(_dataManager),
                    new FirstNameCondition(_dataManager),
                    new BirthGenderCondition(_dataManager),
                    new BirthdayCondition(_dataManager),
                    new BirthPlaceCondition(_dataManager,this),
                    new SearchPlaceCondition(_dataManager,this),
                    new SearchGenderCondition(_dataManager),
                    new SearchAgeCondition(_dataManager),
                    new SearchGoalCondition(_dataManager),
                    new AboutMeCondition(_dataManager),
                    new MediaCondition(_dataManager,this)
                });
                var conditionsBase = _dataManager.GetData<ISettingsManager>().GetTelegramBotRegisterConditions();
                foreach (var validCondition in conditionsBase)
                {
                    var condition = _registrationConditions.FirstOrDefault(x => x.GetType().Name == validCondition.ConditionName);
                    if (condition != null)
                    {
                        if (validCondition.IsEnable)
                        {
                            condition.IsCanPass = validCondition.IsCanPass;
                            condition.Order = validCondition.Order;
                        }
                        else if (!validCondition.IsNecessarily)
                        {
                            _registrationConditions.Remove(condition);
                        }
                    }
                }

                _registrationConditions = _registrationConditions.OrderBy(condition => condition.Order).ToList();

                foreach (var condition in _registrationConditions)
                {
                    _dataManager.GetData<CommandExecutor>().StartListen(this);
                    await condition.Execute(update);
                    break;
                }
                _dataManager.GetData<ILogger>().Debug($"User:{userId}; Command:{nameof(RegisterCommand)} - Start registration");
            }
            else
            {
                await _dataManager.GetData<TelegramBotManager>().SendTextMessage(chatId, _messages[MessageKey.ALREADY_REGISTER]);
                _dataManager.GetData<ILogger>().Debug($"User:{userId}; Command:{nameof(RegisterCommand)} - command don`t access. The user is already authorized in Bot");
            }
        }

        public void StartListen(IUpdater updater) => _updater = updater;

        public void StopListen(IUpdater updater)
        {
            if (CurrentUpdater == updater)
            {
                _updater = null;
            }
        }

        public async Task GetUpdate(Update update)
        {
            if (CurrentUpdater == null)
            {
                foreach (ICondition condition in _registrationConditions)
                {
                    if (condition.IsIgnoredNextCondition)
                    {
                        break;
                    }
                    if (!condition.IsDone)
                    {
                        if (condition.IsStarted)
                        {
                            if (await condition.CheckCondition(update))
                            {
                                continue;
                            }
                            else
                            {
                                return;
                            }
                        }
                        else
                        {
                            await condition.Execute(update);
                            return;
                        }
                    }
                }
                await Registration(update);
                await FinalMessage(update);
                _registrationConditions = null;
            }
            else
            {
                await CurrentUpdater.GetUpdate(update);
            }
        }

        private async Task FinalMessage(Update update)
        {
            _dataManager.GetData<CommandExecutor>().StopListen(this);
            string finalMessage = _messages[MessageKey.SUCCESS_REGISTRATION];
            await _dataManager.GetData<CommandExecutor>().ListCommandMessage(update, false, finalMessage);
        }

        private async Task Registration(Update update)
        {
            long userId = Get.GetUserId(update);
            _dataManager.GetData<ILogger>().Debug($"User:{userId}; Command:{nameof(RegisterCommand)} - End conditions. Build user data for register");
            BuildUserData();

            long chatId = Get.GetChatId(update);
            await _dataManager.GetData<TelegramBotManager>().SendTextMessage(chatId, _messages[MessageKey.SEND_DATA_REGISTRATION]);
            _dataManager.GetData<ILogger>().Debug($"User:{userId}; Command:{nameof(RegisterCommand)} - Try register new user");
            _dataManager.GetData<ICustomerManager>().CreateClient(_registerViewModel);
            _dataManager.GetData<ILogger>().Debug($"User:{userId}; Command:{nameof(RegisterCommand)} - User has been register");
        }

        private void BuildUserData()
        {
            var userData = new Dictionary<int, ICondition>();
            foreach (var condition in _registrationConditions)
            {
                ProcessCondition(userData, condition);
            }

            foreach (var condition in userData)
            {
                if (condition.Value is FirstNameCondition)
                {
                    _registerViewModel.FirstName = (string)condition.Value.Info;
                }
                else if (condition.Value is BirthPlaceCondition)
                {
                    if (!((string)condition.Value.Info).IsNull())
                    {
                        string city = ((string)condition.Value.Info).Split('-')[0];
                        string coord = ((string)condition.Value.Info).Split('-')[1];
                        _registerViewModel.BirthCity = city;
                        _registerViewModel.BirthCoord = coord;
                    }
                }
                else if (condition.Value is BirthGenderCondition)
                {
                    _registerViewModel.BirthGender = (string)condition.Value.Info;
                }
                else if (condition.Value is BirthdayCondition)
                {
                    string birthDate = (string)condition.Value.Info;
                    if (!birthDate.IsNull())
                    {
                        string[] formats = { "yyyy-MM-dd HH-mm" };
                        DateTime birthday;
                        if (DateTime.TryParseExact(birthDate, formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out birthday))
                        {
                            _registerViewModel.Birthday = birthday;
                        }
                    }
                }
                else if (condition.Value is SearchPlaceCondition)
                {
                    if (!((string)condition.Value.Info).IsNull())
                    {
                        string city = ((string)condition.Value.Info).Split('-')[0];
                        string coord = ((string)condition.Value.Info).Split('-')[1];
                        _registerViewModel.SearchCity = city;
                        _registerViewModel.SearchCoord = coord;
                    }
                }
                else if (condition.Value is SearchGenderCondition)
                {
                    _registerViewModel.SearchGender = (string)condition.Value.Info;
                }
                else if (condition.Value is SearchAgeCondition)
                {
                    _registerViewModel.SearchAge = (string)condition.Value.Info;
                }
                else if (condition.Value is SearchGoalCondition)
                {
                    _registerViewModel.SearchGoal = (string)condition.Value.Info;
                }
                else if (condition.Value is AboutMeCondition)
                {
                    _registerViewModel.AboutMe = (string)condition.Value.Info;
                }
                else if (condition.Value is MediaCondition)
                {
                    _registerViewModel.ClientMediaInfo = (ClientMediaInfoViewModel)condition.Value.Info;
                }
            }

            _registerViewModel.TelegramId = _telegramId;
            _registerViewModel.UserName = _userName;
        }

        private void ProcessCondition(Dictionary<int, ICondition> userData, ICondition condition)
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
