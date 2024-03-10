namespace Service.ViewModels
{
    using Data.Core.Interfaces;

    /// <summary>
    /// Видео-Кружок клиента
    /// </summary>
    public class ClientVideoNoteInfoViewModel : BaseVideoNoteInfoViewModel, IAvatarable
    {
        /// <summary>
        /// Id видео-кружок
        /// </summary>
        public int ClientVideoNoteInfoId { get; set; }

        /// <summary>
        /// Id медиа информации
        /// </summary>
        public int ClientMediaInfoId { get; set; }

        /// <summary>
        /// Id альбом группы
        /// </summary>
        public string? MediaGroupId { get; set; }

        /// <summary>
        /// Это аватар?
        /// </summary>
        public bool? IsAvatar { get; set; }
    }
}
