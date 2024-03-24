namespace Service.Core.Filtrable.Client
{
    using Service.Abstract.Filtrable;
    using Service.Enums;
    using Service.ViewModels;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Фильтр если лайк уже отправлен или было совпадение
    /// </summary>
    public class AlreadyMatchFilter : IClientFitrable
    {
        private ClientViewModel _myClient = null;

        public AlreadyMatchFilter(ClientViewModel myClient) => _myClient = myClient;

        public IEnumerable<ClientViewModel> Filter(IEnumerable<ClientViewModel> filters)
        {
            return filters.Where(x => !x.ClientMatchInfo.UncheckedClientMatchs.Any(match => match.MatchTelegramId == _myClient.TelegramId)
            && !x.ClientMatchInfo.CheckedClientMatchs.Any(match => match.MatchTelegramId == _myClient.TelegramId && match.AnswearMatchType != MatchType.Dislike.ToString()));
        }
    }
}
