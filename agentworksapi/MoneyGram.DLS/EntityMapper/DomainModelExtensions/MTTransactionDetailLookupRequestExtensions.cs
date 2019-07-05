using DOMAIN = MoneyGram.DLS.DomainModel.Request;
using SERVICE = MoneyGram.DLS.Service;

namespace MoneyGram.DLS.EntityMapper.DomainModelExtensions
{
    public static class MTTransactionDetailLookupRequestExtensions
    {
        public static SERVICE.MTTransactionDetailLookupRequest ToMTTransactionDetailLookupRequestService(this DOMAIN.MTTransactionDetailLookupRequest req)
        {
            var serviceModel = DLSMapper.Map<DOMAIN.MTTransactionDetailLookupRequest, SERVICE.MTTransactionDetailLookupRequest>(req)
                .AdditionalOperations();
            return serviceModel;
        }

        public static SERVICE.MTTransactionDetailLookupRequest AdditionalOperations(this SERVICE.MTTransactionDetailLookupRequest svcModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(svcModel);
            return svcModel;
        }

        public static void DefineMappings()
        {
            DLSMapper.CreateMap<DOMAIN.MTTransactionDetailLookupRequest, SERVICE.MTTransactionDetailLookupRequest>()
                .ForMember(svc => svc.header, opt => opt.MapFrom(dm => dm.header.ToService()))
                .ForMember(svc => svc.dateTime, opt => opt.MapFrom(dm => dm.StartDate));
        }
    }
}