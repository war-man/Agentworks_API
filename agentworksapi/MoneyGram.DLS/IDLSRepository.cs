using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyGram.DLS.DomainModel.Request;
using MoneyGram.DLS.DomainModel.Response;

namespace MoneyGram.DLS
{
    public interface IDLSRepository
    {
        DailyTransactionDetailLookupResponse DailyTransactionDetailLookup(bool isInTrainingMode, DailyTransactionDetailLookupRequest dailyTransactionDetailLookupRequest);
        Task<DailyTransactionDetailLookupResponse> DailyTransactionDetailLookupAsync(bool isInTrainingMode, DailyTransactionDetailLookupRequest dailyTransactionDetailLookupRequest);
        
        BPTransactionDetailLookupResponse BPTransactionDetailLookup(bool isInTrainingMode, BPTransactionDetailLookupRequest bpTransactionDetailLookupRequest, IList<string> strPosIdList);
        Task<BPTransactionDetailLookupResponse> BPTransactionDetailLookupAsync(bool isInTrainingMode, BPTransactionDetailLookupRequest bpTransactionDetailLookupRequest, IList<string> strPosIdList);

        MTTransactionDetailLookupResponse MTTransactionDetailLookup(bool isInTrainingMode, MTTransactionDetailLookupRequest mtTransactionDetailLookupRequest, IList<string> strPosIdList);
        Task<MTTransactionDetailLookupResponse> MTTransactionDetailLookupAsync(bool isInTrainingMode, MTTransactionDetailLookupRequest mtTransactionDetailLookupRequest, IList<string> strPosIdList);
    }
}
