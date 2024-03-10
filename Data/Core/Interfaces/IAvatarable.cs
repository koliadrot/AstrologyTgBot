namespace Data.Core.Interfaces
{
    /// <summary>
    /// Метка аватара
    /// </summary>
    public interface IAvatarable
    {
        /// <summary>
        /// Аватар ли это
        /// </summary>
        public bool? IsAvatar { get; set; }
    }
}
