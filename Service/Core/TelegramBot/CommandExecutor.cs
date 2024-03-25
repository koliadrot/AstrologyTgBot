using Newtonsoft.Json;
using NLog;
using Service.Abstract;
using Service.Abstract.TelegramBot;
using Service.Core.TelegramBot.Commands;
using Service.Core.TelegramBot.Notifies;
using Service.Enums;
using Service.Extensions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Service.Core.TelegramBot
{
    /// <summary>
    /// Испольнитель команд формата /start /balance и прочее
    /// </summary>
    public class CommandExecutor : IListener
    {
        public ICommand DefaultCommand { get; private set; }
        public ICommand MenuCommand { get; private set; }

        private List<ICommand> _commands;

        /// <summary>
        /// Список доступных команд
        /// </summary>
        public IReadOnlyList<ICommand> Commands => _commands;

        private List<INotify> _notifies;

        /// <summary>
        /// Список доступных уведомлений
        /// </summary>
        public IReadOnlyList<INotify> Notifies => _notifies;

        private List<ISupportCommandExecutor> _supportCommandExecutors;

        /// <summary>
        /// Список доступных вспомогательных команд
        /// </summary>
        public IReadOnlyList<ISupportCommandExecutor> SupportCommandExecutors => _supportCommandExecutors;


        private List<BotCommand> _botCommands;
        private Dictionary<string, string> _messages = new Dictionary<string, string>();
        public Dictionary<string, string> Messages => _messages;

        private IUpdater _updater;
        public IUpdater CurrentUpdater => _updater;

        public DataManager DataManager { get; private set; }

        private int _currentLevelMenu = 0;
        public int CurrentLevelMenu { get => _currentLevelMenu; set => _currentLevelMenu = Mathf.Clamp(value, 0, Menu.Count); }

        private string _currentMenuName = string.Empty;
        public string CurrentMenuName { get => _currentMenuName; set => _currentMenuName = value; }

        public Dictionary<int, Dictionary<string, List<ICommand>>> Menu = new Dictionary<int, Dictionary<string, List<ICommand>>>();

        public ReplyKeyboardMarkup LastMenuReplyButtons;

        private List<ICommand> _reqiredCommands;
        private bool _isBusy;

        private Update _lastCommand;

        private const string MAIN_MENU_LEVEL = "main";

        public CommandExecutor(DataManager dataManager)
        {
            DataManager = dataManager;
            DataManager.AddData(this);
            _isBusy = false;
        }

        /// <summary>
        /// Иници-я стартера
        /// </summary>
        /// <returns></returns>
        public async Task Init(long userId)
        {
            var telegramBotData = DataManager.GetData<ISettingsManager>().GetTelegramBot();
            bool isUserAuth = DataManager.GetData<ICustomerManager>().ExistTelegram(userId);
            Messages.Clear();
            foreach (var message in telegramBotData.Messages)
            {
                if (!_messages.ContainsKey(message.MessageName))
                {
                    _messages.Add(message.MessageName, message.MessageValue);
                }
            }

            _notifies = new List<INotify>
            {
                new NewLikesNotify(DataManager),
                new MatchNotify(DataManager),
                new OfferShowFindClientsNotify(DataManager),
            };

            MenuCommand = new MenuCommand(DataManager);
            DefaultCommand = isUserAuth ? new FindApplicationCommand(DataManager) : new StartCommand(DataManager);
            _commands = new List<ICommand>
            {
                DefaultCommand,
                MenuCommand,
                new RegisterCommand(DataManager),
                new MyApplicationCommand(DataManager),
                new CheckMatchCommand(DataManager),
                !isUserAuth ? new FindApplicationCommand(DataManager) : new StartCommand(DataManager)
            };

            //var fillUserProfileCommand = new FillUserProfileCommand(DataManager);
            //if (fillUserProfileCommand.HasRequiredConditions(userId))
            //{
            //    _commands.Add(fillUserProfileCommand);
            //}


            string customTypeString = Enum.GetName(typeof(TelegramBotCommandType), TelegramBotCommandType.Custom);
            var customBotCommands = telegramBotData.BotCommands.Where(command => command.CommandType == customTypeString).ToList();
            foreach (var validCommand in customBotCommands)
            {
                var command = _commands.FirstOrDefault(x => x.Name == validCommand.CommandName);
                if (command != null)
                {
                    if (validCommand.IsEnable)
                    {
                        command.Description = validCommand.Description;
                        command.ShortDescription = validCommand.Name;
                        command.IsAuth = validCommand.IsAuth;
                        command.IsDefault = validCommand.IsDefault;
                    }
                    else if (validCommand.IsPublic)
                    {
                        _commands.Remove(command);
                    }
                }
            }

            string infoType = Enum.GetName(typeof(TelegramBotCommandType), TelegramBotCommandType.Info);
            var infoBotCommands = telegramBotData.BotCommands.Where(command => command.CommandType == infoType).ToList();
            foreach (var validCommand in infoBotCommands)
            {
                if (validCommand.IsEnable)
                {
                    var newLinkCommad = new InfoCommand(DataManager, validCommand.AdditionalData)
                    {
                        Name = validCommand.CommandName,
                        Description = validCommand.Description,
                        ShortDescription = validCommand.Name,
                        IsAuth = validCommand.IsAuth,
                        IsDefault = validCommand.IsDefault,
                    };
                    _commands.Add(newLinkCommad);
                }
            }

            var commandsToRemove = _commands.Where(command => !command.IsDefault && command.IsAuth != isUserAuth).ToList();
            commandsToRemove.ForEach(command => _commands.Remove(command));

            if (_botCommands == null)
            {
                _botCommands = new List<BotCommand>();
                _commands.Where(command => command.IsStartMenu).ToList().ForEach(command => _botCommands.Add(new BotCommand() { Command = command.Name, Description = command.Description }));
                await DataManager.GetData<TelegramBotManager>().SetNewCommands(_botCommands);
            }
            _updater = null;
            _isBusy = false;
            string menuJson = DataManager.GetData<ISettingsManager>().GetTelegramBot().Menu;
            CreateMenu(menuJson, isUserAuth);
            var quitCommand = new QuitCommand(DataManager);
            _commands.Add(quitCommand);

            var getClientApplicationCommand = new GetClientApplicationCommand(DataManager);
            _commands.Add(getClientApplicationCommand);

            //NOTE:Временное решение добавление кнопки "Назад" внутри комманд, до появления полного функционала
            _reqiredCommands = new List<ICommand>
            {
                quitCommand,
                DefaultCommand,
                MenuCommand
            };
            RegisterCommand registerCommand = Commands.FirstOrDefault(x => x is RegisterCommand) as RegisterCommand;
            if (registerCommand != null)
            {
                _reqiredCommands.Add(registerCommand);
            }
            //fillUserProfileCommand = Commands.FirstOrDefault(x => x is FillUserProfileCommand) as FillUserProfileCommand;
            //if (fillUserProfileCommand != null)
            //{
            //    _reqiredCommands.Add(fillUserProfileCommand);
            //}

            _supportCommandExecutors = new List<ISupportCommandExecutor>();
            _supportCommandExecutors.AddRange(Commands.OfType<ISupportCommandExecutor>());
        }

        public void StartListen(IUpdater updater) => _updater = updater;

        public void StopListen(IUpdater updater)
        {
            if (CurrentUpdater == updater)
            {
                _updater = null;
            }
        }

        public async Task GetUpdate(Update update, bool isForce = false)
        {
            string messageText = string.Empty;
            long userId = Get.GetUserId(update);
            _lastCommand = update;
            try
            {
                if (!_isBusy || isForce)
                {
                    _isBusy = true;
                    messageText = Get.GetText(update);

                    if (_notifies != null && _notifies.Any(x => x.IsInteraction(update)))
                    {
                        foreach (var notify in _notifies)
                        {
                            if (notify.IsInteraction(update))
                            {
                                await notify.InlineAction(update);
                                break;
                            }
                        }
                    }
                    else if (CurrentUpdater != null)
                    {
                        if (_reqiredCommands.Contains(messageText))
                        {
                            await ExecuteCommand(update, _reqiredCommands);
                        }
                        else
                        {
                            await CurrentUpdater.GetUpdate(update);
                        }
                    }
                    else
                    {
                        Menu[CurrentLevelMenu].TryGetValue(CurrentMenuName, out List<ICommand> listCommands);
                        await ExecuteCommand(update, listCommands);
                    }
                    _isBusy = false;
                }
                else
                {
                    await WaitMessage(update);
                }
            }
            catch (Exception ex)
            {
                await ErrorMessage(update);
                DataManager.GetData<ILogger>().Error($"User:{userId} - Failed update message {messageText}! {ex}");
            }
        }

        /// <summary>
        /// Переобновление
        /// </summary>
        /// <returns></returns>
        public async Task ReUpdate() => await GetUpdate(_lastCommand, true);

        private async Task ExecuteCommand(Update update, List<ICommand> listCommands = null)
        {
            if (listCommands == null || !listCommands.Any())
            {
                listCommands = _commands;
            }
            List<string> messageTexts = Get.GetText(update).Split(' ').ToList();
            if (messageTexts[0] == DefaultCommand.Name && messageTexts.Count > 1)
            {
                messageTexts.RemoveAt(0);
                await DefaultCommand.Execute(update, messageTexts.ToArray());
                return;
            }
            string messageText = Get.GetText(update);
            //NOTE:если вводим в сообщения одну из главных команд, то пока что будем затирать всю историю до появления диспетчера
            _updater = null;
            if (!messageTexts[0].IsNull())
            {
                foreach (var command in listCommands)
                {
                    if (command.IsValidCommand(messageText))
                    {
                        await command.Execute(update);
                        return;
                    }
                }
                foreach (var command in _commands.Where(x => !(x is TransitionCommand)))
                {
                    if (command.IsValidCommand(messageText))
                    {
                        await command.Execute(update);
                        return;
                    }
                }
                await DefaultCommandMessage(update);
            }
        }

        /// <summary>
        /// Сообщение с списком доступных комманд
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        public async Task ListCommandMessage(Update update, bool isWrongMessage = true, string additionalMessageText = "")
        {
            LastMenuReplyButtons = null;
            long chatId = Get.GetChatId(update);
            long userId = Get.GetUserId(update);
            await Init(userId);
            string fullMessage = string.Empty;
            if (isWrongMessage)
            {
                string userMessage = Get.GetText(update);
                fullMessage = Get.ReplaceKeysInText(_messages[MessageKey.UNKNOW_COMMAND], new Dictionary<string, string>() { { Promt.MESSAGE, userMessage } });
            }
            if (!additionalMessageText.IsNull())
            {
                fullMessage += $"{additionalMessageText}";
            }
            ReplyKeyboardMarkup replyKeyboard = null;
            CurrentLevelMenu = 0;
            CurrentMenuName = MAIN_MENU_LEVEL;
            if (Menu.Any() && Menu[CurrentLevelMenu].TryGetValue(MAIN_MENU_LEVEL, out List<ICommand> mainLevelCommands))
            {
                if (mainLevelCommands.Count > 2)
                {
                    var inlineKeyboardButtons = new List<List<KeyboardButton>>();
                    foreach (var miniCommand in mainLevelCommands)
                    {
                        var row = new List<KeyboardButton>();
                        var infoCommand = miniCommand as InfoCommand;
                        if (infoCommand != null && infoCommand.Info.IsValidLink())
                        {
                            var url = new WebAppInfo() { Url = infoCommand.Info };
                            row.Add(KeyboardButton.WithWebApp(infoCommand.ShortDescription, url));
                        }
                        else
                        {
                            row.Add(new KeyboardButton(miniCommand.ShortDescription));
                        }
                        inlineKeyboardButtons.Add(row);
                    }

                    replyKeyboard = new ReplyKeyboardMarkup(inlineKeyboardButtons);
                }
                else
                {
                    List<KeyboardButton> inlineKeyboardButtons = new List<KeyboardButton>();
                    replyKeyboard = new ReplyKeyboardMarkup(new[]
                    {
                        inlineKeyboardButtons
                    });

                    foreach (var command in mainLevelCommands)
                    {
                        var infoCommand = command as InfoCommand;
                        if (infoCommand != null && infoCommand.Info.IsValidLink())
                        {
                            var url = new WebAppInfo() { Url = infoCommand.Info };
                            inlineKeyboardButtons.Add(KeyboardButton.WithWebApp(infoCommand.ShortDescription, url));
                        }
                        else
                        {
                            inlineKeyboardButtons.Add(new KeyboardButton(command.ShortDescription));
                        }
                    }
                }
            }

            if (!DataManager.GetData<ICustomerManager>().ExistTelegram(userId))
            {
                fullMessage += _messages[MessageKey.NEED_AUTH];
            }
            if (replyKeyboard != null)
            {
                replyKeyboard.ResizeKeyboard = true;
                replyKeyboard.IsPersistent = false;
            }
            await DataManager.GetData<TelegramBotManager>().SendTextMessage(chatId, fullMessage, replyMarkup: replyKeyboard);
        }

        /// <summary>
        /// Отправляет текущие команды последнего меню
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        public async Task SendCurrentListCommand(Update update)
        {
            _updater = null;
            long chatId = Get.GetChatId(update);
            if (LastMenuReplyButtons != null)
            {
                await DataManager.GetData<TelegramBotManager>().SendTextMessage(chatId, _messages[MessageKey.TRANSITION_HELP], replyMarkup: LastMenuReplyButtons);
            }
            else
            {
                await ListCommandMessage(update, false, _messages[MessageKey.TRANSITION_HELP]);
            }
        }

        /// <summary>
        /// Есть ли команда
        /// </summary>
        /// <param name="messageText"></param>
        /// <param name="isIgnoreLevvel"></param>
        /// <returns></returns>
        public bool HasCommand(string messageText, bool isIgnoreLevvel = true)
        {
            if (isIgnoreLevvel)
            {
                return Commands.Contains(messageText);
            }
            else
            {
                Menu[CurrentLevelMenu].TryGetValue(CurrentMenuName, out List<ICommand> listCommands);
                return listCommands.Contains(messageText);
            }
        }

        private async Task WaitMessage(Update update)
        {
            long chatId = Get.GetChatId(update);
            string messsage = _messages[MessageKey.WAIT];
            await DataManager.GetData<TelegramBotManager>().SendTextMessage(chatId, messsage);
        }

        private async Task ErrorMessage(Update update)
        {
            long chatId = Get.GetChatId(update);
            string messsage = _messages[MessageKey.ERROR];
            await DataManager.GetData<TelegramBotManager>().SendTextMessage(chatId, messsage);
            _isBusy = false;
            _updater = null;
        }

        /// <summary>
        /// Комманда по-умолчанию
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        public async Task DefaultCommandMessage(Update update) => await DefaultCommand.Execute(update);

        /// <summary>
        /// Комманда Меню
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        public async Task MenuCommandMessage(Update update) => await MenuCommand.Execute(update);

        /// <summary>
        /// Получает команду по техническому имени
        /// </summary>
        /// <param name="commandName"></param>
        /// <returns></returns>
        public ICommand? GetCommandByCommandName(string commandName) => Commands.FirstOrDefault(x => x.Name == commandName);

        /// <summary>
        /// Получает команду по типу
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ICommand? GetCommandByType(Type type) => Commands.FirstOrDefault(x => type.IsAssignableFrom(x.GetType()));

        /// <summary>
        /// Получает уведомление по техническому имени
        /// </summary>
        /// <param name="commandName"></param>
        /// <returns></returns>
        public INotify? GetNotifyByName(string notifyName) => Notifies.FirstOrDefault(x => x.Name == notifyName);

        /// <summary>
        /// Получает уведомление по типу
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public INotify? GetNotifyByType(Type type) => Notifies.FirstOrDefault(x => type.IsAssignableFrom(x.GetType()));

        /// <summary>
        /// Получает вспомошательную команду по техническому имени
        /// </summary>
        /// <param name="commandName"></param>
        /// <returns></returns>
        public ISupportCommandExecutor? GetSupportCommandExecutorByName(string key) => SupportCommandExecutors.FirstOrDefault(x => x.Key == key);

        /// <summary>
        /// Получает вспомошательную команду по типу
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ISupportCommandExecutor? GetSupportCommandExecutorByType(Type type) => SupportCommandExecutors.FirstOrDefault(x => type.IsAssignableFrom(x.GetType()));

        private void CreateMenu(string menuJson, bool isAuth)
        {
            Menu.Clear();
            MenuList menuList = JsonConvert.DeserializeObject<MenuList>(menuJson);
            Menu menu = isAuth ? menuList.AuthMenu : menuList.StartMenu;
            PrintMenu(menu.Items, parentName: MAIN_MENU_LEVEL);
            foreach (var levelCommands in Menu.Values)
            {
                foreach (var levelCommand in levelCommands.Values)
                {
                    if (levelCommand.Any(x => x.Name.EndsWith("quit", StringComparison.OrdinalIgnoreCase)))
                    {
                        levelCommand.Sort((cmd1, cmd2) =>
                        {
                            if (cmd1.Name.EndsWith("quit", StringComparison.OrdinalIgnoreCase))
                            {
                                return 1;
                            }
                            else if (cmd2.Name.EndsWith("quit", StringComparison.OrdinalIgnoreCase))
                            {
                                return -1;
                            }
                            else
                            {
                                return string.Compare(cmd1.Name, cmd2.Name, StringComparison.OrdinalIgnoreCase);
                            }
                        });
                    }
                }

            }
        }

        private void PrintMenu(List<MenuItem> menuItems, string parentName, int level = 0)
        {
            foreach (var item in menuItems)
            {
                string commandName = item.CommandName;

                if (commandName.EndsWith($"-{level}"))
                {
                    int index = commandName.LastIndexOf($"-{level}");
                    if (index >= 0)
                    {
                        commandName = commandName.Substring(0, index);
                    }
                }
                ICommand command = GetCommandByCommandName(commandName);

                if (command != null)
                {
                    command.ShortDescription = item.Name;
                    if (!Menu.ContainsKey(level))
                    {
                        List<ICommand> commands = new List<ICommand>
                        {
                            command
                        };
                        Dictionary<string, List<ICommand>> keyValuePairs = new Dictionary<string, List<ICommand>>()
                        {
                            {parentName, commands},
                        };
                        Menu.Add(level, keyValuePairs);
                    }
                    else
                    {
                        var keyValuePairs = Menu[level];
                        if (!keyValuePairs.ContainsKey(parentName))
                        {
                            keyValuePairs.Add(parentName, new List<ICommand>() { command });
                        }
                        else
                        {
                            keyValuePairs[parentName].Add(command);
                        }
                    }
                }
                if (item.SubMenu != null && item.SubMenu.Items != null)
                {
                    var transitionEnterCommand = new TransitionCommand(DataManager)
                    {
                        Name = item.CommandName,
                        ShortDescription = item.Name,
                    };
                    var transitionQuitCommand = new TransitionCommand(DataManager)
                    {
                        Name = item.CommandName + "quit",
                        ShortDescription = _messages[ReplyButton.BACK],
                        IsForwardDirection = false,
                        ParentShortDescription = parentName
                    };


                    if (!Menu.ContainsKey(level))
                    {
                        List<ICommand> commands = new List<ICommand>
                        {
                            transitionEnterCommand,
                        };
                        Dictionary<string, List<ICommand>> keyValuePairs = new Dictionary<string, List<ICommand>>()
                        {
                            {parentName, commands},
                        };
                        Menu.Add(level, keyValuePairs);
                    }
                    else
                    {
                        var keyValuePairs = Menu[level];
                        if (!keyValuePairs.ContainsKey(parentName))
                        {
                            keyValuePairs.Add(parentName, new List<ICommand>() { transitionEnterCommand });
                        }
                        else
                        {
                            keyValuePairs[parentName].Add(transitionEnterCommand);
                        }
                    }

                    if (!Menu.ContainsKey(level + 1))
                    {
                        List<ICommand> commands = new List<ICommand>
                        {
                            transitionQuitCommand,
                        };
                        Dictionary<string, List<ICommand>> keyValuePairs = new Dictionary<string, List<ICommand>>()
                        {
                            {transitionEnterCommand.ShortDescription, commands},
                        };
                        Menu.Add(level + 1, keyValuePairs);
                    }
                    else
                    {
                        var keyValuePairs = Menu[level + 1];
                        if (!keyValuePairs.ContainsKey(transitionEnterCommand.ShortDescription))
                        {
                            keyValuePairs.Add(transitionEnterCommand.ShortDescription, new List<ICommand>() { transitionQuitCommand });
                        }
                        else
                        {
                            keyValuePairs[transitionEnterCommand.ShortDescription].Add(transitionQuitCommand);
                        }
                    }

                    _commands.Add(transitionEnterCommand);
                    _commands.Add(transitionQuitCommand);

                    PrintMenu(item.SubMenu.Items, transitionEnterCommand.ShortDescription, level + 1);
                }
            }
        }
    }
}