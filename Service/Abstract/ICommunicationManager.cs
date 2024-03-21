namespace Service.Abstract
{
    using Communication;

    /// <summary>
    /// Менеджер свяств связи
    /// </summary>
    public interface ICommunicationManager
    {
        /// <summary>
        /// Получает текущий тип связи
        /// </summary>
        /// <returns></returns>
        ICommunication GetCurrentCommunication();
    }
}
