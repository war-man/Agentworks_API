using System;

namespace MoneyGram.DLS.DomainModel.Request
{
    [Serializable]
    public class DailyTransactionDetailLookupRequest : BaseServiceMessage 
    {
        public string AgentId { get; set; }

        public string PosId { get; set; }

        public string StartDate { get; set; }
    }
}
