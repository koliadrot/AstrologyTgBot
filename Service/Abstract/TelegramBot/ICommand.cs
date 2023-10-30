using Service.Enums;
using Telegram.Bot.Types;

namespace Service.Abstract.TelegramBot
{
    /// <summary>
    /// Интерфейс комманд бота
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Имя команда
        /// </summary>
        string Name { set; get; }

        /// <summary>
        /// Описание команды
        /// </summary>
        string Description { set; get; }

        /// <summary>
        /// Короткое описание команды
        /// </summary>
        string ShortDescription { set; get; }

        /// <summary>
        /// Тип команды, которому требуется авторизация пользователя
        /// </summary>
        bool IsAuth { get; set; }

        /// <summary>
        /// Команда доступная по-умолчанию
        /// </summary>
        bool IsDefault { get; set; }

        /// <summary>
        /// Показывать команду в стартовом меню телеграм меню бота
        /// </summary>
        /// NOTE:Т.к бот динамически не умеет показывать меню для каждого пользователя
        /// надо явно указать какие команды будут неизменны всегда в меню
        bool IsStartMenu { get; }

        /// <summary>
        /// Тип команды
        /// </summary>
        TelegramBotCommandType CommandType { get; }

        /// <summary>
        /// Выполнить команду
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        Task Execute(Update update, string[] args = null);
    }
}
