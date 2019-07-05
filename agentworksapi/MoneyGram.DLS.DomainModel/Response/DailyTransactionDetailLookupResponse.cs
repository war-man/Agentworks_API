using System;
using System.Collections.Generic;

namespace MoneyGram.DLS.DomainModel.Response
{
    [Serializable]
    public class DailyTransactionDetailLookupResponse : BaseServiceMessage
    {
        public List<TransactionDetailLookupResult> GetDailyTransactionDetailLookupResultList { get; set; }
    }
}
