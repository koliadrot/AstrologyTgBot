namespace Service.Core.TelegramBot.FindCondition
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
    /// Условие любовного письма
    /// </summary>
    public class LoveLetterCondition : ICondition, IUpdater
    {
        private bool _isDone = false;
        public bool IsDone => _isDone;

        private bool isStarted = false;

        public bool IsStarted => isStarted;

        public bool IsCancel { get; private set; } = false;

        private object _info;
        public object Info => _info;

        public bool IsIgnoredNextCondition => true;

        public bool IsCanPass { get; set; } = false;

        public int Order { get; set; } = -1;

        private List<ICondition> _conditions = new List<ICondition>();
        public List<ICondition> Conditions => _conditions;

        private Func<Task> onSendLoveLetterMatch = async () => { };

        private DataManager _dataManager;
        private IListener _listener;
        private Dictionary<string, string> _messages;

        private const int LIMIT_MEDIA_COUNT = 1;

        public LoveLetterCondition(DataManager dataManager, IListener listener, Func<Task> onSendLoveLetterMatchAction)
        {
            _dataManager = dataManager;
            _messages = _dataManager.GetData<CommandExecutor>().Messages;
            _listener = listener;
            onSendLoveLetterMatch = onSendLoveLetterMatchAction;
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

                inlineKeyboardButtons.Add(new KeyboardButton(_messages[ReplyButton.BACK]));
                replyKeyboard.ResizeKeyboard = true;
                replyKeyboard.IsPersistent = true;

                await _dataManager.GetData<TelegramBotManager>().SendTextMessage(chatId, Get.ReplaceKeysInText(_messages[MessageKey.WRITE_LOVE_LETTER], new Dictionary<string, string>() { { Promt.DURATION, TelegramSupport.LIMIT_VIDEO_DURATION.ToString() } }), replyMarkup: replyKeyboard);
                isStarted = true;
                _listener.StartListen(this);
                _dataManager.GetData<ILogger>().Debug($"User:{userId}; Condition:{nameof(LoveLetterCondition)} -  Start condition");
            }
        }

        public async Task<bool> CheckCondition(Update update)
        {
            long chatId = Get.GetChatId(update);
            long userId = Get.GetUserId(update);
            string messageText = Get.GetText(update);
            var callbackQuery = update.CallbackQuery;
            if (callbackQuery == null)
            {
                if (update.Message.VideoNote != null)
                {
                    var mediaGroupId = update.Message.MediaGroupId;
                    if (mediaGroupId.IsNull())
                    {
                        var videoNote = update.Message.VideoNote;
                        if (videoNote.Duration <= TelegramSupport.LIMIT_VIDEO_DURATION)
                        {
                            var videoBytes = await _dataManager.GetData<TelegramBotManager>().DownloadVideoById(videoNote.FileId);
                            var loveVideoNote = new ClientMatchUncheckedVideoNoteInfoViewModel()
                            {
                                FileId = videoNote.FileId,
                                FileUniqueId = videoNote.FileUniqueId,
                                FileSize = videoNote.FileSize,
                                Length = videoNote.Length,
                                Duration = videoNote.Duration,
                                VideoNote = videoBytes
                            };
                            if (videoNote.Thumbnail != null)
                            {
                                var photoThumbnailBytes = await _dataManager.GetData<TelegramBotManager>().DownloadPhotoById(videoNote.Thumbnail.FileId);
                                loveVideoNote.ThumbnailFileId = videoNote.Thumbnail.FileId;
                                loveVideoNote.ThumbnailFileUniqueId = videoNote.Thumbnail.FileUniqueId;
                                loveVideoNote.ThumbnailFileSize = videoNote.Thumbnail.FileSize;
                                loveVideoNote.ThumbnailWidth = videoNote.Thumbnail.Width;
                                loveVideoNote.ThumbnailHeight = videoNote.Thumbnail.Height;
                                loveVideoNote.Thumbnail = photoThumbnailBytes;
                            }
                            _info = loveVideoNote;
                            _isDone = true;
                            await onSendLoveLetterMatch();
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
                else if (update.Message.Video != null)
                {
                    var mediaGroupId = update.Message.MediaGroupId;
                    if (mediaGroupId.IsNull())
                    {
                        var video = update.Message.Video;
                        if (video.Duration <= TelegramSupport.LIMIT_VIDEO_DURATION)
                        {
                            var videoBytes = await _dataManager.GetData<TelegramBotManager>().DownloadVideoById(video.FileId);
                            var loveVideo = new ClientMatchUncheckedVideoInfoViewModel()
                            {
                                FileId = video.FileId,
                                FileUniqueId = video.FileUniqueId,
                                FileSize = video.FileSize,
                                Width = video.Width,
                                Height = video.Height,
                                Duration = video.Duration,
                                MimeType = video.MimeType,
                                Video = videoBytes
                            };
                            if (video.Thumbnail != null)
                            {
                                var photoThumbnailBytes = await _dataManager.GetData<TelegramBotManager>().DownloadPhotoById(video.Thumbnail.FileId);
                                loveVideo.ThumbnailFileId = video.Thumbnail.FileId;
                                loveVideo.ThumbnailFileUniqueId = video.Thumbnail.FileUniqueId;
                                loveVideo.ThumbnailFileSize = video.Thumbnail.FileSize;
                                loveVideo.ThumbnailWidth = video.Thumbnail.Width;
                                loveVideo.ThumbnailHeight = video.Thumbnail.Height;
                                loveVideo.Thumbnail = photoThumbnailBytes;
                            }
                            _info = loveVideo;
                            _isDone = true;
                            await onSendLoveLetterMatch();
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
                else if (_messages[ReplyButton.BACK] == messageText)
                {
                    _isDone = true;
                    IsCancel = true;
                    return _isDone;
                }
                else if (!messageText.IsNull())
                {
                    _info = messageText;
                    await onSendLoveLetterMatch();
                    _isDone = true;
                }
                else
                {
                    await _dataManager.GetData<TelegramBotManager>().SendTextMessage(chatId, _messages[MessageKey.WRONG_MEDIA_INFO]);
                    _dataManager.GetData<ILogger>().Debug($"User:{userId}; Condition:{nameof(LoveLetterCondition)} -  Wrong format {messageText}");
                }
            }
            return _isDone;
        }

        public Task GetUpdate(Update update) => null;
    }
}
