namespace Service.ViewModels
{
    /// <summary>
    /// Видео-кружок не проверенного совпадения пар
    /// </summary>
    public class ClientMatchUncheckedVideoNoteInfoViewModel : BaseVideoNoteInfoViewModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public int ClientMatchUncheckedVideoNoteInfoId { get; set; }

        /// <summary>
        /// Id не проверенного совпадения
        /// </summary>
        public int ClientMatchUncheckedId { get; set; }
    }
}
