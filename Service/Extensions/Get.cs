using System.Text.RegularExpressions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Service
{
    public static class Get
    {

        /// <summary>
        /// Получает Id чата запроса
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        public static long GetChatId(Update update)
        {
            switch (update.Type)
            {
                case UpdateType.Message:
                    return update.Message.Chat.Id;
                case UpdateType.CallbackQuery:
                    return update.CallbackQuery.Message.Chat.Id;
                case UpdateType.Unknown:
                    break;
                case UpdateType.InlineQuery:
                    break;
                case UpdateType.ChosenInlineResult:
                    break;
                case UpdateType.EditedMessage:
                    break;
                case UpdateType.ChannelPost:
                    break;
                case UpdateType.EditedChannelPost:
                    break;
                case UpdateType.ShippingQuery:
                    break;
                case UpdateType.PreCheckoutQuery:
                    break;
                case UpdateType.Poll:
                    break;
                case UpdateType.PollAnswer:
                    break;
                case UpdateType.MyChatMember:
                    break;
                case UpdateType.ChatMember:
                    break;
                case UpdateType.ChatJoinRequest:
                    break;
                default: return 0;
            }
            return 0;
        }

        /// <summary>
        /// Получает текст запроса
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        public static string GetText(Update update)
        {
            switch (update.Type)
            {
                case UpdateType.Message:
                    if (update.Message.Text == null)
                    {
                        if (update.Message.Contact != null)
                        {
                            return update.Message.Contact.PhoneNumber;
                        }
                        else if (update.Message.Location != null)
                        {
                            return $"{update.Message.Location.Latitude}:{update.Message.Location.Longitude}";
                        }
                        break;
                    }
                    else
                    {
                        return update.Message.Text;
                    }
                case UpdateType.CallbackQuery:
                    return update.CallbackQuery.Data;
            }
            return string.Empty;
        }

        /// <summary>
        /// Получает Id пользователя
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        public static long GetUserId(Update update)
        {
            switch (update.Type)
            {
                case UpdateType.Message:
                    return update.Message.From.Id;
                case UpdateType.CallbackQuery:
                    return update.CallbackQuery.From.Id;
                case UpdateType.Unknown:
                    break;
                case UpdateType.InlineQuery:
                    break;
                case UpdateType.ChosenInlineResult:
                    break;
                case UpdateType.EditedMessage:
                    break;
                case UpdateType.ChannelPost:
                    break;
                case UpdateType.EditedChannelPost:
                    break;
                case UpdateType.ShippingQuery:
                    break;
                case UpdateType.PreCheckoutQuery:
                    break;
                case UpdateType.Poll:
                    break;
                case UpdateType.PollAnswer:
                    break;
                case UpdateType.MyChatMember:
                    break;
                case UpdateType.ChatMember:
                    break;
                case UpdateType.ChatJoinRequest:
                    break;
                default: return 0;
            }
            return 0;
        }

        /// <summary>
        /// Получает техническое имя пользователя
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        public static string GetUserName(Update update)
        {
            switch (update.Type)
            {
                case UpdateType.Message:
                    return update.Message.From.Username;
                case UpdateType.CallbackQuery:
                    return update.CallbackQuery.From.Username;
                case UpdateType.Unknown:
                    break;
                case UpdateType.InlineQuery:
                    break;
                case UpdateType.ChosenInlineResult:
                    break;
                case UpdateType.EditedMessage:
                    break;
                case UpdateType.ChannelPost:
                    break;
                case UpdateType.EditedChannelPost:
                    break;
                case UpdateType.ShippingQuery:
                    break;
                case UpdateType.PreCheckoutQuery:
                    break;
                case UpdateType.Poll:
                    break;
                case UpdateType.PollAnswer:
                    break;
                case UpdateType.MyChatMember:
                    break;
                case UpdateType.ChatMember:
                    break;
                case UpdateType.ChatJoinRequest:
                    break;
                default: return string.Empty;
            }
            return string.Empty;
        }

        /// <summary>
        /// Сообщение отправлено ботом?
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        public static bool IsBot(Update update)
        {
            switch (update.Type)
            {
                case UpdateType.Message:
                    return update.Message.From.IsBot;
                case UpdateType.CallbackQuery:
                    return update.CallbackQuery.From.IsBot;
                case UpdateType.Unknown:
                    break;
                case UpdateType.InlineQuery:
                    break;
                case UpdateType.ChosenInlineResult:
                    break;
                case UpdateType.EditedMessage:
                    break;
                case UpdateType.ChannelPost:
                    break;
                case UpdateType.EditedChannelPost:
                    break;
                case UpdateType.ShippingQuery:
                    break;
                case UpdateType.PreCheckoutQuery:
                    break;
                case UpdateType.Poll:
                    break;
                case UpdateType.PollAnswer:
                    break;
                case UpdateType.MyChatMember:
                    break;
                case UpdateType.ChatMember:
                    break;
                case UpdateType.ChatJoinRequest:
                    break;
                default: return false;
            }
            return false;
        }

        /// <summary>
        /// Приводит мобильный номер к нормальному виду
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string NormalizePhoneNumber(string number)
        {
            string normalizedNumber = Regex.Replace(number, @"[^0-9]+", "");
            if (normalizedNumber.StartsWith("8") && normalizedNumber.Length == 11)
            {
                normalizedNumber = "7" + normalizedNumber.Substring(1);
            }
            if (normalizedNumber.Length == 10 && !normalizedNumber.StartsWith("+"))
            {
                normalizedNumber = "+7" + normalizedNumber;
            }
            if (normalizedNumber.Length == 9 && !normalizedNumber.StartsWith("+"))
            {
                normalizedNumber = "+992" + normalizedNumber;
            }
            normalizedNumber = Regex.Replace(normalizedNumber, @"[^0-9]+", "");
            if (!normalizedNumber.StartsWith("+"))
            {
                normalizedNumber = "+" + normalizedNumber;
            }
            return normalizedNumber;
        }

        /// <summary>
        /// Приводит текстовое сообщение с заменой значений %example% шаблону
        /// </summary>
        /// <param name="text"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string ReplaceKeysInText(string text, Dictionary<string, string> info)
        {
            var matches = Regex.Matches(text, @"%\w+%");
            string resultText = text;
            foreach (Match match in matches)
            {
                string key = match.Value.Trim('%').ToLower();
                if (info.ContainsKey(key))
                {
                    string value = info[key];
                    resultText = resultText.Replace(match.Value, value);
                }
            }
            return resultText;
        }

        /// <summary>
        /// Получить возраст
        /// </summary>
        /// <param name="text"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static int GetAge(DateTime startDate)
        {
            int age = DateTime.Now.Year - startDate.Year;
            if (DateTime.Now.Month < startDate.Month || (DateTime.Now.Month == startDate.Month && DateTime.Now.Day < startDate.Day))
            {
                age -= 1;
            }
            return age;
        }
    }
}
