using Telegram.Bot.Types;

namespace Service.Abstract.TelegramBot
{
    /// <summary>
    /// Условие
    /// </summary>
    public interface ICondition
    {
        /// <summary>
        /// Выполнено ли условие
        /// </summary>
        bool IsDone { get; }

        /// <summary>
        /// Отменено ли условие
        /// </summary>
        bool IsCancel { get; }

        /// <summary>
        /// Запущено ли условие
        /// </summary>
        bool IsStarted { get; }

        /// <summary>
        /// Игнорировать ли другие услвия после выполнения текущего
        /// </summary>
        bool IsIgnoredNextCondition { get; }

        /// <summary>
        /// Mожет ли быть пропущено условие?
        /// </summary>
        bool IsCanPass { get; set; }

        /// <summary>
        /// Возможная информация выполненого условия
        /// </summary>
        object Info { get; }

        /// <summary>
        /// Порядковый номер
        /// </summary>
        int Order { get; set; }

        /// <summary>
        /// Список условий для выполнения условия
        /// </summary>
        List<ICondition> Conditions { get; }

        /// <summary>
        /// Выполнить запуск условия
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        Task Execute(Update update);

        /// <summary>
        /// Проверка условия
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        Task<bool> CheckCondition(Update update);
    }
}
