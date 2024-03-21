namespace Service.ViewModels.Communication
{
    public class ReceiveCommunicationInfo
    {
        public bool IsSend { get; set; }
        public string ExternalId { get; set; }
        public string Recipient { get; set; }
        public string SendMethod { get; set; }
        public decimal Cost { get; set; }
        public bool IsReceived { get; set; }
        public string Code { get; set; }
        public string SendResult { get; set; }
        public string ErrorMessage { get; set; }
        public string Type { get; set; }
    }
}
