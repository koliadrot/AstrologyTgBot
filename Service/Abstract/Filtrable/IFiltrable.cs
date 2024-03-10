namespace Service.Abstract.Filtrable
{
    using System.Collections.Generic;

    /// <summary>
    /// Шаблон фильтра
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IFiltrable<T> where T : class
    {
        /// <summary>
        /// Фильтр
        /// </summary>
        /// <param name="Filters"></param>
        /// <returns></returns>
        IEnumerable<T> Filter(IEnumerable<T> filters);
    }
}
