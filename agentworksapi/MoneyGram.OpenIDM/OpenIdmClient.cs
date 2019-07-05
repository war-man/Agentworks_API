using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MoneyGram.Common.Json;

namespace MoneyGram.OpenIDM
{
    public class OpenIdmClient : IOpenIdmClient
    {
        private readonly IOpenIdmConfig _openIdmConfig;

        public OpenIdmClient()
        {
            _openIdmConfig = new OpenIdmConfig();
        }

        public async Task<bool> RegisterDevice(DwRegisterDeviceRequest request)
        {
            var registrationStatus = false;

            using (var client = new HttpClient(new HttpClientHandler()))
            {
                client.BaseAddress = new Uri(_openIdmConfig.OpenIdmUrl);
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("x-openidm-username", "anonymous");
                client.DefaultRequestHeaders.Add("x-openidm-password", "anonymous");
                var serializedRequest = JsonProcessor.SerializeObject(request);
                var content = new StringContent(serializedRequest, Encoding.UTF8, "application/json");
                var requestUrl = "openidm/endpoint/device/MgiDT4/" + request.DeviceId + "?_action=register";
                HttpResponseMessage response = null;
                response = await client.PostAsync(requestUrl, content).ConfigureAwait(false);
                registrationStatus = response.StatusCode == HttpStatusCode.OK;
            }

            return registrationStatus;
        }
    }
}