using MoneyGram.AgentConnect.IntegrationTest.Data.Requests;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.Consumer
{
    public sealed class CreateOrUpdateProfileReceiverData
    {
        public CreateOrUpdateProfileReceiverData(CreateOrUpdateProfileReceiverRequest createOrUpdateProfileRequest)
        {
            Request = createOrUpdateProfileRequest;
        }

        public CreateOrUpdateProfileReceiverRequest Request { get; }

        public DomainModel.Transaction.CreateOrUpdateProfileReceiverRequest CreateOrUpdateProfileReceiverRequest { get; set; }
    }
}