namespace Service.Support
{
    using Service.Enums;
    using Telegram.Bot.Types;

    /// <summary>
    /// Кастомная реализация InputMedia
    /// </summary>
    //NOTE:InputMedia от Telegram.Bot не содержит тип медиа-контента VideoNote
    public class InputMediaCustom
    {
        /// <summary>
        /// Файл
        /// </summary>
        public InputFile Media { get; private set; }

        /// <summary>
        /// Тип медиа контента
        /// </summary>
        public MediaType MediaType { get; private set; }


        public InputMediaCustom(InputFile file, MediaType mediaType)
        {
            Media = file;
            MediaType = mediaType;
        }
    }
}
