using MoneyGram.AgentConnect.IntegrationTest.Data.Requests;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.Consumer
{
    public sealed class CreateOrUpdateProfileConsumerData
    {
        public CreateOrUpdateProfileConsumerData(CreateOrUpdateProfileConsumerRequest createOrUpdateProfileRequest)
        {
            Request = createOrUpdateProfileRequest;
        }

        public CreateOrUpdateProfileConsumerRequest Request { get; private set; }

        public DomainModel.Transaction.CreateOrUpdateProfileConsumerRequest CreateOrUpdateProfileConsumerRequest { get; set; }
    }
}