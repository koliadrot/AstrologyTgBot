namespace Service.ViewModels
{
    /// <summary>
    /// Видео проверенного совпадения от партнеров
    /// </summary>
    public class ClientMatchCheckedVideoInfoViewModel : BaseVideoInfoViewModel
    {
        /// <summary>
        /// Id видео
        /// </summary>
        public int ClientMatchCheckedVideoInfoId { get; set; }

        /// <summary>
        /// Id совпадения партнеров
        /// </summary>
        public int ClientMatchCheckedId { get; set; }
    }
}
