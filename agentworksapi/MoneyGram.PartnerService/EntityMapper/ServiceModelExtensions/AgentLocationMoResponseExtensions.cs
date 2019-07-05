using System.Linq;
using DOMAIN = MoneyGram.PartnerService.DomainModel;
using SERVICE = MoneyGram.PartnerService.Service;

namespace MoneyGram.PartnerService.EntityMapper.ServiceModelExtensions
{
    public static class AgentLocationMoResponseExtensions
    {
        public static DOMAIN.AgentLocationMoResponse ToDomain(this SERVICE.GetAgentLocationMoResponse req)
        {
            var domainModel = PsMapper.Map<SERVICE.GetAgentLocationMoResponse, DOMAIN.AgentLocationMoResponse>(req)
                .AdditionalOperations();
            return domainModel;
        }

        public static DOMAIN.AgentLocationMoResponse AdditionalOperations(this DOMAIN.AgentLocationMoResponse dmModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(dmModel);
            return dmModel;
        }

        public static void DefineMappings()
        {
            PsMapper.CreateMap<SERVICE.GetAgentLocationMoResponse, DOMAIN.AgentLocationMoResponse>()
                .ForMember(dm => dm.AgentList, opt => opt.MapFrom(svs => svs.agentList != null ? svs.agentList.ToList().ToAgentList() : null))
                .ForMember(dm => dm.header, opt => opt.MapFrom(svs => svs.header.ToDomain()));
        }
    }
}
