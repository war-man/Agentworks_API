using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyGram.Common.Extensions;
using MoneyGram.DLS.DomainModel.Request;
using MoneyGram.DLS.DomainModel.Response;

namespace MoneyGram.DLS
{
    public abstract class DLSRepositoryDecorator : IDLSRepository
    {
        private readonly IDLSRepository _dlsRepository;

        public DLSRepositoryDecorator(IDLSRepository dlsRepository)
        {
            dlsRepository.ThrowIfNull(nameof(dlsRepository));
            _dlsRepository = dlsRepository;
        }

        public virtual BPTransactionDetailLookupResponse BPTransactionDetailLookup(bool isInTrainingMode, BPTransactionDetailLookupRequest bpTransactionDetailLookupRequest, IList<string> strPosIdList)
        {
            return _dlsRepository.BPTransactionDetailLookup(isInTrainingMode, bpTransactionDetailLookupRequest, strPosIdList);
        }

        public virtual async Task<BPTransactionDetailLookupResponse> BPTransactionDetailLookupAsync(bool isInTrainingMode, BPTransactionDetailLookupRequest bpTransactionDetailLookupRequest, IList<string> strPosIdList)
        {
            return await _dlsRepository.BPTransactionDetailLookupAsync(isInTrainingMode, bpTransactionDetailLookupRequest, strPosIdList);
        }

        public virtual DailyTransactionDetailLookupResponse DailyTransactionDetailLookup(bool isInTrainingMode, DailyTransactionDetailLookupRequest dailyTransactionDetailLookupRequest)
        {
            return _dlsRepository.DailyTransactionDetailLookup(isInTrainingMode, dailyTransactionDetailLookupRequest);
        }

        public virtual async Task<DailyTransactionDetailLookupResponse> DailyTransactionDetailLookupAsync(bool isInTrainingMode, DailyTransactionDetailLookupRequest dailyTransactionDetailLookupRequest)
        {
            return await _dlsRepository.DailyTransactionDetailLookupAsync(isInTrainingMode, dailyTransactionDetailLookupRequest);
        }

        public virtual MTTransactionDetailLookupResponse MTTransactionDetailLookup(bool isInTrainingMode, MTTransactionDetailLookupRequest mtTransactionDetailLookupRequest, IList<string> strPosIdList)
        {
            return _dlsRepository.MTTransactionDetailLookup(isInTrainingMode, mtTransactionDetailLookupRequest, strPosIdList);
        }

        public virtual async Task<MTTransactionDetailLookupResponse> MTTransactionDetailLookupAsync(bool isInTrainingMode, MTTransactionDetailLookupRequest mtTransactionDetailLookupRequest, IList<string> strPosIdList)
        {
            return await _dlsRepository.MTTransactionDetailLookupAsync(isInTrainingMode, mtTransactionDetailLookupRequest, strPosIdList);
        }
    }
}
