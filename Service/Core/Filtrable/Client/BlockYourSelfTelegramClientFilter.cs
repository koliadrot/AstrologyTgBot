namespace Service.Core.Filtrable.Client
{
    using Service.Abstract.Filtrable;
    using Service.ViewModels;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Блокирует свой собственный клиентский фильтр по Id Телеграмму
    /// </summary>
    public class BlockYourSelfTelegramClientFilter : IClientFitrable
    {
        private string _myTelegramId = default;

        public BlockYourSelfTelegramClientFilter(long myTelegramId) => _myTelegramId = myTelegramId.ToString();

        public BlockYourSelfTelegramClientFilter(ClientViewModel myClient) => _myTelegramId = myClient.TelegramId;

        public IEnumerable<ClientViewModel> Filter(IEnumerable<ClientViewModel> filters) => filters.Where(x => x.TelegramId != _myTelegramId);
    }
}
