using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Service.Abstract.TelegramBot
{

    /// <summary>
    /// Интерфейс обновляющего.Исполняет команды обновлений.Для работы с перенаправлением обновлений сообщений.
    /// По сути начинаем рекурсию в рекурсии.
    /// Например:регистрация, где пользователь сначала указывает номер, потом имя, фамилию и тд.
    /// с возможностью перенаправлять обновления из друг в друга.
    /// </summary>
    public interface IUpdater
    {

        /// <summary>
        /// Обработать обновление
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        Task GetUpdate(Update update);
    }
}
