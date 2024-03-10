namespace Service.Core.TelegramBot.RegisterCondition
{
    using NLog;
    using Service.Abstract.TelegramBot;
    using Service.Extensions;
    using Service.Support;
    using Service.ViewModels;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Telegram.Bot.Types;
    using Telegram.Bot.Types.ReplyMarkups;

    /// <summary>
    /// Аватар анкеты
    /// </summary>
    public class AvatarCondition : BaseRegisterCondition, ICondition
    {
        private bool isDone;
        public bool IsDone => isDone;

        private bool isStarted = false;

        public bool IsStarted => isStarted;

        public bool IsCancel { get; private set; } = false;

        public object Info => _media;

        public bool IsIgnoredNextCondition => false;

        public bool IsCanPass { get; set; } = true;

        public int Order { get; set; } = -1;

        private List<ICondition> _conditions = new List<ICondition>();
        public List<ICondition> Conditions => _conditions;

        private DataManager _dataManager;
        private Dictionary<string, string> _messages = new Dictionary<string, string>();
        private ClientMediaInfoViewModel _media = new ClientMediaInfoViewModel();
        private bool _isOptional;

        private const float START_DELAY = 10f;
        private const float MAX_DELAY = 15f;
        private const int LIMIT_MEDIA_COUNT = 1;


        public AvatarCondition(DataManager dataManager, bool isOptional = true)
        {
            _dataManager = dataManager;
            _messages = _dataManager.GetData<CommandExecutor>().Messages;
            _isOptional = isOptional;
        }

        public async Task Execute(Update update)
        {
            if (!isStarted)
            {
                long chatId = Get.GetChatId(update);
                long userId = Get.GetUserId(update);
                List<KeyboardButton> inlineKeyboardButtons = new List<KeyboardButton>();
                ReplyKeyboardMarkup replyKeyboard = new ReplyKeyboardMarkup(new[]
                {
                    inlineKeyboardButtons
                });
                replyKeyboard.ResizeKeyboard = true;
                replyKeyboard.OneTimeKeyboard = true;
                if (_isOptional && IsCanPass)
                {
                    inlineKeyboardButtons.Add(new KeyboardButton(_messages[ReplyButton.SKIP]));
                }

                var photos = await _dataManager.GetData<TelegramBotManager>().GetUserProfilePhoto(userId);
                if (photos != null && photos.TotalCount > 0)
                {
                    inlineKeyboardButtons.Add(new KeyboardButton(_messages[ReplyButton.Avatar]));
                }

                await _dataManager.GetData<TelegramBotManager>().SendTextMessage(chatId, Get.ReplaceKeysInText(_messages[MessageKey.ENTER_AVATAR], new Dictionary<string, string>() { { Promt.DURATION, TelegramSupport.LIMIT_VIDEO_DURATION.ToString() } }), replyKeyboard);
                isStarted = true;
                _dataManager.GetData<ILogger>()?.Debug($"User:{userId}; Condition:{nameof(AvatarCondition)} -  Start condition");
            }
        }

        public async Task<bool> CheckCondition(Update update)
        {
            long chatId = Get.GetChatId(update);
            long userId = Get.GetUserId(update);
            string messageText = Get.GetText(update);
            if (messageText == _messages[ReplyButton.SKIP] && IsCanPass)
            {
                isDone = true;
                _dataManager.GetData<ILogger>().Debug($"User:{userId}; Condition:{nameof(AvatarCondition)} -  Skip");
            }
            else if (messageText == _messages[ReplyButton.Avatar])
            {
                var photos = await _dataManager.GetData<TelegramBotManager>().GetUserProfilePhoto(userId);
                if (photos != null && photos.TotalCount > 0)
                {
                    var selectPhoto = photos.Photos[0].LastOrDefault();
                    var photoBytes = await _dataManager.GetData<TelegramBotManager>().DownloadPhotoById(selectPhoto.FileId);
                    var viewModel = new ClientPhotoInfoViewModel()
                    {
                        FileId = selectPhoto.FileId,
                        FileUniqueId = selectPhoto.FileUniqueId,
                        FileSize = selectPhoto.FileSize,
                        Width = selectPhoto.Width,
                        Height = selectPhoto.Height,
                        Photo = photoBytes,
                        IsAvatar = true
                    };
                    _media.ClientPhotoInfos.Add(viewModel);
                    isDone = true;
                }
                else
                {
                    await _dataManager.GetData<TelegramBotManager>().SendTextMessage(chatId, Get.ReplaceKeysInText(_messages[MessageKey.EMPTY_AVATAR_INFO], new Dictionary<string, string>() { { Promt.DURATION, TelegramSupport.LIMIT_VIDEO_DURATION.ToString() } }));
                }
            }
            else if (update.Message.Photo != null)
            {
                var mediaGroupId = update.Message.MediaGroupId;
                if (mediaGroupId.IsNull())
                {
                    var selectPhoto = update.Message.Photo.LastOrDefault();
                    var photoBytes = await _dataManager.GetData<TelegramBotManager>().DownloadPhotoById(selectPhoto.FileId);
                    var viewModel = new ClientPhotoInfoViewModel()
                    {
                        FileId = selectPhoto.FileId,
                        FileUniqueId = selectPhoto.FileUniqueId,
                        FileSize = selectPhoto.FileSize,
                        Width = selectPhoto.Width,
                        Height = selectPhoto.Height,
                        Photo = photoBytes,
                        IsAvatar = true
                    };
                    _media.ClientPhotoInfos.Add(viewModel);
                    isDone = true;
                }
                else
                {
                    await _dataManager.GetData<TelegramBotManager>().SendTextMessage(chatId, Get.ReplaceKeysInText(_messages[MessageKey.VERY_MANY_MEDIA_INFO], new Dictionary<string, string>() { { Promt.MEDIA, LIMIT_MEDIA_COUNT.ToString() } }));
                }
            }
            else if (update.Message.Video != null)
            {
                var mediaGroupId = update.Message.MediaGroupId;
                if (mediaGroupId.IsNull())
                {
                    var video = update.Message.Video;
                    if (video.Duration <= TelegramSupport.LIMIT_VIDEO_DURATION)
                    {
                        var videoBytes = await _dataManager.GetData<TelegramBotManager>().DownloadVideoById(video.FileId);
                        var viewModel = new ClientVideoInfoViewModel()
                        {
                            FileId = video.FileId,
                            FileUniqueId = video.FileUniqueId,
                            FileSize = video.FileSize,
                            Width = video.Width,
                            Height = video.Height,
                            Duration = video.Duration,
                            MimeType = video.MimeType,
                            Video = videoBytes,
                            IsAvatar = true
                        };
                        if (video.Thumbnail != null)
                        {
                            var photoThumbnailBytes = await _dataManager.GetData<TelegramBotManager>().DownloadPhotoById(video.Thumbnail.FileId);
                            viewModel.ThumbnailFileId = video.Thumbnail.FileId;
                            viewModel.ThumbnailFileUniqueId = video.Thumbnail.FileUniqueId;
                            viewModel.ThumbnailFileSize = video.Thumbnail.FileSize;
                            viewModel.ThumbnailWidth = video.Thumbnail.Width;
                            viewModel.ThumbnailHeight = video.Thumbnail.Height;
                            viewModel.Thumbnail = photoThumbnailBytes;
                        }

                        _media.ClientVideoInfos.Add(viewModel);
                        isDone = true;
                    }
                    else
                    {
                        await _dataManager.GetData<TelegramBotManager>().SendTextMessage(chatId, Get.ReplaceKeysInText(_messages[MessageKey.VERY_LONG_VIDEO], new Dictionary<string, string>() { { Promt.DURATION, TelegramSupport.LIMIT_VIDEO_DURATION.ToString() } }));
                    }

                }
                else
                {
                    await _dataManager.GetData<TelegramBotManager>().SendTextMessage(chatId, Get.ReplaceKeysInText(_messages[MessageKey.VERY_MANY_MEDIA_INFO], new Dictionary<string, string>() { { Promt.MEDIA, LIMIT_MEDIA_COUNT.ToString() } }));
                }
            }
            else if (update.Message.VideoNote != null)
            {
                var mediaGroupId = update.Message.MediaGroupId;
                if (mediaGroupId.IsNull())
                {
                    var videoNote = update.Message.VideoNote;
                    if (videoNote.Duration <= TelegramSupport.LIMIT_VIDEO_DURATION)
                    {
                        var videoNoteBytes = await _dataManager.GetData<TelegramBotManager>().DownloadVideoById(videoNote.FileId);
                        var viewModel = new ClientVideoNoteInfoViewModel()
                        {
                            FileId = videoNote.FileId,
                            FileUniqueId = videoNote.FileUniqueId,
                            FileSize = videoNote.FileSize,
                            Length = videoNote.Length,
                            Duration = videoNote.Duration,
                            VideoNote = videoNoteBytes,
                            IsAvatar = true
                        };
                        if (videoNote.Thumbnail != null)
                        {
                            var photoThumbnailBytes = await _dataManager.GetData<TelegramBotManager>().DownloadPhotoById(videoNote.Thumbnail.FileId);
                            viewModel.ThumbnailFileId = videoNote.Thumbnail.FileId;
                            viewModel.ThumbnailFileUniqueId = videoNote.Thumbnail.FileUniqueId;
                            viewModel.ThumbnailFileSize = videoNote.Thumbnail.FileSize;
                            viewModel.ThumbnailWidth = videoNote.Thumbnail.Width;
                            viewModel.ThumbnailHeight = videoNote.Thumbnail.Height;
                            viewModel.Thumbnail = photoThumbnailBytes;
                        }

                        _media.ClientVideoNoteInfos.Add(viewModel);
                        isDone = true;
                    }
                    else
                    {
                        await _dataManager.GetData<TelegramBotManager>().SendTextMessage(chatId, Get.ReplaceKeysInText(_messages[MessageKey.VERY_LONG_VIDEO], new Dictionary<string, string>() { { Promt.DURATION, TelegramSupport.LIMIT_VIDEO_DURATION.ToString() } }));
                    }

                }
                else
                {
                    await _dataManager.GetData<TelegramBotManager>().SendTextMessage(chatId, Get.ReplaceKeysInText(_messages[MessageKey.VERY_MANY_MEDIA_INFO], new Dictionary<string, string>() { { Promt.MEDIA, LIMIT_MEDIA_COUNT.ToString() } }));
                }
            }
            else
            {
                await _dataManager.GetData<TelegramBotManager>().SendTextMessage(chatId, _messages[MessageKey.WRONG_MEDIA_INFO]);
                _dataManager.GetData<ILogger>().Debug($"User:{userId}; Condition:{nameof(AvatarCondition)} -  Wrong format");
            }

            return isDone;
        }
    }
}

