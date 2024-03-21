namespace Service.Core.Communication.Algorithms
{
    using Newtonsoft.Json;
    using Service.Abstract.Communication;
    using Service.Core.TelegramBot;
    using Service.Extensions;
    using Service.ViewModels;
    using Service.ViewModels.Communication;
    using System;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using Telegram.Bot.Types;

    public class TelegramAlgorithm : ICommunication
    {
        public string Description() => "Отправка сообщений в Telegram";

        public void AbortMessage(string MessageId, ClientViewModel To, string Login, string Password)
        {
            try
            {
                using (SettingsManager settingsManager = new SettingsManager())
                {
                    if (long.TryParse(To.TelegramId, out long chatId))
                    {
                        var telegramBotParams = settingsManager.GetTelegramBot();
                        string token = telegramBotParams.TokenApi;
                        string Url = "https://api.telegram.org/bot" + token + "/deleteMessage";

                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);

                        string postData = string.Format("chat_id={0}&message_id={1}", chatId, MessageId);
                        var data = Encoding.UTF8.GetBytes(postData);

                        request.Method = "POST";
                        request.ContentType = "application/x-www-form-urlencoded";
                        request.ContentLength = data.Length;

                        using (var stream = request.GetRequestStream())
                        {
                            stream.Write(data, 0, data.Length);
                        }

                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    }
                }
            }
            catch (Exception ex) { }
        }

        public ReceiveCommunicationInfo SendMessage(string From, string To, string Message, string Login = "", string Password = "")
        {
            try
            {
                using (SettingsManager settingsManager = new SettingsManager())
                {
                    if (long.TryParse(To, out long chatId))
                    {
                        var telegramBotParams = settingsManager.GetTelegramBot();
                        string token = telegramBotParams.TokenApi;
                        string Url = "https://api.telegram.org/bot" + token + "/sendMessage";
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);

                        string postData = string.Format("text={0}&chat_id={1}", Message, chatId);
                        var data = Encoding.UTF8.GetBytes(postData);

                        request.Method = "POST";
                        request.ContentType = "application/x-www-form-urlencoded";
                        request.ContentLength = data.Length;

                        using (var stream = request.GetRequestStream())
                        {
                            stream.Write(data, 0, data.Length);
                        }

                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        string contentResponse = null;

                        using (var streamReader = new StreamReader(response.GetResponseStream()))
                        {
                            contentResponse = streamReader.ReadToEnd();
                        }

                        var result = JsonConvert.DeserializeAnonymousType(contentResponse, new { ok = default(bool), result = new Message() });
                        if (result.ok)
                        {
                            return new ReceiveCommunicationInfo
                            {
                                ExternalId = result.result.MessageId.ToString(),
                                IsSend = true,
                                Recipient = chatId.ToString(),
                                SendMethod = Description(),
                                Cost = 0,
                                IsReceived = true
                            };
                        }
                    }
                }
            }
            catch (Exception ex) { }
            return new ReceiveCommunicationInfo { IsSend = false };
        }

        public async Task<ReceiveCommunicationInfo> SendMessage(ClientViewModel To, SendCommunicationInfo MessageModel, string Login, string Password, string From = "")
        {
            string messageText = MessageModel.Message;
            if (!To.TelegramId.IsNull() && long.TryParse(To.TelegramId, out long chatId))
            {
                if (!messageText.IsNull())
                {
                    string baseUrl = Get.GetProcUrl();
                    if (!baseUrl.IsValidLink())
                    {
                        throw new InvalidOperationException("Wrong url proc");
                    }
                    try
                    {
                        using (var client = new HttpClient())
                        {
                            if (!baseUrl.IsNull() && baseUrl.EndsWith("/"))
                            {
                                baseUrl = baseUrl.TrimEnd('/');
                            }

                            Update update = new Update();
                            update.Message = new Message()
                            {
                                Text = messageText,
                                Date = DateTime.UtcNow,
                            };
                            update.Message.Chat = new Chat()
                            {
                                Id = chatId,
                            };
                            update.Message.From = new User()
                            {
                                Id = chatId,
                                IsBot = false,
                                FirstName = string.Empty,
                                LastName = string.Empty,
                                Username = string.Empty,
                                LanguageCode = string.Empty,
                                IsPremium = false,
                                CanJoinGroups = false,
                                CanReadAllGroupMessages = false,
                                SupportsInlineQueries = false,
                            };
                            string requestJson = JsonConvert.SerializeObject(update);
                            string fullUrl = $"{baseUrl}/{GlobalTelegramSettings.BASE_MESSAGE}/{GlobalTelegramSettings.SEND_MESSAGE}?password={HttpUtility.UrlEncode(GlobalTelegramSettings.API_PASSWORD)}";
                            var content = new StringContent(requestJson, Encoding.UTF8, "application/json");
                            HttpResponseMessage response = await client.PostAsync(fullUrl, content);
                            if (response.IsSuccessStatusCode)
                            {
                                var contentResponse = await response.Content.ReadAsStringAsync();
                                var result = JsonConvert.DeserializeObject<Message>(contentResponse);
                                return new ReceiveCommunicationInfo
                                {
                                    ExternalId = result.MessageId.ToString(),
                                    IsSend = true,
                                    Recipient = chatId.ToString(),
                                    SendMethod = Description(),
                                    Cost = 0,
                                    IsReceived = true
                                };
                            }
                        }
                    }
                    catch (InvalidOperationException ex) { }
                }
            }
            return new ReceiveCommunicationInfo { IsSend = false };
        }

        public async Task<string> GetBalance(string login, string password) => "";
    }
}
