using System;

namespace MoneyGram.DLS.DomainModel.Request
{
    [Serializable]
    public class MTTransactionDetailLookupRequest : BaseServiceMessage 
    {
        public string AgentId { get; set; }

        public string PosId { get; set; }

        public string StartDate { get; set; }

        public string DeliveryOption { get; set; }

        public string EventType { get; set; }
    }
}
