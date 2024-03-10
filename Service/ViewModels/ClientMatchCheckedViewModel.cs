namespace Service.ViewModels
{
    /// <summary>
    /// Проверенное совпадение пары
    /// </summary>
    public class ClientMatchCheckedViewModel : BaseClientMatchViewModel
    {
        /// <summary>
        /// Id совпадения
        /// </summary>
        public int ClientMatchCheckedId { get; set; }

        /// <summary>
        /// Видео-кружок с любовного письма
        /// </summary>
        public virtual ClientMatchCheckedVideoNoteInfoViewModel? ClientMatchCheckedVideoNoteInfo { get; set; }

        /// <summary>
        /// Видео с любовного письма
        /// </summary>
        public virtual ClientMatchCheckedVideoInfoViewModel? ClientMatchCheckedVideoInfo { get; set; }
    }
}
