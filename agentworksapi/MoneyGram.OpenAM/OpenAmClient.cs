using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MoneyGram.Common.Json;

namespace MoneyGram.OpenAM
{
    public class OpenAmClient : IOpenAmClient
    {
        private readonly IOpenAmConfig openAmConfig;

        public OpenAmClient(IOpenAmConfig openAmConfig)
        {
            this.openAmConfig = openAmConfig;
        }

        public async Task<bool> ValidateToken(string token)
        {
            var isValid = false;

            using (var client = openAmHttpClient(ClientType.User))
            {
                var realm = !string.IsNullOrEmpty(openAmConfig.Realm) ? openAmConfig.Realm + "/" : string.Empty;
                var response = await client.GetAsync("auth/oauth2/" + realm + "tokeninfo?access_token=" + token);
                isValid = response.IsSuccessStatusCode;
            }

            return isValid;
        }

        public async Task<Dictionary<string, object>> GetUserInfo(string token)
        {
            Dictionary<string, object> userInfo = null;

            using (var client = openAmHttpClient(ClientType.User))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var realm = !string.IsNullOrEmpty(openAmConfig.Realm) ? openAmConfig.Realm + "/" : string.Empty;
                var response =
                    await client.PostAsync("auth/oauth2/" + realm + "userinfo", new StringContent(string.Empty));

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    userInfo = JsonProcessor.DeserializeObject<Dictionary<string, object>>(content);
                }
            }

            return userInfo;
        }

        public async Task<Dictionary<string, object>> ValidateDevice(string deviceId)
        {
            // Mapping between properties received from OpenAM and their OAuth claim equivalents
            var sessionProperties = new Dictionary<string, string>
            {
                {"mgiMainOfficeId", "mgiMainOfficeId"},
                {"mgiDeviceAgentId", "mgiDeviceAgentLocationId"},
                {"mgiDevicePosNumber", "mgiDevicePosNumber"},
                {"mgiDeviceId", "mgiDeviceId"}
            };

            Dictionary<string, object> deviceInfo = null;
            using (var client = openAmHttpClient(ClientType.Device))
            {
                var url = "auth/json/sessions/?_action=getProperty";
                var request = new
                {
                    properties = sessionProperties.Keys.ToList()
                };

                var content = new StringContent(JsonProcessor.SerializeObject(request), Encoding.UTF8,
                    "application/json");
                content.Headers.Add("mgiSsoSession", deviceId);

                var response = await client.PostAsync(url, content);

                // This helps to see requests in Fiddler
                // Should not be uncommented for anything except debugging purposes
                //WebRequest.DefaultWebProxy = new WebProxy("127.0.0.1", 8888);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return null;
                }

                var responseData = await response.Content.ReadAsStringAsync();

                deviceInfo = JsonProcessor.DeserializeObject<Dictionary<string, object>>(responseData);

                // Convert to OAuth claim names
                deviceInfo = deviceInfo
                    .Select(x => new KeyValuePair<string, object>(sessionProperties[x.Key], x.Value))
                    .ToDictionary(x => x.Key, x => x.Value);
            }

            return deviceInfo;
        }

        public async Task<Dictionary<string, object>> ValidateUnregisteredDevice(string deviceId)
        {
            Dictionary<string, object> deviceInfo = null;
            using (var client = openAmHttpClient(ClientType.Device))
            {
                var url = "auth/json/sessions/?_action=getProperty";

                var content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
                content.Headers.Add("mgiSsoSession", deviceId);

                var response = await client.PostAsync(url, content);

                // This helps to see requests in Fiddler
                // Should not be uncommented for anything except debugging purposes
                //WebRequest.DefaultWebProxy = new WebProxy("127.0.0.1", 8888);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return null;
                }

                var responseData = await response.Content.ReadAsStringAsync();

                deviceInfo = JsonProcessor.DeserializeObject<Dictionary<string, object>>(responseData);
                deviceInfo = deviceInfo.Where(prop => !string.IsNullOrWhiteSpace((string) prop.Value))
                    .ToDictionary(prop => prop.Key,
                        prop => prop.Value);
            }

            return deviceInfo;
        }

        private HttpClient openAmHttpClient(ClientType clientType)
        {
            var baseUrl = getBaseUrl(clientType);
            var openAmHttpClient = new HttpClient(new HttpClientHandler())
            {
                BaseAddress = new Uri(baseUrl)
            };

            openAmHttpClient.DefaultRequestHeaders.Accept.Clear();
            openAmHttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return openAmHttpClient;
        }

        private string getBaseUrl(ClientType clientType)
        {
            var baseUrl = string.Empty;
            switch (clientType)
            {
                case ClientType.User:
                    baseUrl = openAmConfig.OpenAmUrl;
                    break;
                case ClientType.Device:
                    baseUrl = openAmConfig.OpenAmDeviceUrl;
                    break;
            }

            return baseUrl;
        }
    }
}