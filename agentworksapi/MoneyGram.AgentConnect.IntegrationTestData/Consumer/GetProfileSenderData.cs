using MoneyGram.AgentConnect.IntegrationTest.Data.Requests;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.Consumer
{
    public class GetProfileSenderData
    {
        public GetProfileSenderData(GetProfileSenderRequest getProfileSenderRequest)
        {
            Request = getProfileSenderRequest;
        }

        public GetProfileSenderRequest Request { get; private set; }

        public DomainModel.Transaction.GetProfileSenderRequest GetProfileSenderRequest { get; set; }
    }
}