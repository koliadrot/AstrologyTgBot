using Service.Abstract.TelegramBot;
using Service.Enums;
using Service.Extensions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Service.Core.TelegramBot.Commands
{
    // <summary>
    /// Команда-шаблон информации
    /// </summary>
    public class InfoCommand : ICommand
    {
        public string Name { get; set; } = "/info";

        public string Description { get; set; } = "Предоставляет информацию";
        public string ShortDescription { get; set; } = "Информация";

        public bool IsAuth { get; set; } = false;

        public bool IsDefault { get; set; } = true;

        public bool IsStartMenu => false;

        /// <summary>
        /// Наполение информации
        /// </summary>
        public string Info { get; private set; } = string.Empty;

        public TelegramBotCommandType CommandType => TelegramBotCommandType.Info;

        private readonly DataManager _dataManager;
        private readonly Dictionary<string, string> _messages;

        public InfoCommand(DataManager dataManager, string info)
        {
            _dataManager = dataManager;
            Info = info;
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
            await SendStartMessage(update);
            if (Info.IsValidLink())
            {
                var replyKeyboard = new InlineKeyboardMarkup(InlineKeyboardButton.WithUrl(_messages[ReplyButton.GO_TO_LINK], Info));
                await _dataManager.GetData<TelegramBotManager>().SendTextMessage(chatId, Description.IsNull() ? ShortDescription : Description, replyMarkup: replyKeyboard);
            }
            else
            {
                await _dataManager.GetData<TelegramBotManager>().SendTextMessage(chatId, Info);
            }
        }
    }
}
