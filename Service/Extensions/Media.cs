namespace Service.Extensions
{
    using SixLabors.ImageSharp;
    using SixLabors.ImageSharp.Formats.Jpeg;
    using SixLabors.ImageSharp.PixelFormats;
    using System;

    /// <summary>
    /// Мультимедиа расширения
    /// </summary>
    public static class Media
    {
        /// <summary>
        /// Получить массив байтов по расположению картинки
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static byte[] GetImageBytesByFilePath(string filePath)
        {
            using (Image<Rgba32> image = Image.Load<Rgba32>(filePath))
            {
                using (var ms = new MemoryStream())
                {
                    image.SaveAsPng(ms);
                    return ms.ToArray();
                }
            }
        }

        /// <summary>
        /// Получить массив байтов из картинки
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static byte[] GetImageBytes(Image<Rgba32> img)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, new JpegEncoder());
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Сохранение картинки на устройтво
        /// </summary>
        /// <param name="imageBytes"></param>
        /// <param name="fileName"></param>
        /// <param name="savePath"></param>
        public static void SaveImageFromBytes(byte[] imageBytes, string fileName = "", string savePath = "")
        {
            using (MemoryStream memoryStream = new MemoryStream(imageBytes))
            {
                using (Image<Rgba32> image = Image.Load<Rgba32>(memoryStream))
                {
                    if (fileName.IsNull())
                    {
                        fileName = $"Photo_{Guid.NewGuid()}.jpg";
                    }
                    string localFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, savePath, fileName);

                    Directory.CreateDirectory(Path.GetDirectoryName(localFilePath));
                    image.Save(localFilePath);
                }
            }
        }

        /// <summary>
        /// Загрузка картинки по массиву байтов
        /// </summary>
        /// <param name="imageBytes"></param>
        /// <returns></returns>
        public static Image<Rgba32> LoadImageFromBytes(byte[] imageBytes)
        {
            using (MemoryStream memoryStream = new MemoryStream(imageBytes))
            {
                return Image.Load<Rgba32>(memoryStream);
            }
        }

        /// <summary>
        /// Получить массив байтов по расположению видеофайла
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static byte[] GetVideoBytesByFilePath(string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                byte[] videoBytes = new byte[fs.Length];
                fs.Read(videoBytes, 0, videoBytes.Length);
                return videoBytes;
            }
        }

        /// <summary>
        /// Сохранение видео на устройство
        /// </summary>
        /// <param name="videoBytes"></param>
        /// <param name="fileName"></param>
        /// <param name="savePath"></param>
        public static void SaveVideoFromBytes(byte[] videoBytes, string fileName = "", string savePath = "")
        {
            if (videoBytes == null || videoBytes.Length == 0)
            {
                // Обработка ситуации, когда массив байтов видео пуст
                return;
            }

            if (fileName.IsNull())
            {
                fileName = $"Video_{Guid.NewGuid()}.mp4";
            }

            string localFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, savePath, fileName);

            Directory.CreateDirectory(Path.GetDirectoryName(localFilePath));

            using (FileStream fs = new FileStream(localFilePath, FileMode.Create, FileAccess.Write))
            {
                fs.Write(videoBytes, 0, videoBytes.Length);
            }
        }

        /// <summary>
        /// Загрузка видео из массива байтов
        /// </summary>
        /// <param name="videoBytes"></param>
        /// <returns></returns>
        public static Stream LoadVideoStreamFromBytes(byte[] videoBytes)
        {
            if (videoBytes == null || videoBytes.Length == 0)
            {
                // Обработка ситуации, когда массив байтов видео пуст
                return null;
            }

            return new MemoryStream(videoBytes);
        }

        ///// <summary>
        ///// Загрузка видео из массива байтов и возвращение его в виде объекта Video
        ///// (предполагается, что у вас есть соответствующий класс Video для представления видео)
        ///// </summary>
        ///// <param name="videoBytes"></param>
        ///// <returns></returns>
        //public static Video LoadVideoFromBytes(byte[] videoBytes)
        //{
        //    // Предположим, у вас есть класс Video для представления видео
        //    // и у этого класса есть конструктор, принимающий Stream (или другие необходимые параметры).

        //    Stream videoStream = LoadVideoStreamFromBytes(videoBytes);

        //    if (videoStream == null)
        //    {
        //        // Обработка ситуации, когда массив байтов видео пуст
        //        return null;
        //    }

        //    // Создайте объект Video, используя videoStream и другие необходимые параметры
        //    return new Video(videoStream);
        //}

    }
}
