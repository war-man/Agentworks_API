using DOMAIN = MoneyGram.PartnerService.DomainModel;
using SERVICE = MoneyGram.PartnerService.Service;

namespace MoneyGram.PartnerService.EntityMapper.DomainModelExtensions
{
    public static class GetAgentLocationMoRequestExtensions
    {
        public static SERVICE.GetAgentLocationMoRequest ToAgentLocationMoRequestService(this DOMAIN.AgentLocationMoRequest req)
        {
            var serviceModel = PsMapper.Map<DOMAIN.AgentLocationMoRequest, SERVICE.GetAgentLocationMoRequest>(req)
                .AdditionalOperations();
            return serviceModel;
        }

        public static SERVICE.GetAgentLocationMoRequest AdditionalOperations(this SERVICE.GetAgentLocationMoRequest svcModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(svcModel);
            return svcModel;
        }

        public static void DefineMappings()
        {
            PsMapper.CreateMap<DOMAIN.AgentLocationMoRequest, SERVICE.GetAgentLocationMoRequest>()
                .ForMember(svs => svs.header, opt => opt.MapFrom(dm => dm.header.ToService()));
        }
    }
}