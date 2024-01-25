namespace Service.Support
{
    using Newtonsoft.Json.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Поиск городов и мест с деталими по координатам с OpenMap
    /// https://nominatim.org/release-docs/develop/api/Overview/
    /// </summary>
    public class OpenMap
    {
        private const string BASE_URL = "https://nominatim.openstreetmap.org";
        private const string USER_AGENT = "Mozilla/5.0 (compatible; AcmeInc/2.0)";

        /// <summary>
        /// Детали по имени города
        /// </summary>
        /// <param name="cityName">Имя города</param>
        /// <returns></returns>
        /// https://nominatim.org/release-docs/develop/api/Search/
        public async Task<JArray?> GetDetailsByCityName(string cityName)
        {
            if (!string.IsNullOrEmpty(cityName))
            {
                string url = $"{BASE_URL}/search?addressdetails=1&q={cityName}&format=jsonv2&limit=1";

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.UserAgent.ParseAdd(USER_AGENT);
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        JArray results = JArray.Parse(responseBody);

                        return results;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Детали по координатам геопозиции
        /// </summary>
        /// <param name="lat">Широта</param>
        /// <param name="lon">Долгота</param>
        /// <returns></returns>
        /// https://nominatim.org/release-docs/develop/api/Reverse/
        public async Task<JObject?> GetDetailsByGeo(string lat, string lon)
        {
            if (!string.IsNullOrEmpty(lat) && !string.IsNullOrEmpty(lon))
            {
                string url = $"{BASE_URL}/reverse?format=jsonv2&lat={lat}&lon={lon}";

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.UserAgent.ParseAdd(USER_AGENT);
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        JObject result = JObject.Parse(responseBody);

                        return result;
                    }
                }
            }
            return null;
        }
    }
}
