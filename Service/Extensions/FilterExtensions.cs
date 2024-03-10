namespace Service.Extensions
{
    using Service.Abstract.Filtrable;
    using Service.ViewModels;
    using System.Collections.Generic;

    /// <summary>
    /// Расширение фильтров
    /// </summary>
    public static class FilterExtensions
    {
        /// <summary>
        /// Фильтр клиентов
        /// </summary>
        /// <param name="client"></param>
        /// <param name="filters"></param>
        /// <returns></returns>
        public static IEnumerable<ClientViewModel> Filter(this IEnumerable<ClientViewModel> client, IEnumerable<IClientFitrable> filters)
        {
            foreach (var filter in filters)
            {
                client = filter.Filter(client);
            }
            return client;
        }
    }
}
