using MoneyGram.AgentConnect.IntegrationTest.Data.Requests;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.Consumer
{
    public class GetProfileReceiverData
    {
        public GetProfileReceiverData(GetProfileReceiverRequest getProfileReceiverRequest)
        {
            Request = getProfileReceiverRequest;
        }

        public GetProfileReceiverRequest Request { get; private set; }

        public DomainModel.Transaction.GetProfileReceiverRequest GetProfileReceiverRequest { get; set; }
    }
}