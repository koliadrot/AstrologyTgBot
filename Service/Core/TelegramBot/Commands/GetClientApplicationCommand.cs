namespace Service.Core.TelegramBot.Commands
{
    using Service.Abstract;
    using Service.Abstract.TelegramBot;
    using Service.Enums;
    using System.Collections.Generic;
    using System.Linq;
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

        public GetClientApplicationCommand(DataManager dataManager)
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
                    await SendUserApplication(update, userId);
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
                        await SendUserApplication(update, telegramUserId);
                    }
                    _dataManager.GetData<CommandExecutor>().StopListen(this);
                }
                else
                {
                    await Execute(update);
                }

            }
        }

        private async Task SendUserApplication(Update update, long userId)
        {
            long chatid = Get.GetChatId(update);
            var user = _dataManager.GetData<ICustomerManager>().GetClientByTelegram(userId.ToString());

            int age = Get.GetAge(user.Birthday.Value);
            string caption = $"{user.FirstName} - {age}, {user.SearchCity}\n{user.AboutMe}";
            var media = _dataManager.GetData<ICustomerManager>().GetMediaFilesByUserId(userId);

            if (media.Any() && media.Count > 0)
            {
                if (media.Count == 1)
                {
                    InputMedia file = media.FirstOrDefault();
                    await _dataManager.GetData<TelegramBotManager>().SendMediaMessage(chatid, file, caption);
                }
                else
                {
                    await _dataManager.GetData<TelegramBotManager>().SendMediaGroupMessage(chatid, media, caption);
                }
            }
        }
    }
}
