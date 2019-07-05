using MoneyGram.AgentConnect.IntegrationTest.Data.Requests;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.Consumer
{
    public class GetProfileConsumerData
    {
        public GetProfileConsumerData(GetProfileConsumerRequest getProfileConsumerRequest)
        {
            Request = getProfileConsumerRequest;
        }

        public GetProfileConsumerRequest Request { get; private set; }

        public DomainModel.Transaction.GetProfileConsumerRequest GetProfileConsumerRequest { get;set; }
    }
}
