namespace Service.ViewModels.TelegramModels
{
    using System.Collections.Generic;

    /// <summary>
    /// Параметры телеграм бота
    /// </summary>
    public class TelegramBotParamsViewModel
    {
        /// <summary>
        /// Уникальный номер
        /// </summary>
        public int TelegramBotId { get; set; }

        /// <summary>
        /// Имя бота
        /// </summary>
        public string BotName { get; set; }

        /// <summary>
        /// Ник нейм бота
        /// </summary>
        public string BotUserName { get; set; }

        /// <summary>
        /// API Токен
        /// </summary>
        public string TokenApi { get; set; }

        /// <summary>
        /// Ссылка куда шлет запросы телеграм сервер
        /// </summary>
        public string WebHookUrl { get; set; }

        /// <summary>
        /// Ссылка на политику использования
        /// </summary>
        public string TosUrl { get; set; }

        /// <summary>
        /// Спрашивать при регистрации о рассылке персональных скидках,акциях и распродажах 
        /// </summary>
        public bool AcceptPromotionsBySms { get; set; }

        /// <summary>
        /// Спрашивать при регистрации о отправке электронных чеках
        /// </summary>
        public bool AcceptElectronicReceipts { get; set; }

        /// <summary>
        /// Список команд меню
        /// </summary>
        public virtual ICollection<TelegramBotCommandViewModel> BotCommands { get; set; } = new List<TelegramBotCommandViewModel>();

        /// <summary>
        /// Список условий регистрации
        /// </summary>
        public virtual ICollection<TelegramBotRegisterConditionViewModel> RegisterConditions { get; set; } = new List<TelegramBotRegisterConditionViewModel>();

        /// <summary>
        /// Список сообщений бота
        /// </summary>
        public virtual ICollection<TelegramBotParamMessageViewModel> Messages { get; set; } = new List<TelegramBotParamMessageViewModel>();

        /// <summary>
        /// Текст вступительного приветствия бота
        /// </summary>
        public string HelloText { get; set; }

        /// <summary>
        /// Состав меню телеграм бота в JSON формате
        /// </summary>
        public string Menu { get; set; }
    }
}
