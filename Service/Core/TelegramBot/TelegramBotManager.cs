using Data.Core;
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
        /// Сброшены ли обновления при отключенном боте?
        /// </summary>
        public bool IsDropPendingUpdates { get; private set; } = default;

        /// <summary>
        /// Имя бота
        /// </summary>
        public string Name { get; private set; } = string.Empty;

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

        private TelegramBotClient _client;

        public TelegramBotManager() => Start();

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
                    var telegramBotParams = bonusDbContext.TelegramParams.FirstOrDefault();
                    if (telegramBotParams.TokenApi.IsNull() || telegramBotParams.WebHookUrl.IsNull())
                    {
                        Stop();
                        return;
                    }
                    Name = telegramBotParams.BotName;
                    UserName = telegramBotParams.BotUserName;
                    Token = telegramBotParams.TokenApi;
                    WebHookUrl = telegramBotParams.WebHookUrl;
                    if (WebHookUrl.EndsWith("/"))
                    {
                        WebHookUrl = WebHookUrl.TrimEnd('/');
                    }
                    _client = new TelegramBotClient(Token);
                    string hook = $"{WebHookUrl}/{GlobalTelegramSettings.BASE_MESSAGE}/{GlobalTelegramSettings.UPDATE_MESSAGE}";
                    await _client.SetWebhookAsync(hook);
                    var task = await _client.GetWebhookInfoAsync();
                    if (!Name.IsNull())
                    {
                        try
                        {
                            await _client.SetMyNameAsync(Name);
                        }
                        catch (ApiRequestException ex)
                        {
                            //NOTE:Часто менять имя нельзя! (приблизительно раз в сутки)
                        }
                    }
                    IsStarted = true;
                    IsDropPendingUpdates = false;
                    OnStarted();
                }
            }
        }

        /// <summary>
        /// Остановка работы бота
        /// </summary>
        /// <param name="isDropPendingUpdates">Если установлен в true, то метод удалит все ожидающие обновления (pending updates),
        /// которые могли накопиться в процессе использования вебхука. Ожидающие обновления - это обновления, которые Telegram API
        /// не смогло доставить вашему боту в прошлые попытки и которые могут быть обработаны позже при удалении вебхука.</param>
        /// <returns></returns>
        public void Stop(bool isDropPendingUpdates = true)
        {
            if (_client != null)
            {
                IsStarted = false;
                IsDropPendingUpdates = isDropPendingUpdates;
                _client = null;
                OnStoped();
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
                var t = ex.ToString();
            }
        }

        /// <summary>
        /// Отправляет текстовое сообщение
        /// </summary>
        /// <param name="chatId">Id чата</param>
        /// <param name="message">Текст сообщения</param>
        /// <param name="replyMarkup">Клавиатура ответа</param>
        /// <returns></returns>
        public async Task<Message> SendTextMessage(long chatId, string message, IReplyMarkup replyMarkup = null, bool isShowPreview = true, ParseMode? parseMode = null)
        {
            await WaitInitTask();
            var messageBody = await _client.SendTextMessageAsync(chatId, message, replyMarkup: replyMarkup, disableWebPagePreview: !isShowPreview, parseMode: parseMode);
            return messageBody;
        }

        /// <summary>
        /// Отправляет фото сообщение
        /// </summary>
        /// <param name="chatId">Id чата</param>
        /// <param name="image">Фото</param>
        /// <param name="replyMarkup">Клавиатура ответа</param>
        /// <returns></returns>
        public async Task<Message> SendPhotoMessage(long chatId, InputFileStream image, string caption = null, IReplyMarkup replyMarkup = null, ParseMode? parseMode = null)
        {
            await WaitInitTask();
            var messageBody = await _client.SendPhotoAsync(chatId, image, replyMarkup: replyMarkup, caption: caption, parseMode: parseMode);
            return messageBody;
        }
    }
}
