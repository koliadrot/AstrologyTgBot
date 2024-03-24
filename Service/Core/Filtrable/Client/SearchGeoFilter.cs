namespace Service.Core.Filtrable.Client
{
    using Service.Abstract.Filtrable;
    using Service.Extensions;
    using Service.ViewModels;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Фильтр поиска по расположению
    /// </summary>
    public class SearchGeoFilter : IClientFitrable
    {
        private ClientViewModel _myClient = null;

        public SearchGeoFilter(ClientViewModel myClient) => _myClient = myClient;

        public IEnumerable<ClientViewModel> Filter(IEnumerable<ClientViewModel> filters)
        {
            IOrderedEnumerable<ClientViewModel> sortedClients = filters.OrderBy(client => SearchDistanceTo(_myClient, client));
            IEnumerable<IGrouping<string?, ClientViewModel>> groupClients = sortedClients.GroupBy(x => x.SearchCoord);
            List<ClientViewModel> shuffledClients = new List<ClientViewModel>();

            foreach (var group in groupClients)
            {
                shuffledClients.AddRange(group.OrderBy(x => Guid.NewGuid()));
            }
            return shuffledClients;
        }

        private double SearchDistanceTo(ClientViewModel from, ClientViewModel to) => VariableExtensions.DistanceTo(from.SearchCoordinate, to.SearchCoordinate);
    }
}
