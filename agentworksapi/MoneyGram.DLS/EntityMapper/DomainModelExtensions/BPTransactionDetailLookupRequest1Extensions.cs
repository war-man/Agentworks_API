using DOMAIN = MoneyGram.DLS.DomainModel.Request;
using SERVICE = MoneyGram.DLS.Service;

namespace MoneyGram.DLS.EntityMapper.DomainModelExtensions
{
    public static class BPTransactionDetailLookupRequest1Extensions
    {
        public static SERVICE.BPTransactionDetailLookupRequest1 ToService(this DOMAIN.BPTransactionDetailLookupRequest req)
        {
            var serviceModel = DLSMapper.Map<DOMAIN.BPTransactionDetailLookupRequest, SERVICE.BPTransactionDetailLookupRequest1>(req)
                .AdditionalOperations();
            return serviceModel;
        }

        public static SERVICE.BPTransactionDetailLookupRequest1 AdditionalOperations(this SERVICE.BPTransactionDetailLookupRequest1 svcModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(svcModel);
            return svcModel;
        }

        public static void DefineMappings()
        {
            DLSMapper.CreateMap<DOMAIN.BPTransactionDetailLookupRequest, SERVICE.BPTransactionDetailLookupRequest1>()
                .ForMember(svc => svc.bpTransactionDetailLookupRequest, opt => opt.MapFrom(dm => dm.ToBPTransactionDetailLookupService()));
        }
    }
}