using DOMAIN = MoneyGram.PartnerService.DomainModel.Request;
using SERVICE = MoneyGram.PartnerService.Service;

namespace MoneyGram.PartnerService.EntityMapper.DomainModelExtensions
{
    public static class GetAgentPasswordRequestExtensions
    {
        public static SERVICE.GetAgentPasswordRequest ToAgentPasswordRequestService(this DOMAIN.AgentPasswordRequest req)
        {
            var serviceModel = PsMapper.Map<DOMAIN.AgentPasswordRequest, SERVICE.GetAgentPasswordRequest>(req)
                .AdditionalOperations();
            return serviceModel;
        }

        public static SERVICE.GetAgentPasswordRequest AdditionalOperations(this SERVICE.GetAgentPasswordRequest svcModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(svcModel);
            return svcModel;
        }

        public static void DefineMappings()
        {
            PsMapper.CreateMap<DOMAIN.AgentPasswordRequest, SERVICE.GetAgentPasswordRequest>()
                .ForMember(svs => svs.header, opt => opt.MapFrom(dm => dm.header.ToService()))
                .ForMember(svs => svs.agentIdSpecified, opt => opt.MapFrom(x => true))
                .ForMember(svs => svs.posNumberSpecified, opt => opt.MapFrom(x => true));
        }
    }
}