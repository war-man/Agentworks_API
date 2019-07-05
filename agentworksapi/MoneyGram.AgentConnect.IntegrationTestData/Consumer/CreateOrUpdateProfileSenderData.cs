using MoneyGram.AgentConnect.IntegrationTest.Data.Requests;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.Consumer
{
    public sealed class CreateOrUpdateProfileSenderData
    {
        public CreateOrUpdateProfileSenderData(CreateOrUpdateProfileSenderRequest createOrUpdateProfileRequest)
        {
            Request = createOrUpdateProfileRequest;
        }

        public CreateOrUpdateProfileSenderRequest Request { get; }

        public DomainModel.Transaction.CreateOrUpdateProfileSenderRequest CreateOrUpdateProfileSenderRequest { get; set; }
    }
}