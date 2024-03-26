using Data.Core;
using Hangfire;
using Newtonsoft.Json;
using NLog;
using Service.Core.TelegramBot.Messages;
using Service.Enums;
using Service.Extensions;
using Service.Support;
using System.Net;
using System.Text;
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
                    string hook = $"{WebHookUrl}/{GlobalTelegramSettings.BASE}/{GlobalTelegramSettings.UPDATE}";
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
            string apiUrl = $"{WebHookUrl}/{GlobalTelegramSettings.BASE}/{GlobalTelegramSettings.REUPDATE}?userId={userId}&password={GlobalTelegramSettings.API_PASSWORD}";
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
                if (commands.Any())
                {
                    await _client.SetMyCommandsAsync(commands);
                }
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
        public async Task<Message?> SendTextMessage(long chatId, string message, IReplyMarkup? replyMarkup = null, bool isShowPreview = true, ParseMode? parseMode = null)
        {
            try
            {
                await WaitInitTask();
                return !message.IsNull()
                    ? await _client.SendTextMessageAsync(chatId, message, replyMarkup: replyMarkup, disableWebPagePreview: !isShowPreview, parseMode: parseMode)
                    : null;
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to send text TG bot. {ex}");
                throw ex;
            }
        }

        /// <summary>
        /// Отправляет текстовое сообщение
        /// </summary>
        /// <param name="textMessage">Модель текстового сообщения</param>
        /// <returns></returns>
        public async Task<Message?> SendTextMessage(TextMessage textMessage) => await SendTextMessage(textMessage.СhatId, textMessage.Text, textMessage.ReplyMarkup, textMessage.IsShowPreview, textMessage.ParseMode);

        /// <summary>
        /// Отредактировать текстовое сообщение
        /// </summary>
        /// <param name="chatId">Чат</param>
        /// <param name="messageId">Id сообщения</param>
        /// <param name="message">Текст сообщения</param>
        /// <param name="replyMarkup">Клавиатура ответа</param>
        /// <param name="isShowPreview">Показывать превью ссылок</param>
        /// <param name="parseMode">Тип парсинга сообщения</param>
        /// <returns></returns>
        public async Task<Message?> EditTextMessage(Chat chatId, int messageId, string message, InlineKeyboardMarkup replyMarkup = null, bool isShowPreview = true, ParseMode? parseMode = null)
        {
            try
            {
                await WaitInitTask();
                return !message.IsNull() && chatId != null && messageId != 0
                    ? await _client.EditMessageTextAsync(chatId, messageId, message, replyMarkup: replyMarkup, disableWebPagePreview: !isShowPreview, parseMode: parseMode)
                    : null;
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to edit message TG bot. {ex}");
                throw ex;
            }
        }

        /// <summary>
        /// Отредактировать текстовое сообщение
        /// </summary>
        /// <param name="editTextMessage">Модель редактироания текстового сообщения</param>
        /// <returns></returns>
        public async Task<Message?> EditTextMessage(EditTextMessage editTextMessage) => await EditTextMessage(editTextMessage.Сhat, editTextMessage.MessageId, editTextMessage.Text, editTextMessage.ReplyMarkup, editTextMessage.IsShowPreview, editTextMessage.ParseMode);

        /// <summary>
        /// Отредактировать описание -фото,-видео файлов
        /// </summary>
        /// <param name="chatId">Чат</param>
        /// <param name="messageId">Id сообщения</param>
        /// <param name="caption">Описание</param>
        /// <param name="replyMarkup">Клавиатура ответа</param>
        /// <param name="parseMode">Тип парсинга сообщения</param>
        /// <returns></returns>
        public async Task<Message?> EditCaptionMessage(Chat chatId, int messageId, string caption, InlineKeyboardMarkup replyMarkup = null, ParseMode? parseMode = null)
        {
            try
            {
                await WaitInitTask();
                return !caption.IsNull()
                    ? await _client.EditMessageCaptionAsync(chatId, messageId, caption, replyMarkup: replyMarkup, parseMode: parseMode)
                    : null;
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to edit message TG bot. {ex}");
                throw ex;
            }
        }

        /// <summary>
        /// Отредактировать описание -фото,-видео файлов
        /// </summary>
        /// <param name="editMediaMessage">Модель редактируемого медиа сообщения</param>
        /// <returns></returns>
        public async Task<Message?> EditCaptionMessage(EditMediaMessage editMediaMessage) => await EditCaptionMessage(editMediaMessage.Сhat, editMediaMessage.MessageId, editMediaMessage.Text, editMediaMessage.ReplyMarkup, editMediaMessage.ParseMode);

        /// <summary>
        /// Отправляет фото сообщение
        /// </summary>
        /// <param name="chatId">Id чата</param>
        /// <param name="photo">Фото</param>
        /// <param name="replyMarkup">Клавиатура ответа</param>
        /// <param name="caption">Описание фото</param>
        /// <param name="parseMode">Тип парсинга сообщения</param>
        /// <returns></returns>
        public async Task<Message?> SendPhotoMessage(long chatId, InputFileStream photo, string caption = null, IReplyMarkup replyMarkup = null, ParseMode? parseMode = null)
        {
            try
            {
                await WaitInitTask();
                return photo != null
                    ? await _client.SendPhotoAsync(chatId, photo, replyMarkup: replyMarkup, caption: caption, parseMode: parseMode)
                    : null;
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to send photo TG bot. {ex}");
                throw ex;
            }
        }

        /// <summary>
        /// Отправляет фото сообщение
        /// </summary>
        /// <param name="chatId">Id чата</param>
        /// <param name="photo">Фото</param>
        /// <param name="replyMarkup">Клавиатура ответа</param>
        /// <param name="caption">Описание фото</param>
        /// <param name="parseMode">Тип парсинга сообщения</param>
        public async Task<Message?> SendPhotoMessage(long chatId, InputFile photo, string caption = null, IReplyMarkup replyMarkup = null, ParseMode? parseMode = null)
        {
            try
            {
                await WaitInitTask();
                return photo != null
                    ? await _client.SendPhotoAsync(chatId, photo, replyMarkup: replyMarkup, caption: caption, parseMode: parseMode)
                    : null;
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to send photo TG bot. {ex}");
                throw ex;
            }
        }

        /// <summary>
        /// Отправляет видео
        /// </summary>
        /// <param name="chatId">Id чата</param>
        /// <param name="video">Видео</param>
        /// <param name="caption">Описание видео</param>
        /// <param name="replyMarkup">Клавиатура ответа</param>
        /// <param name="parseMode">Тип парсинга сообщения</param>
        /// <param name="duration">Длительность</param>
        /// <param name="width">Ширина</param>
        /// <param name="height">Высота</param>
        /// <param name="supportsStreaming">Поддержка трансляции по мере просмотра</param>
        /// <param name="thumbnail">Обложка</param>
        /// <returns></returns>
        public async Task<Message?> SendVideoMessage(long chatId, InputFileStream video, string caption = null, IReplyMarkup replyMarkup = null, ParseMode? parseMode = null,
            int? duration = null, int? width = null, int? height = null, bool? supportsStreaming = null, InputFileStream? thumbnail = null)
        {
            try
            {
                await WaitInitTask();
                return video != null
                    ? await _client.SendVideoAsync(chatId, video, replyMarkup: replyMarkup, caption: caption, parseMode: parseMode, duration: duration, width: width, height: height, supportsStreaming: supportsStreaming, thumbnail: thumbnail)
                    : null;
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to send video TG bot. {ex}");
                throw ex;
            }
        }

        /// <summary>
        /// Отправляет видео
        /// </summary>
        /// <param name="chatId">Id чата</param>
        /// <param name="video">Видео</param>
        /// <param name="caption">Описание видео</param>
        /// <param name="replyMarkup">Клавиатура ответа</param>
        /// <param name="parseMode">Тип парсинга сообщения</param>
        /// <param name="duration">Длительность</param>
        /// <param name="width">Ширина</param>
        /// <param name="height">Высота</param>
        /// <param name="supportsStreaming">Поддержка трансляции по мере просмотра</param>
        /// <param name="thumbnail">Обложка</param>
        /// <returns></returns>
        public async Task<Message?> SendVideoMessage(long chatId, InputFile video, string caption = null, IReplyMarkup replyMarkup = null, ParseMode? parseMode = null,
            int? duration = null, int? width = null, int? height = null, bool? supportsStreaming = null, InputFile? thumbnail = null)
        {
            try
            {
                await WaitInitTask();
                return video != null
                    ? await _client.SendVideoAsync(chatId, video, replyMarkup: replyMarkup, caption: caption, parseMode: parseMode, duration: duration, width: width, height: height, supportsStreaming: supportsStreaming, thumbnail: thumbnail)
                    : null;
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
        /// <param name="chatId">Id чата</param>
        /// <param name="video">Видео</param>
        /// <param name="caption">Описание видео</param>
        /// <param name="replyMarkup">Клавиатура ответа</param>
        /// <param name="parseMode">Тип парсинга сообщения</param>
        /// <param name="duration">Длительность</param>
        /// <param name="thumbnail">Обложка</param>
        /// <returns></returns>
        public async Task<Message?> SendVideoNoteMessage(long chatId, InputFileStream video, string caption = null, IReplyMarkup replyMarkup = null, ParseMode? parseMode = null,
            int? duration = null, int? length = null, InputFileStream? thumbnail = null)
        {
            try
            {
                await WaitInitTask();
                Message message = null;
                if (video != null)
                {
                    message = await _client.SendVideoNoteAsync(chatId, video, replyMarkup: replyMarkup, duration: duration, length: length, thumbnail: thumbnail);
                }
                else if (!caption.IsNull())
                {
                    message = await SendTextMessage(chatId, caption, replyMarkup: replyMarkup, parseMode: parseMode);
                }
                return message;
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to send video note TG bot. {ex}");
                throw ex;
            }
        }

        /// <summary>
        /// Отправляет видео-кружок
        /// </summary>
        /// <param name="chatId">Id чата</param>
        /// <param name="video">Видео</param>
        /// <param name="caption">Описание видео</param>
        /// <param name="replyMarkup">Клавиатура ответа</param>
        /// <param name="parseMode">Тип парсинга сообщения</param>
        /// <param name="duration">Длительность</param>
        /// <param name="thumbnail">Обложка</param>
        /// <returns></returns>
        public async Task<Message?> SendVideoNoteMessage(long chatId, InputFile video, string caption = null, IReplyMarkup replyMarkup = null, ParseMode? parseMode = null,
            int? duration = null, int? length = null, InputFile? thumbnail = null)
        {
            try
            {
                await WaitInitTask();
                Message message = null;
                if (video != null)
                {
                    message = await _client.SendVideoNoteAsync(chatId, video, replyMarkup: !caption.IsNull() ? null : replyMarkup, duration: duration, length: length, thumbnail: thumbnail);
                }
                if (!caption.IsNull())
                {
                    message = await SendTextMessage(chatId, caption, replyMarkup: replyMarkup, parseMode: parseMode);
                }
                return message;
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to send video note TG bot. {ex}");
                throw ex;
            }
        }

        /// <summary>
        /// Отправляет группу файлов медиа формата
        /// </summary>
        /// <param name="chatId">Id чата</param>
        /// <param name="album">Массив медиа файлов</param>
        /// <param name="caption">Описание видео</param>
        /// <param name="parseMode">Тип парсинга сообщения</param>
        /// <returns></returns>
        public async Task<Message[]> SendMediaGroupMessage(long chatId, IEnumerable<InputMediaCustom> album, string caption = null, ParseMode? parseMode = null)
        {
            try
            {
                await WaitInitTask();
                List<InputMedia> inputMedias = new List<InputMedia>();
                foreach (var item in album.Where(x => x.MediaType == MediaType.Video || x.MediaType == MediaType.Photo))
                {
                    if (item.MediaType == MediaType.Video)
                    {
                        inputMedias.Add(new InputMediaVideo(item.Media));
                    }
                    else if (item.MediaType == MediaType.Photo)
                    {
                        inputMedias.Add(new InputMediaPhoto(item.Media));
                    }
                }
                var media = inputMedias.FirstOrDefault();
                bool isCaptionSended = false;
                if (media != null && !caption.IsNull())
                {
                    media.ParseMode = parseMode;
                    media.Caption = caption;
                    isCaptionSended = true;

                }
                foreach (var item in album.Where(x => x.MediaType == MediaType.VideoNote))
                {
                    if (isCaptionSended)
                    {
                        await SendMediaMessage(chatId, item);
                    }
                    else
                    {
                        await SendMediaMessage(chatId, item, caption);
                        isCaptionSended = true;
                    }
                }
                return await _client.SendMediaGroupAsync(chatId, (IEnumerable<IAlbumInputMedia>)inputMedias.Select(x => x as IAlbumInputMedia));
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to send media group TG bot. {ex}");
                throw ex;
            }
        }

        /// <summary>
        /// Отправка медиа сообщения (видео, фото)
        /// </summary>
        /// <param name="chatId">Id чата</param>
        /// <param name="file">Медиа файл</param>
        /// <param name="caption">Описание видео</param>
        /// <param name="replyMarkup"></param>
        /// <param name="parseMode">Тип парсинга сообщения</param>
        /// <returns></returns>
        public async Task<Message?> SendMediaMessage(long chatId, InputMediaCustom file, string caption = null, IReplyMarkup? replyMarkup = null, ParseMode? parseMode = null)
        {
            try
            {
                await WaitInitTask();

                if (file != null)
                {
                    if (file.MediaType == MediaType.Photo)
                    {
                        return await SendPhotoMessage(chatId, file.Media, caption, replyMarkup: replyMarkup, parseMode: parseMode);
                    }
                    else if (file.MediaType == MediaType.Video)
                    {
                        return await SendVideoMessage(chatId, file.Media, caption, replyMarkup: replyMarkup, parseMode: parseMode);
                    }
                    else if (file.MediaType == MediaType.VideoNote)
                    {
                        return await SendVideoNoteMessage(chatId, file.Media, caption, replyMarkup: replyMarkup, parseMode: parseMode);
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to send media TG bot. {ex}");
                throw ex;
            }
        }

        /// <summary>
        /// Отправка медиа сообщения (видео, фото)
        /// </summary>
        /// <param name="mediaMessage">Медиа сообщение</param>
        /// <returns></returns>
        public async Task<Message?> SendMediaMessage(MediaMessage mediaMessage) => await SendMediaMessage(mediaMessage.СhatId, mediaMessage.File, mediaMessage.Caption, mediaMessage.ReplyMarkup, mediaMessage.ParseMode);

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
                        byte[] imgBytes = Media.GetImageBytesByFilePath(localFilePath);
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
                        byte[] imgBytes = Media.GetVideoBytesByFilePath(localFilePath);
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




    public class BackgroundJobMethods
    {
        public static void EnqueueSomeBackgroundJobMethod()
        {
            BackgroundJob.Enqueue(() => SomeBackgroundJobMethod());
        }

        public static void SomeBackgroundJobMethod()
        {
            string token = "702310666:AAHsHpmZnzrErB5oK-Jmag8A0JDsCcHy6O8";
            string Url = "https://api.telegram.org/bot" + token + "/sendMessage";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);

            string postData = string.Format("text={0}&chat_id={1}", "Провекра", 504519150);
            var data = Encoding.UTF8.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string contentResponse = null;

            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                contentResponse = streamReader.ReadToEnd();
            }

            var result = JsonConvert.DeserializeAnonymousType(contentResponse, new { ok = default(bool), result = new Telegram.Bot.Types.Message() });
        }
    }

}
