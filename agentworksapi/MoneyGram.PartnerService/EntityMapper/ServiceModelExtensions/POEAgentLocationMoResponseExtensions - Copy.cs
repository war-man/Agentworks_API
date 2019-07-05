using System.Linq;
using DOMAIN = MoneyGram.PartnerService.DomainModel;
using SERVICE = MoneyGram.PartnerService.Service;

namespace MoneyGram.PartnerService.EntityMapper.ServiceModelExtensions
{
    public static class POEAgentLocationMoResponseExtensions
    {
        public static DOMAIN.POEAgentLocationMoResponse ToDomain(this SERVICE.GetPOEAgentLocationMoResponse req)
        {
            var domainModel = PsMapper.Map<SERVICE.GetPOEAgentLocationMoResponse, DOMAIN.POEAgentLocationMoResponse>(req)
                .AdditionalOperations();
            return domainModel;
        }

        public static DOMAIN.POEAgentLocationMoResponse AdditionalOperations(this DOMAIN.POEAgentLocationMoResponse dmModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(dmModel);
            return dmModel;
        }

        public static void DefineMappings()
        {
            PsMapper.CreateMap<SERVICE.GetPOEAgentLocationMoResponse, DOMAIN.POEAgentLocationMoResponse>()
                .ForMember(dm => dm.AgentList, opt => opt.MapFrom(svs => svs.agentList != null ? svs.agentList.ToList().ToAgentList() : null))
                .ForMember(dm => dm.header, opt => opt.MapFrom(svs => svs.header.ToDomain()));
        }
    }
}
