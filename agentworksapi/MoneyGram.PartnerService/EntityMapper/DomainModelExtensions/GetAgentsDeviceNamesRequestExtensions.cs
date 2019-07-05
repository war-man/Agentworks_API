using DOMAIN = MoneyGram.PartnerService.DomainModel.Request;
using SERVICE = MoneyGram.PartnerService.Service;

namespace MoneyGram.PartnerService.EntityMapper.DomainModelExtensions
{
    public static class GetAgentsDeviceNamesRequestExtensions
    {
        public static SERVICE.GetAgentsDeviceNamesRequest ToService(this DOMAIN.AgentsDeviceNamesRequest req)
        {
            var serviceModel = PsMapper.Map<DOMAIN.AgentsDeviceNamesRequest, SERVICE.GetAgentsDeviceNamesRequest>(req)
                .AdditionalOperations();
            return serviceModel;
        }

        public static SERVICE.GetAgentsDeviceNamesRequest AdditionalOperations(this SERVICE.GetAgentsDeviceNamesRequest svcModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(svcModel);
            return svcModel;
        }

        public static void DefineMappings()
        {
            PsMapper.CreateMap<DOMAIN.AgentsDeviceNamesRequest, SERVICE.GetAgentsDeviceNamesRequest>()
                .ForMember(svs => svs.header, opt => opt.MapFrom(dm => dm.header.ToService()))
                .ForMember(svs => svs.agentPosDeviceList, opt => opt.MapFrom(dm => dm.AgentPosDeviceList != null ? dm.AgentPosDeviceList.ToGetAgentsDeviceNamesRequestList().ToArray() : null));
        }
    }
}