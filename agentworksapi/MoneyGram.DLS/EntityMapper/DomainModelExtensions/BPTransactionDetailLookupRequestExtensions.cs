using DOMAIN = MoneyGram.DLS.DomainModel.Request;
using SERVICE = MoneyGram.DLS.Service;

namespace MoneyGram.DLS.EntityMapper.DomainModelExtensions
{
    public static class BPTransactionDetailLookupRequestExtensions
    {
        public static SERVICE.BPTransactionDetailLookupRequest ToBPTransactionDetailLookupService(this DOMAIN.BPTransactionDetailLookupRequest req)
        {
            var serviceModel = DLSMapper.Map<DOMAIN.BPTransactionDetailLookupRequest, SERVICE.BPTransactionDetailLookupRequest>(req)
                .AdditionalOperations();
            return serviceModel;
        }

        public static SERVICE.BPTransactionDetailLookupRequest AdditionalOperations(this SERVICE.BPTransactionDetailLookupRequest svcModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(svcModel);
            return svcModel;
        }

        public static void DefineMappings()
        {
            DLSMapper.CreateMap<DOMAIN.BPTransactionDetailLookupRequest, SERVICE.BPTransactionDetailLookupRequest>()
                .ForMember(svc => svc.header, opt => opt.MapFrom(dm => dm.header.ToService()))
                .ForMember(svc => svc.dateTime, opt => opt.MapFrom(dm => dm.StartDate));
        }
    }
}