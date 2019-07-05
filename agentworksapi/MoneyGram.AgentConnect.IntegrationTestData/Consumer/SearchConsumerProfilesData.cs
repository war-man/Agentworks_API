using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.Requests;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.Consumer
{
    public class SearchConsumerProfilesData
    {
        public SearchConsumerProfilesData(SearchConsumerProfileRequest searchRequest)
        {
            Request = searchRequest;
        }

        public SearchConsumerProfileRequest Request { get; private set; }

        public SearchConsumerProfilesRequest SearchConsumerProfilesRequest { get; set; }
    }
}
