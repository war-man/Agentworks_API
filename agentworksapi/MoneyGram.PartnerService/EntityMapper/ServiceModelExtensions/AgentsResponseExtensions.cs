using System.Linq;
using DOMAIN = MoneyGram.PartnerService.DomainModel.Response;
using SERVICE = MoneyGram.PartnerService.Service;

namespace MoneyGram.PartnerService.EntityMapper.ServiceModelExtensions
{
    public static class AgentsResponseExtensions
    {
        public static DOMAIN.AgentsResponse ToDomain(this SERVICE.GetAgentsResponse req)
        {
            var domainModel = PsMapper.Map<SERVICE.GetAgentsResponse, DOMAIN.AgentsResponse>(req)
                .AdditionalOperations();
            return domainModel;
        }

        public static DOMAIN.AgentsResponse AdditionalOperations(this DOMAIN.AgentsResponse dmModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(dmModel);
            return dmModel;
        }

        public static void DefineMappings()
        {
            PsMapper.CreateMap<SERVICE.GetAgentsResponse, DOMAIN.AgentsResponse>()
                .ForMember(dm => dm.header, opt => opt.MapFrom(svs => svs.header.ToDomain()))
                .ForMember(dm => dm.AgentList, opt => opt.MapFrom(svs => svs.agentList != null ? svs.agentList.ToList().ToAgentLocList() : null));
        }
    }
}
