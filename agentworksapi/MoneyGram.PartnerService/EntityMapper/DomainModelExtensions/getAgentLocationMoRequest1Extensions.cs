using DOMAIN = MoneyGram.PartnerService.DomainModel;
using SERVICE = MoneyGram.PartnerService.Service;

namespace MoneyGram.PartnerService.EntityMapper.DomainModelExtensions
{
    public static class getAgentLocationMoRequest1Extensions
    {
        public static SERVICE.getAgentLocationMoRequest1 ToService(this DOMAIN.AgentLocationMoRequest req)
        {
            var serviceModel = PsMapper.Map<DOMAIN.AgentLocationMoRequest, SERVICE.getAgentLocationMoRequest1>(req)
                .AdditionalOperations();
            return serviceModel;
        }

        public static SERVICE.getAgentLocationMoRequest1 AdditionalOperations(this SERVICE.getAgentLocationMoRequest1 svcModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(svcModel);
            return svcModel;
        }

        public static void DefineMappings()
        {
            PsMapper.CreateMap<DOMAIN.AgentLocationMoRequest, SERVICE.getAgentLocationMoRequest1>()
                .ForMember(svs => svs.getAgentLocationMoRequest, opt => opt.MapFrom(dm => dm.ToAgentLocationMoRequestService()));
        }
    }
}