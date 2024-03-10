namespace Service.ViewModels
{
    /// <summary>
    /// Видео-кружок от проверенного совпадений партнеров
    /// </summary>
    public class ClientMatchCheckedVideoNoteInfoViewModel : BaseVideoNoteInfoViewModel
    {
        /// <summary>
        /// Id видео-кружок
        /// </summary>
        public int ClientMatchCheckedVideoNoteInfoId { get; set; }

        /// <summary>
        /// Id совпадения партнеров
        /// </summary>
        public int ClientMatchCheckedId { get; set; }
    }
}
