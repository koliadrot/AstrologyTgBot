namespace Service.ViewModels
{
    /// <summary>
    /// Видео от не проверенного совпадений партнеров
    /// </summary>
    public class ClientMatchUncheckedVideoInfoViewModel : BaseVideoInfoViewModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public int ClientMatchUncheckedVideoInfoId { get; set; }

        /// <summary>
        /// Id не проверенного совпадения
        /// </summary>
        public int ClientMatchUncheckedId { get; set; }
    }
}
