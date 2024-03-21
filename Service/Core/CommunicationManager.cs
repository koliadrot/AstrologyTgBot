namespace Service.Core
{
    using Service.Abstract;
    using Service.Abstract.Communication;
    using Service.Core.Communication.Algorithms;

    public class CommunicationManager : ICommunicationManager
    {
        public ICommunication GetCurrentCommunication() => new TelegramAlgorithm();
    }
}
