namespace Service.ViewModels
{
    /// <summary>
    /// Не проверенное совпадение пары
    /// </summary>
    public class ClientMatchUncheckedViewModel : BaseClientMatchViewModel
    {
        /// <summary>
        /// Id совпадения
        /// </summary>
        public int ClientMatchUncheckedId { get; set; }

        /// <summary>
        /// Просмотрено ли совпадние пользователем которому адресовано
        /// </summary>
        public bool IsWatched { get; set; }

        /// <summary>
        /// Видео-кружок с любовного письма
        /// </summary>
        public virtual ClientMatchUncheckedVideoNoteInfoViewModel? ClientMatchUncheckedVideoNoteInfo { get; set; }

        /// <summary>
        /// Видео с любовного письма
        /// </summary>
        public virtual ClientMatchUncheckedVideoInfoViewModel? ClientMatchUncheckedVideoInfo { get; set; }
    }
}
