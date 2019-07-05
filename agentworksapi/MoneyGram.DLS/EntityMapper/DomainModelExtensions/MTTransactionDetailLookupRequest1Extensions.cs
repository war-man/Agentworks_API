using DOMAIN = MoneyGram.DLS.DomainModel.Request;
using SERVICE = MoneyGram.DLS.Service;

namespace MoneyGram.DLS.EntityMapper.DomainModelExtensions
{
    public static class MTTransactionDetailLookupRequest1Extensions
    {
        public static SERVICE.MTTransactionDetailLookupRequest1 ToService(this DOMAIN.MTTransactionDetailLookupRequest req)
        {
            var serviceModel = DLSMapper.Map<DOMAIN.MTTransactionDetailLookupRequest, SERVICE.MTTransactionDetailLookupRequest1>(req)
                .AdditionalOperations();
            return serviceModel;
        }

        public static SERVICE.MTTransactionDetailLookupRequest1 AdditionalOperations(this SERVICE.MTTransactionDetailLookupRequest1 svcModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(svcModel);
            return svcModel;
        }

        public static void DefineMappings()
        {
            DLSMapper.CreateMap<DOMAIN.MTTransactionDetailLookupRequest, SERVICE.MTTransactionDetailLookupRequest1>()
                .ForMember(svc => svc.mtTransactionDetailLookupRequest, opt => opt.MapFrom(dm => dm.ToMTTransactionDetailLookupRequestService()));
        }
    }
}