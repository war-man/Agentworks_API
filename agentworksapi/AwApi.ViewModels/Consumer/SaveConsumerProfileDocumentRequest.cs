using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace AwApi.ViewModels.Consumer
{
    public class SaveConsumerProfileDocumentRequest : Request
    {
        public string ConsumerProfileID { get; set; }
        public string ConsumerProfileIDType { get; set; }
        public ConsumerProfileDocumentContentType ConsumerProfileDocument { get; set; }
    }
}
