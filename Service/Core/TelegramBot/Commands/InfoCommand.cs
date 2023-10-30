using Service.Abstract.TelegramBot;
using Service.Enums;
using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Service.Core.TelegramBot.Commands
{
    /// <summary>
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

        public TelegramBotCommandType CommandType => TelegramBotCommandType.Info;

        private string _info = string.Empty;
        private readonly DataManager _dataManager;

        public InfoCommand(DataManager dataManager, string info)
        {
            _dataManager = dataManager;
            _info = info;
        }

        public async Task Execute(Update update, string[] arg = null)
        {
            long chatId = Get.GetChatId(update);
            if (IsValidLink(_info))
            {
                var keyboard = new InlineKeyboardMarkup(InlineKeyboardButton.WithUrl(ShortDescription, _info));
                await _dataManager.GetData<TelegramBotManager>().SendTextMessage(chatId, Description, replyMarkup: keyboard);
            }
            else
            {
                await _dataManager.GetData<TelegramBotManager>().SendTextMessage(chatId, _info);
            }
        }

        private bool IsValidLink(string link) => Uri.TryCreate(link, UriKind.Absolute, out Uri uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}
