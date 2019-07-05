using System.Collections.Generic;
using MoneyGram.DLS.DomainModel.Request;
using MoneyGram.DLS.DomainModel.Response;

namespace MoneyGram.DLS
{
    public class DLSRepositoryTraining : DLSRepositoryDecorator
    {
        private readonly ITrainingModeRepository _trainingModeRepository;

        public DLSRepositoryTraining(IDLSRepository dlsRepository, ITrainingModeRepository trainingModeRepository) : base(dlsRepository)
        {
            _trainingModeRepository = trainingModeRepository;
        }

        public override BPTransactionDetailLookupResponse BPTransactionDetailLookup(bool isInTrainingMode,
            BPTransactionDetailLookupRequest bpTransactionDetailLookupRequest, IList<string> strPosIdList)
        {
            if (isInTrainingMode)
                return _trainingModeRepository.MockBPTransactionDetailLookup(strPosIdList);
            else
                return base.BPTransactionDetailLookup(isInTrainingMode, bpTransactionDetailLookupRequest, strPosIdList);
        }

        public override MTTransactionDetailLookupResponse MTTransactionDetailLookup(bool isInTrainingMode,
            MTTransactionDetailLookupRequest mtTransactionDetailLookupRequest, IList<string> strPosIdList)
        {
            if (isInTrainingMode)
                return _trainingModeRepository.MockMTTransactionDetailLookup(strPosIdList);
            else
                return base.MTTransactionDetailLookup(isInTrainingMode, mtTransactionDetailLookupRequest, strPosIdList);
        }
    }
}
