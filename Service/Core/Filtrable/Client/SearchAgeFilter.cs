namespace Service.Core.Filtrable.Client
{
    using Service.Abstract.Filtrable;
    using Service.ViewModels;
    using System.Collections.Generic;
    using System.Linq;


    /// <summary>
    /// Фильтр поиска по возрасту
    /// </summary>
    public class SearchAgeFilter : IClientFitrable
    {
        private ClientViewModel _myClient = null;

        public SearchAgeFilter(ClientViewModel myClient) => _myClient = myClient;

        public IEnumerable<ClientViewModel> Filter(IEnumerable<ClientViewModel> filters)
        {
            return filters.Where(x =>
            Get.GetAge(x.Birthday.Value) >= _myClient.MinSearchAge && Get.GetAge(x.Birthday.Value) <= _myClient.MaxSearchAge &&
            Get.GetAge(_myClient.Birthday.Value) >= x.MinSearchAge && Get.GetAge(_myClient.Birthday.Value) <= x.MaxSearchAge
            );
        }
    }
}
