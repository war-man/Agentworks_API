using System.Collections.Generic;
using MoneyGram.DLS.DomainModel.Response;

namespace MoneyGram.DLS
{
    public interface ITrainingModeRepository
    {
        MTTransactionDetailLookupResponse MockMTTransactionDetailLookup(IList<string> strPosIdList);
        BPTransactionDetailLookupResponse MockBPTransactionDetailLookup(IList<string> strPosIdList);
    }
}
