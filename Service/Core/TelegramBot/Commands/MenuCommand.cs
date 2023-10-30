using Service.Abstract.TelegramBot;
using Service.Enums;
using System.Threading.Tasks;
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

        public MenuCommand(DataManager dataManager)
        {
            _dataManager = dataManager;
        }

        public async Task Execute(Update update, string[] arg = null) => await _dataManager.GetData<CommandExecutor>().ListCommandMessage(update, false, "Пожалуйста,выберете,какую информацию вы хотите получить 👇");
    }
}
