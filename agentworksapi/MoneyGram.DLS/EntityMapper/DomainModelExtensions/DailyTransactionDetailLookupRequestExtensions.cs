using DOMAIN = MoneyGram.DLS.DomainModel.Request;
using SERVICE = MoneyGram.DLS.Service;

namespace MoneyGram.DLS.EntityMapper.DomainModelExtensions
{
    public static class DailyTransactionDetailLookupRequestExtensions
    {
        public static SERVICE.DailyTransactionDetailLookupRequest ToDailyTransactionDetailLookupService(this DOMAIN.DailyTransactionDetailLookupRequest req)
        {
            var serviceModel = DLSMapper.Map<DOMAIN.DailyTransactionDetailLookupRequest, SERVICE.DailyTransactionDetailLookupRequest>(req)
                .AdditionalOperations();
            return serviceModel;
        }

        public static SERVICE.DailyTransactionDetailLookupRequest AdditionalOperations(this SERVICE.DailyTransactionDetailLookupRequest svcModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(svcModel);
            return svcModel;
        }

        public static void DefineMappings()
        {
            DLSMapper.CreateMap<DOMAIN.DailyTransactionDetailLookupRequest, SERVICE.DailyTransactionDetailLookupRequest>()
                .ForMember(svc => svc.header, opt => opt.MapFrom(dm => dm.header.ToService()))
                .ForMember(svc => svc.dateTime, opt => opt.MapFrom(dm => dm.StartDate));
        }
    }
}