namespace Service.Core.TelegramBot
{
    /// <summary>
    /// Глобальные маршруты Api
    /// </summary>
    public static class GlobalTelegramSettings
    {
        #region General
        public const string API_PASSWORD = "b9aab849-b5f7-4224-9e3a-d739919c2008";
        #endregion

        #region Routes
        public const string BASE_MESSAGE = "api/message";
        public const string UPDATE_MESSAGE = "update";
        public const string SEND_MESSAGE = "send";
        #endregion
    }
}
