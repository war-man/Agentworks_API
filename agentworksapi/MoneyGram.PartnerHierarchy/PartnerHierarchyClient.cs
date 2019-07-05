using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MoneyGram.Common.Json;
using MoneyGram.PartnerHierarchy.DomainModel.Request;
using MoneyGram.PartnerHierarchy.DomainModel.Response;
using Newtonsoft.Json;

namespace MoneyGram.PartnerHierarchy
{
    public class PartnerHierarchyClient : IPartnerHierarchyClient
    {
        private readonly IPartnerHierarchyConfig _partnerHierarchyConfig;

        public PartnerHierarchyClient(IPartnerHierarchyConfig partnerHierarchyConfig)
        {
            _partnerHierarchyConfig = partnerHierarchyConfig;
        }

        public async Task<PartnerHierarchyAgentResponse> GetPartnerHierarchyAgent(PartnerHierarchyAgentRequest request)
        {
            PartnerHierarchyAgentResponse result = null;

            using (var client = new HttpClient(new HttpClientHandler()))
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));                
                string requestUrl = $"{_partnerHierarchyConfig.PartnerHierarchyUrl}/mainoffices/{request.MainofficeId}/locations/{request.LocationId}";
                HttpResponseMessage response = await client.GetAsync(requestUrl).ConfigureAwait(false);                
                string content = await response.Content.ReadAsStringAsync();

                try //try to deserialize response version with single Agent
                {
                    return JsonProcessor.DeserializeObject<PartnerHierarchySingleAgentResponse>(content);
                }
                catch (JsonSerializationException) //if fails try deserialize response version where Agent is collection
                {
                    return JsonProcessor.DeserializeObject<PartnerHierarchyMultiAgentResponse>(content);
                }
           }
        }
    }
}