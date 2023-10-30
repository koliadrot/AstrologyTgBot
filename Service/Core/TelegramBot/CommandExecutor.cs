using Newtonsoft.Json;
using Service.Abstract;
using Service.Abstract.TelegramBot;
using Service.Core.TelegramBot.Commands;
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

        private List<ICommand> _commands;

        /// <summary>
        /// Список доступных команд
        /// </summary>
        public IReadOnlyList<ICommand> Commands => _commands;

        private List<BotCommand> _botCommands;
        private Dictionary<string, string> _messages = new Dictionary<string, string>();
        public Dictionary<string, string> Messages => _messages;

        private IUpdater _updater;
        public IUpdater CurrentUpdater => _updater;

        public DataManager DataManager { get; private set; }

        private int _currentLevelMenu = 0;
        public int CurrentLevelMenu { get => _currentLevelMenu; set => _currentLevelMenu = Mathf.Clamp(value, 0, Menu.Count); }

        public Dictionary<int, List<ICommand>> Menu = new Dictionary<int, List<ICommand>>();

        private bool _isBusy;

        public CommandExecutor(DataManager dataManager)
        {
            DataManager = dataManager;
            DataManager.AddData(this);
            _isBusy = false;
        }

        /// <summary>
        /// Иници-я комманд
        /// </summary>
        /// <returns></returns>
        public async Task InitCommands(long userId)
        {
            var telegramBotData = DataManager.GetData<ISettingsManager>().GetTelegramBot();
            Messages.Clear();
            foreach (var message in telegramBotData.Messages)
            {
                if (!_messages.ContainsKey(message.MessageName))
                {
                    _messages.Add(message.MessageName, message.MessageValue);
                }
            }
            DefaultCommand = DefaultCommand ?? new StartCommand(DataManager);
            _commands = new List<ICommand>
            {
                DefaultCommand,
                new MenuCommand(DataManager)
            };

            bool isUserAuth = DataManager.GetData<ICustomerManager>().ExistTelegram(userId);

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
            //_commands.Add(new QuitCommand(DataManager));
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
            try
            {
                if (!_isBusy)
                {
                    _isBusy = true;
                    string messageText = Get.GetText(update);

                    if (CurrentUpdater == null || Commands.Contains(messageText))
                    {
                        await ExecuteCommand(update);
                    }
                    else
                    {
                        await CurrentUpdater.GetUpdate(update);
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
            }
        }

        private async Task ExecuteCommand(Update update)
        {
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
                foreach (var command in _commands)
                {
                    if (command.IsValidCommand(messageText))
                    {
                        await command.Execute(update);
                        return;
                    }
                }
                await ListCommandMessage(update);
            }
        }

        /// <summary>
        /// Сообщение с списком доступных комманд
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        public async Task ListCommandMessage(Update update, bool isWrongMessage = true, string additionalMessageText = "")
        {
            long chatId = Get.GetChatId(update);
            long userId = Get.GetUserId(update);
            await InitCommands(userId);
            string fullMessage = string.Empty;
            if (isWrongMessage)
            {
                string userMessage = Get.GetText(update);
                fullMessage = Get.ReplaceKeysInText(_messages[MessageKey.UNKNOW_COMMAND], new Dictionary<string, string>() { { Promt.MESSAGE, userMessage } });
            }
            if (!additionalMessageText.IsNull())
            {
                fullMessage += $"{additionalMessageText}\n";
            }
            ReplyKeyboardMarkup replyKeyboard = null;
            CurrentLevelMenu = 0;
            if (Menu.Count > 0)
            {
                if (Menu[CurrentLevelMenu].Count > 2)
                {
                    var inlineKeyboardButtons = new List<List<KeyboardButton>>();

                    foreach (var miniCommand in Menu[CurrentLevelMenu])
                    {
                        var row = new List<KeyboardButton> { new KeyboardButton(miniCommand.ShortDescription) };
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

                    foreach (var command in Menu[CurrentLevelMenu])
                    {
                        inlineKeyboardButtons.Add(new KeyboardButton(command.ShortDescription));
                    }
                }
                replyKeyboard.ResizeKeyboard = true;
                replyKeyboard.IsPersistent = true;
            }

            if (!DataManager.GetData<ICustomerManager>().ExistTelegram(userId))
            {
                fullMessage += _messages[MessageKey.NEED_AUTH];
            }

            await DataManager.GetData<TelegramBotManager>().SendTextMessage(chatId, fullMessage, replyMarkup: replyKeyboard);
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

        private ICommand GetCommand(string commandName) => Commands.FirstOrDefault(x => x.Name == commandName);


        private void CreateMenu(string menuJson, bool isAuth)
        {
            Menu.Clear();
            MenuList menuList = JsonConvert.DeserializeObject<MenuList>(menuJson);
            Menu menu = isAuth ? menuList.AuthMenu : menuList.StartMenu;
            PrintMenu(menu.Items);
            foreach (var levelCommands in Menu.Values)
            {
                levelCommands.Sort((cmd1, cmd2) =>
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

        private void PrintMenu(List<MenuItem> menuItems, int level = 0)
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
                ICommand command = GetCommand(commandName);

                if (command != null)
                {
                    command.ShortDescription = item.Name;
                    if (!Menu.ContainsKey(level))
                    {
                        List<ICommand> commands = new List<ICommand>
                        {
                            command
                        };
                        Menu.Add(level, commands);
                    }
                    else
                    {
                        Menu[level].Add(command);
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
                        IsForwardDirection = false
                    };


                    if (!Menu.ContainsKey(level))
                    {
                        List<ICommand> commands = new List<ICommand>
                        {
                            transitionEnterCommand,
                        };
                        Menu.Add(level, commands);
                    }
                    else
                    {
                        Menu[level].Add(transitionEnterCommand);
                    }

                    if (!Menu.ContainsKey(level + 1))
                    {
                        List<ICommand> commands = new List<ICommand>
                        {
                            transitionQuitCommand,
                        };
                        Menu.Add(level + 1, commands);
                    }
                    else
                    {
                        Menu[level + 1].Add(transitionQuitCommand);
                    }

                    _commands.Add(transitionEnterCommand);
                    _commands.Add(transitionQuitCommand);

                    PrintMenu(item.SubMenu.Items, level + 1);
                }
            }
        }
    }
}