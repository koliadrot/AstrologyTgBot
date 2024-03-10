using Service.Abstract.TelegramBot;
using Service.Enums;
using Telegram.Bot.Types;

namespace Service.Core.TelegramBot.Commands
{
    /// <summary>
    /// Список команд доступных пользователю
    /// </summary>
    public class MenuCommand : ICommand
    {
        public string Name { get; set; } = "/menu";

        public string Description { get; set; } = "Главное меню";

        public string ShortDescription { get; set; } = "Меню";

        public bool IsAuth { get; set; } = false;
        public bool IsDefault { get; set; } = true;

        public TelegramBotCommandType CommandType => TelegramBotCommandType.Custom;

        public bool IsStartMenu => true;

        private readonly DataManager _dataManager;
        private readonly Dictionary<string, string> _messages;

        public MenuCommand(DataManager dataManager)
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
            await _dataManager.GetData<CommandExecutor>().ListCommandMessage(update, false, _messages[MessageKey.TRANSITION_HELP]);
        }
    }
}
