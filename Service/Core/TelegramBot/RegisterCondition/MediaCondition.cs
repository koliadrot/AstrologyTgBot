namespace Service.Core.TelegramBot.RegisterCondition
{
    using NLog;
    using Service.Abstract.TelegramBot;
    using Service.Extensions;
    using Service.Support;
    using Service.ViewModels;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Timers;
    using Telegram.Bot.Types;
    using Telegram.Bot.Types.ReplyMarkups;

    /// <summary>
    /// Медиа файлы
    /// </summary>
    public class MediaCondition : BaseRegisterCondition, ICondition, IUpdater
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
        private IListener _listener;
        private Dictionary<string, string> _messages = new Dictionary<string, string>();
        private List<string> _supportMiniComands = new List<string>();
        private ClientMediaInfoViewModel _media = new ClientMediaInfoViewModel();
        private List<ClientPhotoInfoViewModel> _photos = new List<ClientPhotoInfoViewModel>();
        private List<ClientVideoInfoViewModel> _videos = new List<ClientVideoInfoViewModel>();
        private int invalidVideoCount = 0;
        private bool _isOptional;
        private Timer _timer;
        private float _currentDelay;
        private bool _isStartProcessing = false;

        private const float START_DELAY = 10f;
        private const float MAX_DELAY = 15f;
        private const int LIMIT_MEDIA_COUNT = 10;


        public MediaCondition(DataManager dataManager, IListener listener, bool isOptional = true)
        {
            _dataManager = dataManager;
            _messages = _dataManager.GetData<CommandExecutor>().Messages;
            _isOptional = isOptional;
            _listener = listener;
            _supportMiniComands.Add(_messages[ReplyButton.YES]);
            _supportMiniComands.Add(_messages[ReplyButton.NO]);
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
                await _dataManager.GetData<TelegramBotManager>().SendTextMessage(chatId, Get.ReplaceKeysInText(_messages[MessageKey.ENTER_MEDIA_INFO], new Dictionary<string, string>() { { Promt.DURATION, TelegramSupport.LIMIT_VIDEO_DURATION.ToString() } }), replyKeyboard);
                isStarted = true;
                invalidVideoCount = 0;
                SetDelay(START_DELAY, true);
                _isStartProcessing = false;
                _photos.Clear();
                _videos.Clear();
                _dataManager.GetData<ILogger>()?.Debug($"User:{userId}; Condition:{nameof(MediaCondition)} -  Start condition");
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
                _dataManager.GetData<ILogger>().Debug($"User:{userId}; Condition:{nameof(MediaCondition)} -  Skip");
            }
            else if (update.Message.Photo != null)
            {
                var mediaGroupId = update.Message.MediaGroupId;
                if (mediaGroupId.IsNull())
                {
                    var photo = update.Message.Photo.LastOrDefault();
                    var photoBytes = await _dataManager.GetData<TelegramBotManager>().DownloadPhotoById(photo.FileId);
                    _photos.Add(new ClientPhotoInfoViewModel()
                    {
                        FileId = photo.FileId,
                        FileUniqueId = photo.FileUniqueId,
                        FileSize = photo.FileSize,
                        Width = photo.Width,
                        Height = photo.Height,
                        //Photo = photoBytes
                    });
                    _media.ClientPhotoInfos = _photos;
                    isDone = true;
                }
                else
                {
                    var photo = update.Message.Photo.LastOrDefault();
                    var photoBytes = await _dataManager.GetData<TelegramBotManager>().DownloadPhotoById(photo.FileId);
                    _photos.Add(new ClientPhotoInfoViewModel()
                    {
                        FileId = photo.FileId,
                        FileUniqueId = photo.FileUniqueId,
                        MediaGroupId = mediaGroupId,
                        FileSize = photo.FileSize,
                        Width = photo.Width,
                        Height = photo.Height,
                        //Photo = photoBytes
                    });
                    await StartCheckerMediaGroup(update);
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
                            //Video = videoBytes
                        };
                        if (video.Thumbnail != null)
                        {
                            var photoThumbnailBytes = await _dataManager.GetData<TelegramBotManager>().DownloadPhotoById(video.Thumbnail.FileId);
                            viewModel.ThumbnailFileId = video.Thumbnail.FileId;
                            viewModel.ThumbnailFileUniqueId = video.Thumbnail.FileUniqueId;
                            viewModel.ThumbnailFileSize = video.Thumbnail.FileSize;
                            viewModel.ThumbnailWidth = video.Thumbnail.Width;
                            viewModel.ThumbnailHeight = video.Thumbnail.Height;
                            //viewModel.Thumbnail = photoThumbnailBytes;
                        }

                        _videos.Add(viewModel);
                        _media.ClientVideoInfos = _videos;
                        isDone = true;
                    }
                    else
                    {
                        await _dataManager.GetData<TelegramBotManager>().SendTextMessage(chatId, Get.ReplaceKeysInText(_messages[MessageKey.VERY_LONG_VIDEO], new Dictionary<string, string>() { { Promt.DURATION, TelegramSupport.LIMIT_VIDEO_DURATION.ToString() } }));
                    }

                }
                else
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
                            MediaGroupId = mediaGroupId,
                            Width = video.Width,
                            Height = video.Height,
                            Duration = video.Duration,
                            MimeType = video.MimeType,
                            //Video = videoBytes,
                        };
                        if (video.Thumbnail != null)
                        {
                            var photoThumbnailBytes = await _dataManager.GetData<TelegramBotManager>().DownloadPhotoById(video.Thumbnail.FileId);
                            viewModel.ThumbnailFileId = video.Thumbnail.FileId;
                            viewModel.ThumbnailFileUniqueId = video.Thumbnail.FileUniqueId;
                            viewModel.ThumbnailFileSize = video.Thumbnail.FileSize;
                            viewModel.ThumbnailWidth = video.Thumbnail.Width;
                            viewModel.ThumbnailHeight = video.Thumbnail.Height;
                            //viewModel.Thumbnail = photoThumbnailBytes;
                        }
                        _videos.Add(viewModel);
                        SetDelay(START_DELAY);
                    }
                    else
                    {
                        invalidVideoCount++;
                    }
                    await StartCheckerMediaGroup(update);
                }
            }
            else
            {
                await _dataManager.GetData<TelegramBotManager>().SendTextMessage(chatId, _messages[MessageKey.WRONG_MEDIA_INFO]);
                _dataManager.GetData<ILogger>().Debug($"User:{userId}; Condition:{nameof(MediaCondition)} -  Wrong format");
            }

            return isDone;
        }

        public async Task GetUpdate(Update update)
        {
            long chatId = Get.GetChatId(update);
            string messageText = Get.GetText(update);
            if (_supportMiniComands.Contains(messageText))
            {
                if (messageText == _messages[ReplyButton.YES])
                {
                    _listener.StopListen(this);
                    isStarted = false;
                    await Execute(update);
                }
                else
                {
                    invalidVideoCount = 0;
                    await EndMediaGroup(update);
                }
            }
            else
            {
                await _dataManager.GetData<TelegramBotManager>().SendTextMessage(chatId, Get.ReplaceKeysInText(_messages[MessageKey.WRONG_LOAD_VIDEO], new Dictionary<string, string>() { { Promt.VIDEO, invalidVideoCount.ToString() }, { Promt.DURATION, TelegramSupport.LIMIT_VIDEO_DURATION.ToString() } }));
            }
        }

        private async Task StartCheckerMediaGroup(Update update)
        {
            if (!_isStartProcessing)
            {
                long chatId = Get.GetChatId(update);
                await _dataManager.GetData<TelegramBotManager>().SendTextMessage(chatId, _messages[MessageKey.PROCESSING_MEDIA]);
                _isStartProcessing = true;
            }
            StopCheckerMediaGroup();
            _timer = new Timer();
            _timer.Interval = TimeSpan.FromSeconds(GetDelay()).TotalMilliseconds;
            _timer.Elapsed += async (sender, e) => await EndMediaGroup(update);
            _timer.Start();
        }

        private void StopCheckerMediaGroup() => _timer?.Stop();

        private async Task EndMediaGroup(Update update)
        {
            long chatId = Get.GetChatId(update);
            long userId = Get.GetUserId(update);

            if (_photos.Count + _videos.Count > LIMIT_MEDIA_COUNT)
            {
                _listener.StopListen(this);
                isStarted = false;
                await _dataManager.GetData<TelegramBotManager>().SendTextMessage(chatId, Get.ReplaceKeysInText(_messages[MessageKey.VERY_MANY_MEDIA_INFO], new Dictionary<string, string>() { { Promt.MEDIA, LIMIT_MEDIA_COUNT.ToString() } }));
                StopCheckerMediaGroup();
                await Execute(update);
                return;
            }

            if (invalidVideoCount > 0)
            {
                _listener.StartListen(this);
                List<KeyboardButton> inlineKeyboardButtons = new List<KeyboardButton>();
                ReplyKeyboardMarkup replyKeyboard = new ReplyKeyboardMarkup(new[]
                {
                    inlineKeyboardButtons
                });
                replyKeyboard.ResizeKeyboard = true;
                replyKeyboard.OneTimeKeyboard = true;
                foreach (var miniCommand in _supportMiniComands)
                {
                    inlineKeyboardButtons.Add(new KeyboardButton(miniCommand));
                }
                await _dataManager.GetData<TelegramBotManager>().SendTextMessage(chatId, Get.ReplaceKeysInText(_messages[MessageKey.WRONG_LOAD_VIDEO], new Dictionary<string, string>() { { Promt.VIDEO, invalidVideoCount.ToString() }, { Promt.DURATION, TelegramSupport.LIMIT_VIDEO_DURATION.ToString() } }), replyMarkup: replyKeyboard);
                StopCheckerMediaGroup();
            }
            else
            {
                _listener.StopListen(this);
                if (_photos.Any() && _photos.Count > 0)
                {
                    _media.ClientPhotoInfos = _photos;
                }

                if (_videos.Any() && _videos.Count > 0)
                {
                    _media.ClientVideoInfos = _videos;
                }

                isDone = true;
                StopCheckerMediaGroup();
                await _dataManager.GetData<TelegramBotManager>().ReupdateUser(userId);
            }

        }

        private void SetDelay(float delay, bool isReset = false)
        {
            if (isReset)
            {
                _currentDelay += delay;
                _currentDelay = Mathf.Clamp(_currentDelay, START_DELAY, MAX_DELAY);
            }
            else
            {
                _currentDelay = START_DELAY;
            }
        }

        private float GetDelay() => _currentDelay;
    }
}
