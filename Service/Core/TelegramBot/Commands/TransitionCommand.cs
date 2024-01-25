using Service.Abstract.TelegramBot;
using Service.Enums;
using Service.Extensions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Service.Core.TelegramBot.Commands
{
    public class TransitionCommand : ICommand
    {
        public string Name { get; set; } = "/transition";

        public string Description { get; set; } = "Переход в уровень меню";

        public string ShortDescription { get; set; } = "Переход";

        /// <summary>
        /// Имя прямого перехода
        /// </summary>
        public string ParentShortDescription { set; get; }

        public bool IsAuth { get; set; } = false;
        public bool IsDefault { get; set; } = true;

        public TelegramBotCommandType CommandType => TelegramBotCommandType.Custom;
        public bool IsStartMenu => false;

        public bool IsForwardDirection { get; set; } = true;

        private readonly DataManager _dataManager;
        private readonly Dictionary<string, string> _messages;

        public TransitionCommand(DataManager dataManager)
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
            long chatId = Get.GetChatId(update);
            ReplyKeyboardMarkup replyKeyboard = null;
            string levelKey = string.Empty;
            if (IsForwardDirection)
            {
                _dataManager.GetData<CommandExecutor>().CurrentLevelMenu++;
                _dataManager.GetData<CommandExecutor>().CurrentMenuName = ShortDescription;
                levelKey = ShortDescription;
            }
            else
            {
                _dataManager.GetData<CommandExecutor>().CurrentLevelMenu--;
                levelKey = ParentShortDescription;
                _dataManager.GetData<CommandExecutor>().CurrentMenuName = ParentShortDescription;
            }
            int level = _dataManager.GetData<CommandExecutor>().CurrentLevelMenu;
            var menu = _dataManager.GetData<CommandExecutor>().Menu;

            if (menu.Any() && menu[level].TryGetValue(levelKey, out List<ICommand> levelCommands))
            {
                if (levelCommands.Count > 2)
                {
                    var inlineKeyboardButtons = new List<List<KeyboardButton>>();

                    foreach (var miniCommand in levelCommands)
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

                    foreach (var command in levelCommands)
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

            if (replyKeyboard != null)
            {
                replyKeyboard.ResizeKeyboard = true;
                replyKeyboard.IsPersistent = false;
            }
            string messageText = _messages.TryGetValue(ShortDescription, out string startMessage) ? startMessage : _messages[MessageKey.TRANSITION_HELP];
            await _dataManager.GetData<TelegramBotManager>().SendTextMessage(chatId, messageText, replyMarkup: replyKeyboard);
            _dataManager.GetData<CommandExecutor>().LastMenuReplyButtons = replyKeyboard;
        }
    }
}
