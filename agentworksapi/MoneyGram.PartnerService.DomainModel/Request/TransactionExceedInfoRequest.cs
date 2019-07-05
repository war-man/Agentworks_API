using System;

namespace MoneyGram.PartnerService.DomainModel
{
    [Serializable]
    public class TransactionExceedInfoRequest : BaseServiceMessage
    {
        public decimal AgentId { get; set; }

        public bool AgentIdFieldSpecified { get; set; }

        public System.DateTime TransactionDate { get; set; }

        public bool TransactionDateFieldSpecified { get; set; }
    }
}
