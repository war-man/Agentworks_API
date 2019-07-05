using System.Linq;
using DOMAIN = MoneyGram.PartnerService.DomainModel.Response;
using SERVICE = MoneyGram.PartnerService.Service;

namespace MoneyGram.PartnerService.EntityMapper.ServiceModelExtensions
{
    public static class AgentsDeviceNamesResponseExtensions
    {
        public static DOMAIN.AgentsDeviceNamesResponse ToDomain(this SERVICE.GetAgentsDeviceNamesResponse req)
        {
            var domainModel = PsMapper.Map<SERVICE.GetAgentsDeviceNamesResponse, DOMAIN.AgentsDeviceNamesResponse>(req)
                .AdditionalOperations();
            return domainModel;
        }

        public static DOMAIN.AgentsDeviceNamesResponse AdditionalOperations(this DOMAIN.AgentsDeviceNamesResponse dmModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(dmModel);
            return dmModel;
        }

        public static void DefineMappings()
        {
            PsMapper.CreateMap<SERVICE.GetAgentsDeviceNamesResponse, DOMAIN.AgentsDeviceNamesResponse>()
                .ForMember(dm => dm.AgentPosDeviceListResult, opt => opt.MapFrom(svs => svs.agentPosDeviceListResult != null ? svs.agentPosDeviceListResult.ToList().ToAgentPosDeviceList() : null))
                .ForMember(dm => dm.header, opt => opt.MapFrom(svs => svs.header.ToDomain()));
        }
    }
}
