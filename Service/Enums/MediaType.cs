namespace Service.Enums
{
    using System.ComponentModel;

    /// <summary>
    /// Тип медиа контента
    /// </summary>
    public enum MediaType
    {
        [Description("Фото")]
        Photo = 0,
        [Description("Видео")]
        Video = 1,
        [Description("Видео-кружок")]
        VideoNote = 2
    }
}
