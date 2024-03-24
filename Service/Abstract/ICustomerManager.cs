namespace Service.Abstract
{
    using Service.Abstract.Filtrable;
    using Service.Support;
    using Service.ViewModels;

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
        /// Возвращает список всех клиентов
        /// </summary>
        /// <returns></returns>
        List<ClientViewModel> GetClients(params ClientViewModel?[] excludeClients);

        /// <summary>
        /// Удалить клиента
        /// </summary>
        /// <param name="clientViewModel"></param>
        void DeleteClient(ClientViewModel clientViewModel);

        /// <summary>
        /// Возвращает Id клиентов
        /// </summary>
        /// <returns></returns>
        IQueryable<string> GetTelegramClientsId();

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
        /// <param name="isIncludeAvatar">Включая аватар</param>
        /// <returns></returns>
        List<InputMediaCustom> GetMediaFilesByUserId(long userId, bool isIncludeAvatar = false);

        /// <summary>
        /// Получить все медиа файлы анкеты у клиента
        /// </summary>
        /// <param name="user"></param>
        /// <param name="isIncludeAvatar"></param>
        /// <returns></returns>
        List<InputMediaCustom> GetMediaFilesByUserId(ClientViewModel user, bool isIncludeAvatar = false);

        /// <summary>
        /// Получить Id файл аватара
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        InputMediaCustom? GetAvatarFileByUserId(long userId);

        /// <summary>
        /// Создать непроверенное совпадение для клиента
        /// </summary>
        /// <param name="clientMatchViewModel"></param>
        void CreateClientMatch(ClientMatchUncheckedViewModel clientMatchViewModel);

        /// <summary>
        /// Обновить непроверенное совпадение пар
        /// </summary>
        /// <param name="clientMatchViewModel"></param>
        void UpdateClientMatch(ClientMatchUncheckedViewModel clientMatchViewModel);

        /// <summary>
        /// Получить все не проверенные совпадения клиента по Id
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IQueryable<ClientMatchUncheckedViewModel> GetAllClientUncheckedMatchsByClientId(int clientId);

        /// <summary>
        /// Получить все не проверенные совпадения клиента по Id телеграмма
        /// </summary>
        /// <param name="telegramId"></param>
        /// <returns></returns>
        IQueryable<ClientMatchUncheckedViewModel> GetAllClientUncheckedMatchsByTelegramId(string telegramId);

        /// <summary>
        /// Получить все не проверенные совпадения
        /// </summary>
        /// <returns></returns>
        IQueryable<ClientMatchUncheckedViewModel> GetAllClientUncheckedMatchs();

        /// <summary>
        /// Получуть не проверенное совпадение клиента у конкретного телеграмм Id
        /// </summary>
        /// <param name="clientMatchInfoId">Id чьи совпадения</param>
        /// <param name="telegramId">Телегамм Id</param>
        /// <returns></returns>
        ClientMatchUncheckedViewModel? GetTargetClientUncheckedMatch(int clientMatchInfoId, string telegramId);

        /// <summary>
        /// Устанавливает совпадение просмотренным
        /// </summary>
        /// <param name="clientMatchUncheckedId"></param>
        void SetWatchClientMatch(ClientMatchUncheckedViewModel clientMatchUncheckedView);

        /// <summary>
        /// Есть ли у клиента новые лайки
        /// </summary>
        /// <param name="clientMatchInfo"></param>
        /// <returns></returns>
        bool HasClientNewLikes(ClientMatchInfoViewModel clientMatchInfo);

        /// <summary>
        /// Кол-во новых лайков у клиента
        /// </summary>
        /// <param name="clientMatchInfo"></param>
        /// <returns></returns>
        int NewLikesCountByClientMatchInfo(ClientMatchInfoViewModel clientMatchInfo, bool isUnwatchedOnly = true);

        /// <summary>
        /// Обновляет времч показа последнего новых лайков у клиента
        /// </summary>
        /// <param name="clientMatchInfo"></param>
        void UpdateTimeShowNewLikes(ClientMatchInfoViewModel clientMatchInfo);

        /// <summary>
        /// Есть ли не проверенные совпадения у клиента по отношению к другому клиенту
        /// </summary>
        /// <param name="clientMatchInfoId"></param>
        /// <param name="telegramId"></param>
        /// <returns></returns>
        bool AnyTargetClientUncheckedMatch(int clientMatchInfoId, string telegramId);

        /// <summary>
        /// Получить все проверенные совпадения клиента по Id
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IQueryable<ClientMatchCheckedViewModel> GetAllClientCheckedMatchsByClientId(int clientId);

        /// <summary>
        /// Получить все проверенные совпадения клиента по Id телеграмма
        /// </summary>
        /// <param name="telegramId"></param>
        /// <returns></returns>
        IQueryable<ClientMatchCheckedViewModel> GetAllClientCheckedMatchsByTelegramId(string telegramId);

        /// <summary>
        /// Получить все проверенные совпадения
        /// </summary>
        /// <returns></returns>
        IQueryable<ClientMatchCheckedViewModel> GetAllClientCheckedMatchs();

        /// <summary>
        /// Получуть проверенное совпадение клиента
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        ClientMatchCheckedViewModel? GetTargetClientCheckedMatch(int clientMatchInfoId, string telegramId);

        /// <summary>
        /// Получает список поисковых фильтров 
        /// </summary>
        /// <param name="myClient"></param>
        /// <returns></returns>
        List<IClientFitrable> GetFindClientFilters(ClientViewModel myClient);
    }
}
