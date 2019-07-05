using DOMAIN = MoneyGram.PartnerService.DomainModel;
using SERVICE = MoneyGram.PartnerService.Service;

namespace MoneyGram.PartnerService.EntityMapper.DomainModelExtensions
{
    public static class GetTransactionExceedInfoRequestExtensions
    {
        public static SERVICE.GetTransactionExceedInfoRequest ToService(this DOMAIN.TransactionExceedInfoRequest req)
        {
            var serviceModel = PsMapper.Map<DOMAIN.TransactionExceedInfoRequest, SERVICE.GetTransactionExceedInfoRequest>(req)
                .AdditionalOperations();
            return serviceModel;
        }

        public static SERVICE.GetTransactionExceedInfoRequest AdditionalOperations(this SERVICE.GetTransactionExceedInfoRequest svcModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(svcModel);
            return svcModel;
        }

        public static void DefineMappings()
        {
            PsMapper.CreateMap<DOMAIN.TransactionExceedInfoRequest, SERVICE.GetTransactionExceedInfoRequest>()
                .ForMember(svs => svs.agentIdSpecified, opt => opt.MapFrom(dm => dm.AgentIdFieldSpecified))
                .ForMember(svs => svs.transactionDateSpecified, opt => opt.MapFrom(dm => dm.TransactionDateFieldSpecified))
                .ForMember(svs => svs.header, opt => opt.MapFrom(dm => dm.header.ToService()));
        }
    }
}