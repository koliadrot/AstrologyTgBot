namespace Service.Abstract.TelegramBot
{
    /// <summary>
    /// Интерфейс слушателя. Слушает обновляющего.Для работы с перенаправлением обновлений сообщений.
    /// По сути начинаем рекурсию в рекурсии.
    /// Например:регистрация, где пользователь сначала указывает номер, потом имя, фамилию и тд.
    /// с возможностью перенаправлять обновления из друг в друга.
    /// </summary>
    public interface IListener
    {
        /// <summary>
        /// Текущий обновляющий
        /// </summary>
        IUpdater CurrentUpdater { get; }

        /// <summary>
        /// Начать слушать обновляющего
        /// </summary>
        void StartListen(IUpdater updater);

        /// <summary>
        /// Стоп слушать обновляющего
        /// </summary>
        void StopListen(IUpdater updater);
    }
}