using Data.Core;
using NLog;
using Service.Extensions;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Service.Core.TelegramBot
{
    /// <summary>
    /// Менеджер телеграм бота
    /// </summary>
    public class TelegramBotManager
    {
        public event Action OnStarted = delegate { };
        public event Action OnStoped = delegate { };
        public event Action OnReseted = delegate { };

        /// <summary>
        /// Запущен ли бот
        /// </summary>
        public bool IsStarted { get; private set; } = default;

        /// <summary>
        /// Остановлен ли вручную бот
        /// </summary>
        public bool IsManulStopped { get; private set; } = default;

        /// <summary>
        /// Сброшены ли обновления при отключенном боте?
        /// </summary>
        public bool IsDropPendingUpdates { get; private set; } = default;

        /// <summary>
        /// Имя бота
        /// </summary>
        public string Name { get; private set; } = string.Empty;

        /// <summary>
        /// Короткое описание бота
        /// </summary>
        public string About { get; private set; } = string.Empty;

        /// <summary>
        /// Описание бота
        /// </summary>
        public string Description { get; private set; } = string.Empty;

        /// <summary>
        /// Уникальный Nickname бота
        /// </summary>
        public string UserName { get; private set; } = string.Empty;

        /// <summary>
        /// Url ссылка куда сервер бота шлет данные
        /// </summary>
        public string WebHookUrl { get; private set; } = string.Empty;

        /// <summary>
        /// Api токен
        /// </summary>
        public string Token { get; private set; } = string.Empty;

        /// <summary>
        /// Логгер
        /// </summary>
        public ILogger Logger { get; set; }

        private TelegramBotClient _client;

        /// <summary>
        /// Старт работы бота
        /// </summary>
        /// <returns></returns>
        public async Task Start()
        {
            if (!IsStarted)
            {
                using (ApplicationDbContext bonusDbContext = new ApplicationDbContext())
                {
                    Logger.Debug("Engage start TG bot");
                    var telegramBotParams = bonusDbContext.TelegramParams.FirstOrDefault();
                    var baseUrl = telegramBotParams.WebHookUrl;
                    if (telegramBotParams.TokenApi.IsNull() || !baseUrl.IsValidLink())
                    {
                        Stop(isForceStop: false);
                        Logger.Error("Start TG bot fault! Check Token Api tg bot or WebHook url processing");
                        return;
                    }
                    Name = telegramBotParams.BotName;
                    About = telegramBotParams.BotAbout;
                    Description = telegramBotParams.BotDescription;
                    UserName = telegramBotParams.BotUserName;
                    Token = telegramBotParams.TokenApi;
                    WebHookUrl = baseUrl;
                    if (WebHookUrl.EndsWith("/"))
                    {
                        WebHookUrl = WebHookUrl.TrimEnd('/');
                    }
                    _client = new TelegramBotClient(Token);
                    string hook = $"{WebHookUrl}/{GlobalTelegramSettings.BASE_MESSAGE}/{GlobalTelegramSettings.UPDATE_MESSAGE}";
                    await _client.SetWebhookAsync(hook);
                    var task = await _client.GetWebhookInfoAsync();

                    await SetName(Name);
                    await SetAbout(About);
                    await SetDescription(Description);

                    IsStarted = true;
                    IsManulStopped = false;
                    IsDropPendingUpdates = false;
                    OnStarted();
                    Logger.Debug("TG bot started");
                }
            }
        }

        private async Task SetName(string name)
        {
            if (!name.IsNull())
            {
                try
                {
                    await _client.SetMyNameAsync(name);
                }
                catch (ApiRequestException ex)
                {
                    //NOTE:Часто менять имя нельзя! (приблизительно раз в сутки)
                    Logger.Error($"Failed to set TG bot name. {ex}");
                }
            }
        }

        private async Task SetAbout(string about)
        {
            if (!about.IsNull())
            {
                try
                {
                    await _client.SetMyShortDescriptionAsync(about);
                }
                catch (ApiRequestException ex)
                {
                    //NOTE:Часто менять короткое описание нельзя! (приблизительно раз в сутки)
                    Logger.Error($"Failed to set TG bot about. {ex}");
                }
            }
        }

        private async Task SetDescription(string description)
        {
            if (!description.IsNull())
            {
                try
                {
                    await _client.SetMyDescriptionAsync(description);
                }
                catch (ApiRequestException ex)
                {
                    //NOTE:Часто менять описание нельзя! (приблизительно раз в сутки)
                    Logger.Error($"Failed to set TG bot description. {ex}");
                }
            }
        }

        /// <summary>
        /// Остановка работы бота
        /// </summary>
        /// <param name="isDropPendingUpdates">Если установлен в true, то метод удалит все ожидающие обновления (pending updates),
        /// которые могли накопиться в процессе использования вебхука. Ожидающие обновления - это обновления, которые Telegram API
        /// не смогло доставить вашему боту в прошлые попытки и которые могут быть обработаны позже при удалении вебхука.</param>
        /// <param name="isForceStop"></param>
        /// <returns></returns>
        public void Stop(bool isDropPendingUpdates = true, bool isForceStop = true)
        {
            if (_client != null)
            {
                IsStarted = false;
                IsManulStopped = isForceStop;
                IsDropPendingUpdates = isDropPendingUpdates;
                _client = null;
                OnStoped();
                Logger.Debug("TG bot stopped");
            }
        }

        /// <summary>
        /// Перезагрузка бота
        /// </summary>
        /// <returns></returns>
        public async Task Reset()
        {
            if (_client != null)
            {
                IsStarted = false;
                await Start();
                OnReseted();
            }
        }

        /// <summary>
        /// Обновление запроса пользователя
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        public async Task ReupdateUser(Update update)
        {
            long userId = Get.GetUserId(update);
            await ReupdateUser(userId);
        }

        public async Task ReupdateUser(long userId)
        {
            var httpClient = new HttpClient();
            string apiUrl = $"{WebHookUrl}/{GlobalTelegramSettings.BASE_MESSAGE}/{GlobalTelegramSettings.RE_UPDATE_MESSAGE}?userId={userId}&password={GlobalTelegramSettings.API_PASSWORD}";
            HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
        }

        private async Task WaitInitTask()
        {
            while (!IsStarted)
            {
                await Task.Yield();
            }
        }

        /// <summary>
        /// Получает текущие действующие команды у бота с сервера
        /// </summary>
        /// <returns></returns>
        public async Task<BotCommand[]> GetExistCommands()
        {
            await WaitInitTask();
            return await _client.GetMyCommandsAsync();
        }

        /// <summary>
        /// Устанавливает новые команды
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        public async Task SetNewCommands(IEnumerable<BotCommand> commands)
        {
            await WaitInitTask();
            try
            {
                await _client.SetMyCommandsAsync(commands);
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to set TG bot commands. {ex}");
            }
        }

        /// <summary>
        /// Отправляет текстовое сообщение
        /// </summary>
        /// <param name="chatId">Id чата</param>
        /// <param name="message">Текст сообщения</param>
        /// <param name="replyMarkup">Клавиатура ответа</param>
        /// <param name="isShowPreview">Показывать превью ссылок</param>
        /// <param name="parseMode">Тип парсинга сообщения</param>
        /// <returns></returns>
        public async Task<Message> SendTextMessage(long chatId, string message, IReplyMarkup replyMarkup = null, bool isShowPreview = true, ParseMode? parseMode = null)
        {
            try
            {
                await WaitInitTask();
                return await _client.SendTextMessageAsync(chatId, message, replyMarkup: replyMarkup, disableWebPagePreview: !isShowPreview, parseMode: parseMode);
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to send text TG bot. {ex}");
                throw ex;
            }
        }

        /// <summary>
        /// Отредактировать сообщение
        /// </summary>
        /// <param name="chatId">Чат</param>
        /// <param name="messageId">Id сообщения</param>
        /// <param name="message">Текст сообщения</param>
        /// <param name="replyMarkup">Клавиатура ответа</param>
        /// <param name="isShowPreview">Показывать превью ссылок</param>
        /// <param name="parseMode">Тип парсинга сообщения</param>
        /// <returns></returns>
        public async Task<Message> EditTextMessage(Chat chatId, int messageId, string message, InlineKeyboardMarkup replyMarkup = null, bool isShowPreview = true, ParseMode? parseMode = null)
        {
            try
            {
                await WaitInitTask();
                return await _client.EditMessageTextAsync(chatId, messageId, message, replyMarkup: replyMarkup, disableWebPagePreview: !isShowPreview, parseMode: parseMode);
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to edit message TG bot. {ex}");
                throw ex;
            }
        }

        /// <summary>
        /// Отправляет фото сообщение
        /// </summary>
        /// <param name="chatId">Id чата</param>
        /// <param name="image">Фото</param>
        /// <param name="replyMarkup">Клавиатура ответа</param>
        /// <param name="caption">Описание фото</param>
        /// <param name="parseMode">Тип парсинга сообщения</param>
        /// <returns></returns>
        public async Task<Message> SendPhotoMessage(long chatId, InputFileStream image, string caption = null, IReplyMarkup replyMarkup = null, ParseMode? parseMode = null)
        {
            try
            {
                await WaitInitTask();
                return await _client.SendPhotoAsync(chatId, image, replyMarkup: replyMarkup, caption: caption, parseMode: parseMode);
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to send photo TG bot. {ex}");
                throw ex;
            }
        }

        public async Task<Message> SendPhotoMessage(long chatId, InputFile photo, string caption = null, IReplyMarkup replyMarkup = null, ParseMode? parseMode = null)
        {
            try
            {
                await WaitInitTask();
                return await _client.SendPhotoAsync(chatId, photo, replyMarkup: replyMarkup, caption: caption, parseMode: parseMode);
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to send photo TG bot. {ex}");
                throw ex;
            }
        }

        /// <summary>
        /// Отправляет группу файлов медиа формата
        /// </summary>
        /// <param name="chatId"></param>
        /// <param name="album"></param>
        /// <param name="caption"></param>
        /// <param name="parseMode"></param>
        /// <returns></returns>
        public async Task<Message[]> SendMediaGroupMessage(long chatId, IEnumerable<InputMedia> album, string caption = null, ParseMode? parseMode = null)
        {
            try
            {
                await WaitInitTask();
                var media = album.FirstOrDefault();
                if (media != null && !caption.IsNull())
                {
                    media.ParseMode = parseMode;
                    media.Caption = caption;
                }
                return await _client.SendMediaGroupAsync(chatId, (IEnumerable<IAlbumInputMedia>)album.Select(x => x as IAlbumInputMedia));
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to send media group TG bot. {ex}");
                throw ex;
            }
        }


        /// <summary>
        /// Отправляет видео
        /// </summary>
        /// <param name="chatId"></param>
        /// <param name="video"></param>
        /// <param name="caption"></param>
        /// <param name="replyMarkup"></param>
        /// <param name="parseMode"></param>
        /// <param name="duration"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="supportsStreaming"></param>
        /// <param name="thumbnail"></param>
        /// <returns></returns>
        public async Task<Message> SendVideoMessage(long chatId, InputFileStream video, string caption = null, IReplyMarkup replyMarkup = null, ParseMode? parseMode = null,
            int? duration = null, int? width = null, int? height = null, bool? supportsStreaming = null, InputFileStream? thumbnail = null)
        {
            try
            {
                await WaitInitTask();
                return await _client.SendVideoAsync(chatId, video, replyMarkup: replyMarkup, caption: caption, parseMode: parseMode, duration: duration, width: width, height: height, supportsStreaming: supportsStreaming, thumbnail: thumbnail);
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to send video TG bot. {ex}");
                throw ex;
            }
        }

        public async Task<Message> SendVideoMessage(long chatId, InputFile video, string caption = null, IReplyMarkup replyMarkup = null, ParseMode? parseMode = null,
            int? duration = null, int? width = null, int? height = null, bool? supportsStreaming = null, InputFile? thumbnail = null)
        {
            try
            {
                await WaitInitTask();
                return await _client.SendVideoAsync(chatId, video, replyMarkup: replyMarkup, caption: caption, parseMode: parseMode, duration: duration, width: width, height: height, supportsStreaming: supportsStreaming, thumbnail: thumbnail);
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to send video TG bot. {ex}");
                throw ex;
            }
        }

        /// <summary>
        /// Отправляет видео-кружок
        /// </summary>
        /// <param name="chatId"></param>
        /// <param name="video"></param>
        /// <param name="caption"></param>
        /// <param name="replyMarkup"></param>
        /// <param name="duration"></param>
        /// <param name="length"></param>
        /// <param name="thumbnail"></param>
        /// <returns></returns>
        public async Task<Message> SendVideoNoteMessage(long chatId, InputFileStream video, string caption = null, IReplyMarkup replyMarkup = null,
            int? duration = null, int? length = null, InputFileStream? thumbnail = null)
        {
            try
            {
                await WaitInitTask();
                return await _client.SendVideoNoteAsync(chatId, video, replyMarkup: replyMarkup, duration: duration, length: length, thumbnail: thumbnail);
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to send video note TG bot. {ex}");
                throw ex;
            }
        }


        public async Task<Message> SendVideoNoteMessage(long chatId, InputFile video, string caption = null, IReplyMarkup replyMarkup = null,
            int? duration = null, int? length = null, InputFile? thumbnail = null)
        {
            try
            {
                await WaitInitTask();
                return await _client.SendVideoNoteAsync(chatId, video, replyMarkup: replyMarkup, duration: duration, length: length, thumbnail: thumbnail);
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to send video note TG bot. {ex}");
                throw ex;
            }
        }

        /// <summary>
        /// Запрос фотографий профиля
        /// </summary>
        /// <param name="chatId"></param>
        /// <returns></returns>
        public async Task<UserProfilePhotos> GetUserProfilePhoto(long chatId)
        {
            try
            {
                await WaitInitTask();
                return await _client.GetUserProfilePhotosAsync(chatId);
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to get photo profile TG bot. {ex}");
                throw ex;
            }
        }

        /// <summary>
        /// Получает файл по его Id
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public async Task<Telegram.Bot.Types.File> GetFileById(string fileId)
        {
            try
            {
                await WaitInitTask();
                return await _client.GetFileAsync(fileId);
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to get file TG bot. {ex}");
                throw ex;
            }
        }

        /// <summary>
        /// Скачать и сохранить файл
        /// </summary>
        /// <param name="fileId"></param>
        /// <param name="savePath"></param>
        /// <returns></returns>
        public async Task<bool> DownloadAndSaveFileById(string fileId, string savePath = "")
        {
            try
            {
                await WaitInitTask();
                var file = await _client.GetFileAsync(fileId);

                if (file != null)
                {
                    string fileName = $"{file.FileId}_{file.FilePath.Split('/').Last()}";
                    string localFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, savePath, fileName);

                    Directory.CreateDirectory(Path.GetDirectoryName(localFilePath));

                    using (Stream localFileStream = new FileStream(localFilePath, FileMode.Create))
                    {
                        await _client.DownloadFileAsync(file.FilePath, localFileStream);
                    }

                    if (System.IO.File.Exists(localFilePath))
                    {
                        return true;
                    }
                    else
                    {
                        Logger.Error($"File not found after saving: {localFilePath}");
                        return false;
                    }
                }
                else
                {
                    Logger.Error($"File not found for fileId: {fileId}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to download and save file TG bot. {ex}");
                throw ex;
            }
        }

        /// <summary>
        /// Скачать и вернуть массив байтов картинки
        /// </summary>
        /// <param name="photoId"></param>
        /// <returns></returns>
        public async Task<byte[]?> DownloadPhotoById(string photoId)
        {
            try
            {
                await WaitInitTask();
                var file = await _client.GetFileAsync(photoId);

                if (file != null)
                {
                    string fileName = $"{file.FileId}_{file.FilePath.Split('/').Last()}";
                    string localFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

                    Directory.CreateDirectory(Path.GetDirectoryName(localFilePath));

                    using (Stream localFileStream = new FileStream(localFilePath, FileMode.Create))
                    {
                        await _client.DownloadFileAsync(file.FilePath, localFileStream);
                    }

                    if (System.IO.File.Exists(localFilePath))
                    {
                        byte[] imgBytes = Support.Support.GetImageBytesByFilePath(localFilePath);
                        System.IO.File.Delete(localFilePath);
                        return imgBytes;
                    }
                    else
                    {
                        Logger.Error($"File not found after saving: {localFilePath}");
                        return null;
                    }
                }
                else
                {
                    Logger.Error($"File not found for fileId: {photoId}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to get bytes photo TG bot. {ex}");
                throw ex;
            }
        }

        /// <summary>
        /// Скачать и вернуть массив байтов видео
        /// </summary>
        /// <param name="videoId"></param>
        /// <returns></returns>
        public async Task<byte[]?> DownloadVideoById(string videoId)
        {
            try
            {
                await WaitInitTask();
                var file = await _client.GetFileAsync(videoId);

                if (file != null)
                {
                    string fileName = $"{file.FileId}_{file.FilePath.Split('/').Last()}";
                    string localFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

                    Directory.CreateDirectory(Path.GetDirectoryName(localFilePath));

                    using (Stream localFileStream = new FileStream(localFilePath, FileMode.Create))
                    {
                        await _client.DownloadFileAsync(file.FilePath, localFileStream);
                    }

                    if (System.IO.File.Exists(localFilePath))
                    {
                        byte[] imgBytes = Support.Support.GetVideoBytesByFilePath(localFilePath);
                        System.IO.File.Delete(localFilePath);
                        return imgBytes;
                    }
                    else
                    {
                        Logger.Error($"File not found after saving: {localFilePath}");
                        return null;
                    }
                }
                else
                {
                    Logger.Error($"File not found for fileId: {videoId}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to get bytes photo TG bot. {ex}");
                throw ex;
            }
        }

    }
}
