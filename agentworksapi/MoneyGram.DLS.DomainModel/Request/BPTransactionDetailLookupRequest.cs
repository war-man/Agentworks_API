using System;

namespace MoneyGram.DLS.DomainModel.Request
{
    [Serializable]
    public class BPTransactionDetailLookupRequest : BaseServiceMessage
    {
        public string AgentId { get; set; }

        public string PosId { get; set; }

        public string StartDate { get; set; }

        public string ProductVariant { get; set; }

        public string EventType { get; set; }
    }
}
