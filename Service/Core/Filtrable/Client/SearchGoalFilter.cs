namespace Service.Core.Filtrable.Client
{
    using Service.Abstract.Filtrable;
    using Service.ViewModels;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Фильтр поиска по цели поиска
    /// </summary>
    public class SearchGoalFilter : IClientFitrable
    {
        private ClientViewModel _myClient = null;

        public SearchGoalFilter(ClientViewModel myClient) => _myClient = myClient;

        public IEnumerable<ClientViewModel> Filter(IEnumerable<ClientViewModel> filters) => filters.Where(x => x.SearchGoal == _myClient.SearchGoal);
    }
}
