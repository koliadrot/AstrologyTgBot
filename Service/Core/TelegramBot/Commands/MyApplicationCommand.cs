namespace Service.Core.TelegramBot.Commands
{
    using Service.Abstract.TelegramBot;
    using Service.Enums;
    using Service.Support;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Telegram.Bot.Types;

    /// <summary>
    /// Команда анкеты
    /// </summary>
    public class MyApplicationCommand : ICommand
    {
        public string Name { get; set; } = "/myapplication";

        public string Description { get; set; } = "Моя анкета";

        public string ShortDescription { get; set; } = "Анкета";

        public bool IsAuth { get; set; } = true;
        public bool IsDefault { get; set; } = false;

        public TelegramBotCommandType CommandType => TelegramBotCommandType.Custom;
        public bool IsStartMenu => false;

        private readonly DataManager _dataManager;
        private readonly Dictionary<string, string> _messages;
        private readonly TelegramSupport _telegramSupport;

        public MyApplicationCommand(DataManager dataManager)
        {
            _dataManager = dataManager;
            _messages = _dataManager.GetData<CommandExecutor>().Messages;
            _telegramSupport = new TelegramSupport(dataManager);
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

            long userId = Get.GetUserId(update);
            await _telegramSupport.SendUserApplication(update, userId);
        }
    }
}
