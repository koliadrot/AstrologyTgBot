namespace Service.Core.TelegramBot.Commands
{
    using Service.Abstract;
    using Service.Abstract.TelegramBot;
    using Service.Enums;
    using Service.Support;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Telegram.Bot.Types;

    /// <summary>
    /// Команда выдачи анкеты другого клиента по Id или username
    /// </summary>
    public class GetClientApplicationCommand : ICommand, IUpdater
    {
        public string Name { get; set; } = "/getapp26";

        public string Description { get; set; } = "Чужая анкета по ID";

        public string ShortDescription { get; set; } = "Чужая анкета 26";

        public bool IsAuth { get; set; } = false;
        public bool IsDefault { get; set; } = true;

        public TelegramBotCommandType CommandType => TelegramBotCommandType.Custom;
        public bool IsStartMenu => false;

        private readonly DataManager _dataManager;
        private readonly Dictionary<string, string> _messages;
        private readonly TelegramSupport _telegramSupport;

        public GetClientApplicationCommand(DataManager dataManager)
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

            long chatid = Get.GetChatId(update);
            await _dataManager.GetData<TelegramBotManager>().SendTextMessage(chatid, $"Необходимо ввести телеграмм id или username пользователя");
            _dataManager.GetData<CommandExecutor>().StartListen(this);
        }

        public async Task GetUpdate(Update update)
        {
            string messageText = Get.GetText(update);
            if (long.TryParse(messageText, out long userId))
            {
                if (_dataManager.GetData<ICustomerManager>().ExistTelegram(userId))
                {
                    await _telegramSupport.SendUserApplication(update, userId, nameof(GetClientApplicationCommand));
                    _dataManager.GetData<CommandExecutor>().StopListen(this);
                }
            }
            else
            {
                List<long> telegramIds = _dataManager.GetData<ICustomerManager>().GetIdTelegramsByUserName(messageText);
                if (telegramIds != null)
                {
                    foreach (var telegramUserId in telegramIds)
                    {
                        await _telegramSupport.SendUserApplication(update, telegramUserId, nameof(GetClientApplicationCommand));
                    }
                    _dataManager.GetData<CommandExecutor>().StopListen(this);
                }
                else
                {
                    await Execute(update);
                }
            }
        }
    }
}
