using DOMAIN = MoneyGram.PartnerService.DomainModel.Request;
using SERVICE = MoneyGram.PartnerService.Service;

namespace MoneyGram.PartnerService.EntityMapper.DomainModelExtensions
{
    public static class GetAgentsRequestExtensions
    {
        public static SERVICE.GetAgentsRequest ToService(this DOMAIN.AgentsRequest req)
        {
            var serviceModel = PsMapper.Map<DOMAIN.AgentsRequest, SERVICE.GetAgentsRequest>(req)
                .AdditionalOperations();
            return serviceModel;
        }

        public static SERVICE.GetAgentsRequest AdditionalOperations(this SERVICE.GetAgentsRequest svcModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(svcModel);
            return svcModel;
        }

        public static void DefineMappings()
        {
            PsMapper.CreateMap<DOMAIN.AgentsRequest, SERVICE.GetAgentsRequest>()
                .ForMember(svs => svs.header, opt => opt.MapFrom(dm => dm.header.ToService()));
        }
    }
}