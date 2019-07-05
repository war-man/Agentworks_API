using System.Collections.Generic;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.Models
{
    public class RefundReasonInfoList
    {
        public List<EnumeratedIdentifierInfo> RefundReasonInfos { get; set; }
    }
}