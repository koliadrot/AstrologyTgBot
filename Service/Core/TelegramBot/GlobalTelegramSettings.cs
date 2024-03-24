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
        public const string RE_UPDATE_MESSAGE = "reupdate";
        public const string SEND_MESSAGE = "send";
        public const string OFFER_SHOW_FIND_CLIENTS_NOTIFY = "offershowfindclientsnotify";
        public const string NEW_LIKES_NOTIFY = "newlikesnotify";
        #endregion
    }
}
