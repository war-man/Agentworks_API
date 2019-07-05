using System;
using System.Collections.Generic;

namespace MoneyGram.DLS.DomainModel.Response
{
    [Serializable]
    public class MTTransactionDetailLookupResponse : BaseServiceMessage 
    {
        public List<TransactionDetailLookupResult> GetMTTransactionDetailLookupResultList;
    }
}
