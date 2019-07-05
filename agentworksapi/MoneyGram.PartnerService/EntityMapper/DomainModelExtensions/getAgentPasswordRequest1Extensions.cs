using DOMAIN = MoneyGram.PartnerService.DomainModel.Request;
using SERVICE = MoneyGram.PartnerService.Service;

namespace MoneyGram.PartnerService.EntityMapper.DomainModelExtensions
{
    public static class getAgentPasswordRequest1Extensions
    {
        public static SERVICE.getAgentPasswordRequest1 ToService(this DOMAIN.AgentPasswordRequest req)
        {
            var serviceModel = PsMapper.Map<DOMAIN.AgentPasswordRequest, SERVICE.getAgentPasswordRequest1>(req)
                .AdditionalOperations();
            return serviceModel;
        }

        public static SERVICE.getAgentPasswordRequest1 AdditionalOperations(this SERVICE.getAgentPasswordRequest1 svcModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(svcModel);
            return svcModel;
        }

        public static void DefineMappings()
        {
            PsMapper.CreateMap<DOMAIN.AgentPasswordRequest, SERVICE.getAgentPasswordRequest1>()
                .ForMember(svs => svs.getAgentPasswordRequest, opt => opt.MapFrom(dm => dm.ToAgentPasswordRequestService()));
        }
    }
}