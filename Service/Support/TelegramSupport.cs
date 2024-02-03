namespace Service.Support
{
    using Service.Abstract;
    using Service.Core;
    using Service.Core.TelegramBot;
    using Telegram.Bot.Types;

    /// <summary>
    /// Телеграм команда расширения
    /// </summary>
    public class TelegramSupport
    {
        private TelegramBotManager _telegramBotManager;
        private DataManager _dataManager;

        public TelegramSupport(DataManager dataManager) => _dataManager = dataManager;

        public TelegramSupport(TelegramBotManager telegramBotManager) => _telegramBotManager = telegramBotManager;

        /// <summary>
        /// Отправить анкету клиента
        /// </summary>
        /// <param name="update">Инфо от кого пришел запрос</param>
        /// <param name="userId">Id клиента анкеты, которую надо отправить</param>
        /// <returns></returns>
        public async Task<Message> SendUserApplication(Update update, long userId)
        {
            long chatId = Get.GetChatId(update);
            return await SendUserApplication(chatId, userId);
        }

        /// <summary>
        /// Отправить анкету клиента
        /// </summary>
        /// <param name="chatId">Id в какой чат отправить анкету</param>
        /// <param name="userId">Id клиента анкеты, которую надо отправить</param>
        /// <returns></returns>
        public async Task<Message> SendUserApplication(long chatId, long userId)
        {
            if (_dataManager == null)
            {
                using (CustomerManager customerManager = new CustomerManager())
                {
                    var user = customerManager.GetClientByTelegram(userId.ToString());

                    int age = Get.GetAge(user.Birthday.Value);
                    string caption = $"{user.FirstName} - {age}, {user.SearchCity}\n{user.AboutMe}";
                    var media = customerManager.GetMediaFilesByUserId(userId);

                    if (media != null && media.Any())
                    {
                        if (media.Count == 1)
                        {
                            InputMedia file = media.FirstOrDefault();
                            return await _telegramBotManager.SendMediaMessage(chatId, file, caption);
                        }
                        else
                        {
                            Message[] messages = await _telegramBotManager.SendMediaGroupMessage(chatId, media, caption);
                            return messages.FirstOrDefault();
                        }
                    }
                    return null;
                }
            }
            else
            {
                var user = _dataManager.GetData<ICustomerManager>().GetClientByTelegram(userId.ToString());

                int age = Get.GetAge(user.Birthday.Value);
                string caption = $"{user.FirstName} - {age}, {user.SearchCity}\n{user.AboutMe}";
                var media = _dataManager.GetData<ICustomerManager>().GetMediaFilesByUserId(userId);

                if (media != null && media.Any())
                {
                    if (media.Count == 1)
                    {
                        InputMedia file = media.FirstOrDefault();
                        return await _dataManager.GetData<TelegramBotManager>().SendMediaMessage(chatId, file, caption);
                    }
                    else
                    {
                        Message[] messages = await _dataManager.GetData<TelegramBotManager>().SendMediaGroupMessage(chatId, media, caption);
                        return messages.FirstOrDefault();
                    }
                }
                return null;
            }
        }
    }
}
