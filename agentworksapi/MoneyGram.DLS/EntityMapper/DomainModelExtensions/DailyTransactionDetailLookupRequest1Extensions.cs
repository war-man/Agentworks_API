using DOMAIN = MoneyGram.DLS.DomainModel.Request;
using SERVICE = MoneyGram.DLS.Service;

namespace MoneyGram.DLS.EntityMapper.DomainModelExtensions
{
    public static class DailyTransactionDetailLookupRequest1Extensions
    {
        public static SERVICE.DailyTransactionDetailLookupRequest1 ToService(this DOMAIN.DailyTransactionDetailLookupRequest req)
        {
            var serviceModel = DLSMapper.Map<DOMAIN.DailyTransactionDetailLookupRequest, SERVICE.DailyTransactionDetailLookupRequest1>(req)
                .AdditionalOperations();
            return serviceModel;
        }

        public static SERVICE.DailyTransactionDetailLookupRequest1 AdditionalOperations(this SERVICE.DailyTransactionDetailLookupRequest1 svcModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(svcModel);
            return svcModel;
        }

        public static void DefineMappings()
        {
            DLSMapper.CreateMap<DOMAIN.DailyTransactionDetailLookupRequest, SERVICE.DailyTransactionDetailLookupRequest1>()
            .ForMember(svc => svc.dailyTransactionDetailLookupRequest, opt => opt.MapFrom(dm => dm.ToDailyTransactionDetailLookupService()));
        }
    }
}