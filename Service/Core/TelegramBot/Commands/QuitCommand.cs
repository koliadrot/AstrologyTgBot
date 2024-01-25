namespace Service.Core.TelegramBot.Commands
{
    using NLog;
    using Service.Abstract;
    using Service.Abstract.TelegramBot;
    using Service.Enums;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Telegram.Bot.Types;
    using Telegram.Bot.Types.ReplyMarkups;

    /// <summary>
    /// Команда выхода из системы
    /// </summary>
    public class QuitCommand : ICommand, IUpdater
    {
        public string Name { get; set; } = "/quitnotsecret26";

        public string Description { get; set; } = "Выход из системы";

        public string ShortDescription { get; set; } = "Выход не секретный 26";

        public bool IsAuth { get; set; } = true;
        public bool IsDefault { get; set; } = false;

        public TelegramBotCommandType CommandType => TelegramBotCommandType.Custom;
        public bool IsStartMenu => false;

        private string _telegramId = string.Empty;
        private DataManager _dataManager;
        private readonly Dictionary<string, string> _messages;

        private List<string> _supportMiniComands = new List<string>
        {
            YES,
            NO
        };
        private const string YES = "Да";
        private const string NO = "Нет";

        public QuitCommand(DataManager dataManager)
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
            long userId = Get.GetUserId(update);
            long chatId = Get.GetChatId(update);
            _telegramId = userId.ToString();
            if (_dataManager.GetData<ICustomerManager>().ExistTelegram(userId))
            {
                await SendStartMessage(update);
                List<KeyboardButton> inlineKeyboardButtons = new List<KeyboardButton>();
                ReplyKeyboardMarkup replyKeyboard = new ReplyKeyboardMarkup(new[]
                {
                    inlineKeyboardButtons
                 });
                replyKeyboard.ResizeKeyboard = true;
                replyKeyboard.OneTimeKeyboard = true;
                foreach (var miniCommand in _supportMiniComands)
                {
                    inlineKeyboardButtons.Add(new KeyboardButton(miniCommand));
                }
                _dataManager.GetData<CommandExecutor>().StartListen(this);
                await _dataManager.GetData<TelegramBotManager>().SendTextMessage(chatId, "Вы уверены, что хотите удалить аккаунт? 😰", replyKeyboard);
            }
            else
            {
                await _dataManager.GetData<CommandExecutor>().DefaultCommandMessage(update);
                _dataManager.GetData<ILogger>().Debug($"User:{userId}; Command:{nameof(QuitCommand)} - command don`t access. User isn`t auth in Bot.");
            }
        }

        public async Task GetUpdate(Update update)
        {
            string messageText = Get.GetText(update);
            long userId = Get.GetUserId(update);
            if (_supportMiniComands.Contains(messageText))
            {
                if (messageText == NO)
                {
                    _dataManager.GetData<CommandExecutor>().StopListen(this);
                    await _dataManager.GetData<CommandExecutor>().ListCommandMessage(update, false, _messages[MessageKey.TRANSITION_HELP]);
                    return;
                }
            }
            else
            {
                await Execute(update);
                return;
            }
            Quit();
            await FinalMessage(update);
            _dataManager.GetData<ILogger>().Debug($"User:{userId}; Command:{nameof(QuitCommand)} - Quit account");
        }

        private void Quit()
        {
            var client = _dataManager.GetData<ICustomerManager>().GetClientByTelegram(_telegramId);
            _dataManager.GetData<ICustomerManager>().DeleteClient(client);
        }

        private async Task FinalMessage(Update update)
        {
            _dataManager.GetData<CommandExecutor>().StopListen(this);
            await _dataManager.GetData<CommandExecutor>().ListCommandMessage(update, false, "Вы успешно удалили аккаунт 😭");
        }
    }
}
