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

        public MyApplicationCommand(DataManager dataManager)
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

            long userId = Get.GetUserId(update);
            long chatid = Get.GetChatId(update);
            var user = _dataManager.GetData<ICustomerManager>().GetClientByTelegram(userId.ToString());

            int age = Get.GetAge(user.Birthday.Value);
            string caption = $"{user.FirstName} - {age}, {user.SearchCity}\n{user.AboutMe}";
            var media = _dataManager.GetData<ICustomerManager>().GetMediaFilesByUserId(userId);

            if (media.Any() && media.Count > 0)
            {
                if (media.Count == 1)
                {
                    var file = media.FirstOrDefault().Media;
                    await _dataManager.GetData<TelegramBotManager>().SendPhotoMessage(chatid, file, caption);
                }
                else
                {
                    await _dataManager.GetData<TelegramBotManager>().SendMediaGroupMessage(chatid, media, caption);
                }
            }
        }
    }
}
