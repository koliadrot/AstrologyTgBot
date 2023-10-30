using Service.Abstract;
using Service.Abstract.TelegramBot;
using Service.Enums;
using Service.Extensions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

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

        public async Task Execute(Update update, string[] arg = null)
        {
            long chatId = Get.GetChatId(update);
            string message = string.Empty;
            List<KeyboardButton> inlineKeyboardButtons = new List<KeyboardButton>();
            ReplyKeyboardMarkup replyKeyboard = new ReplyKeyboardMarkup(new[]
            {
                inlineKeyboardButtons
            });
            replyKeyboard.ResizeKeyboard = true;
            replyKeyboard.OneTimeKeyboard = true;

            string helloText = _dataManager.GetData<ISettingsManager>().GetTelegramBot().HelloText;
            message = helloText.IsNull() ? DEFAULT_HELLO_TEXT : helloText;

            long userId = Get.GetUserId(update);
            await _dataManager.GetData<CommandExecutor>().InitCommands(userId);
            await _dataManager.GetData<CommandExecutor>().ListCommandMessage(update, false, message);
        }
    }
}