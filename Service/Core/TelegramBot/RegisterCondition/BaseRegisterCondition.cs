using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;

namespace Service.Core.TelegramBot.RegisterCondition
{
    /// <summary>
    /// Базовая реализация условия регистрации
    /// </summary>
    public class BaseRegisterCondition
    {
        protected virtual ReplyKeyboardMarkup GetSkipReplyButtons(bool isCanPass, string replyButton)
        {
            ReplyKeyboardMarkup replySkipKeyboard = default;
            if (isCanPass)
            {
                List<KeyboardButton> inlineKeyboardButtons = new List<KeyboardButton>();
                replySkipKeyboard = new ReplyKeyboardMarkup(new[]
                {
                inlineKeyboardButtons
            });
                replySkipKeyboard.ResizeKeyboard = true;
                replySkipKeyboard.OneTimeKeyboard = true;
                inlineKeyboardButtons.Add(new KeyboardButton(replyButton));
            }
            return replySkipKeyboard;
        }
    }
}
