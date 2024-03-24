namespace Service.Core
{
    using Service.Abstract;
    using Service.Abstract.Communication;
    using Service.Core.Communication.Algorithms;

    /// <summary>
    /// Менеджер коммуникаций
    /// </summary>
    public class CommunicationManager : ICommunicationManager
    {
        private ICommunication _currentAlgorithm = null;

        public ICommunication GetCurrentCommunication()
        {
            if (_currentAlgorithm == null)
            {
                //NOTE:Пока без админки будет автоматически выбираться телеграмм
                _currentAlgorithm = new TelegramAlgorithm();
            }
            return _currentAlgorithm;
        }
    }
}
