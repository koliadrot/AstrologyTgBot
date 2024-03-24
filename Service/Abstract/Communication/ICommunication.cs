namespace Service.Abstract.Communication
{
    using Service.ViewModels;
    using Service.ViewModels.Communication;
    using System.Threading.Tasks;

    /// <summary>
    /// Интерфейс коммуникации
    /// </summary>
    public interface ICommunication
    {
        public const string TYPE_MESSAGE_KEY = "typeMessageKey";

        ReceiveCommunicationInfo SendMessage(string From, string To, string Message, string Login = "", string Password = "");
        Task<ReceiveCommunicationInfo> SendMessage(ClientViewModel To, SendCommunicationInfo MessageModel, string Login, string Password, string From = "");
        Task<string> GetBalance(string login, string password);
        void AbortMessage(string MessageId, ClientViewModel To, string Login, string Password);

        /// <summary>
        /// Описание
        /// </summary>
        /// <returns></returns>
        string Description();

        /// <summary>
        /// Залочена отправка
        /// </summary>
        bool IsLock { get; }
    }
}
