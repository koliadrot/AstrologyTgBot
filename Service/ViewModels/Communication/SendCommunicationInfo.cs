namespace Service.ViewModels.Communication
{
    using System.Collections.Generic;

    public class SendCommunicationInfo
    {
        public SendCommunicationInfo(string message)
        {
            this.Message = message;
            this.AdditionalParams = new Dictionary<string, string>();
        }
        public SendCommunicationInfo(string message, string title, Dictionary<string, string> additionalParams)
        {
            this.Message = message;
            this.Title = title;
            this.AdditionalParams = additionalParams;
        }
        public SendCommunicationInfo(string message, string title)
        {
            this.Message = message;
            this.Title = title;
            this.AdditionalParams = new Dictionary<string, string>();
        }
        public SendCommunicationInfo()
        {
            this.AdditionalParams = new Dictionary<string, string>();
        }
        public string Message { get; set; }
        public string Title { get; set; }
        public Dictionary<string, string> AdditionalParams { get; set; }
        public bool ShowInMobileApplicationHistory { get; set; }
    }
}
