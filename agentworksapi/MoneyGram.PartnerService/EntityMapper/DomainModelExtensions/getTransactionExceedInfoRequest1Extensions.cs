using DOMAIN = MoneyGram.PartnerService.DomainModel;
using SERVICE = MoneyGram.PartnerService.Service;

namespace MoneyGram.PartnerService.EntityMapper.DomainModelExtensions
{
    public static class getTransactionExceedInfoRequest1Extensions
    {
        public static SERVICE.getTransactionExceedInfoRequest1 ToTransactionExceedInfoRequest1Service(this DOMAIN.TransactionExceedInfoRequest req)
        {
            var serviceModel = PsMapper.Map<DOMAIN.TransactionExceedInfoRequest, SERVICE.getTransactionExceedInfoRequest1>(req)
                .AdditionalOperations();
            return serviceModel;
        }

        public static SERVICE.getTransactionExceedInfoRequest1 AdditionalOperations(this SERVICE.getTransactionExceedInfoRequest1 svcModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(svcModel);
            return svcModel;
        }

        public static void DefineMappings()
        {
            PsMapper.CreateMap<DOMAIN.TransactionExceedInfoRequest, SERVICE.getTransactionExceedInfoRequest1>()
                .ForMember(svs => svs.getTransactionExceedInfoRequest, opt => opt.MapFrom(dm => dm.ToService()));
        }
    }
}