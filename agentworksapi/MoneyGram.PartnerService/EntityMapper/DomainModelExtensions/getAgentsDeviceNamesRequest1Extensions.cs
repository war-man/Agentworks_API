using DOMAIN = MoneyGram.PartnerService.DomainModel.Request;
using SERVICE = MoneyGram.PartnerService.Service;

namespace MoneyGram.PartnerService.EntityMapper.DomainModelExtensions
{
    public static class getAgentsDeviceNamesRequest1Extensions
    {
        public static SERVICE.getAgentsDeviceNamesRequest1 ToAgentsDeviceNamesRequest1Service(this DOMAIN.AgentsDeviceNamesRequest req)
        {
            var serviceModel = PsMapper.Map<DOMAIN.AgentsDeviceNamesRequest, SERVICE.getAgentsDeviceNamesRequest1>(req)
                .AdditionalOperations();
            return serviceModel;
        }

        public static SERVICE.getAgentsDeviceNamesRequest1 AdditionalOperations(this SERVICE.getAgentsDeviceNamesRequest1 svcModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(svcModel);
            return svcModel;
        }

        public static void DefineMappings()
        {
            PsMapper.CreateMap<DOMAIN.AgentsDeviceNamesRequest, SERVICE.getAgentsDeviceNamesRequest1>()
                .ForMember(svs => svs.getAgentsDeviceNamesRequest, opt => opt.MapFrom(dm => dm.ToService()));
        }
    }
}