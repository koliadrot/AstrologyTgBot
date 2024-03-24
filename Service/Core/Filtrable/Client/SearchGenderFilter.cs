namespace Service.Core.Filtrable.Client
{
    using Service.Abstract.Filtrable;
    using Service.Enums;
    using Service.ViewModels;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Фильтр поиска по полу
    /// </summary>
    public class SearchGenderFilter : IClientFitrable
    {
        private ClientViewModel _myClient = null;

        public SearchGenderFilter(ClientViewModel myClient) => _myClient = myClient;

        public IEnumerable<ClientViewModel> Filter(IEnumerable<ClientViewModel> filters)
        {
            return _myClient.SearchGender == GenderType.NoGender.ToString()
                ? filters.Where(x => x.SearchGender == GenderType.NoGender.ToString())
                : filters.Where(x => x.BirthGender == _myClient.SearchGender && x.SearchGender == _myClient.BirthGender);
        }
    }
}
