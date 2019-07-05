using System;
using System.Collections.Generic;

namespace MoneyGram.DLS.DomainModel.Response
{
    [Serializable]
    public class BPTransactionDetailLookupResponse : BaseServiceMessage 
    {
        public List<TransactionDetailLookupResult> GetDailyTransactionDetailLookupResultList { get; set; }
    }
}
