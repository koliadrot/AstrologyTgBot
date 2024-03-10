using Service.Abstract;
using Service.Abstract.TelegramBot;
using Service.Enums;
using Service.Extensions;
using Telegram.Bot.Types;

namespace Service.Core.TelegramBot.Commands
{
    /// <summary>
    /// Стартовая команда
    /// </summary>
    public class StartCommand : ICommand
    {
        public string Name { get; set; } = "/start";

        public string Description { get; set; } = "Информация о боте";

        public string ShortDescription { get; set; } = "Инфо";

        public bool IsAuth { get; set; } = false;
        public bool IsDefault { get; set; } = true;

        public TelegramBotCommandType CommandType => TelegramBotCommandType.Custom;
        public bool IsStartMenu => false;

        private readonly DataManager _dataManager;
        private readonly Dictionary<string, string> _messages;
        private const string DEFAULT_HELLO_TEXT = "Привет!";

        public StartCommand(DataManager dataManager)
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
            await SendStartMessage(update);
            string helloText = _dataManager.GetData<ISettingsManager>().GetTelegramBot().HelloText;
            string message = helloText.IsNull() ? DEFAULT_HELLO_TEXT : helloText;

            await _dataManager.GetData<CommandExecutor>().ListCommandMessage(update, false, message);
        }
    }
}