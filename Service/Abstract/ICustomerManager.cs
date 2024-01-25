namespace Service.Abstract
{
    using Service.ViewModels;
    using Telegram.Bot.Types;

    public interface ICustomerManager
    {
        /// <summary>
        /// Создать нового клиента
        /// </summary>
        /// <param name="clientViewModel"></param>
        /// <returns></returns>
        ClientViewModel CreateClient(ClientViewModel clientViewModel);

        /// <summary>
        /// Обновить клиента
        /// </summary>
        /// <param name="clientViewModel"></param>
        /// <returns></returns>
        ClientViewModel UpdateClient(ClientViewModel clientViewModel);

        /// <summary>
        /// Удалить клиента
        /// </summary>
        /// <param name="clientViewModel"></param>
        void DeleteClient(ClientViewModel clientViewModel);

        /// <summary>
        /// Есть ли у клиента телеграмм аккаунт по Id
        /// </summary>
        /// <param name="telegramId"></param>
        /// <returns></returns>
        bool ExistTelegram(long telegramId);

        /// <summary>
        /// Есть ли у клиента телеграмм аккаунт по техническому имени
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        bool ExistTelegram(string userName);

        /// <summary>
        /// Получить клиента по id пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        ClientViewModel GetClientByTelegram(string userId);

        /// <summary>
        /// Получить клиента по техническому имени пользователя
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        ClientViewModel GetClientByUserName(string userName);

        /// <summary>
        /// Получить список Id клиентов по техническому имени пользователя
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        List<long> GetIdTelegramsByUserName(string userName);

        /// <summary>
        /// Получить все медиа файлы анкеты у клиента
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<InputMedia> GetMediaFilesByUserId(long userId);
    }
}
