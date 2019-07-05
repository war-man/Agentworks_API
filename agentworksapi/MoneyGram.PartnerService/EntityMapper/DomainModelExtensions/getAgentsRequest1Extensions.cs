using MoneyGram.Common;
using DOMAIN = MoneyGram.PartnerService.DomainModel.Request;
using SERVICE = MoneyGram.PartnerService.Service;

namespace MoneyGram.PartnerService.EntityMapper.DomainModelExtensions
{
    public static class getAgentsRequest1Extensions
    {
        public static SERVICE.getAgentsRequest1 ToAgentsRequest1Service(this DOMAIN.AgentsRequest req)
        {
            var serviceModel = PsMapper.Map<DOMAIN.AgentsRequest, SERVICE.getAgentsRequest1>(req)
                .AdditionalOperations(req);
            return serviceModel;
        }

        public static SERVICE.getAgentsRequest1 AdditionalOperations(this SERVICE.getAgentsRequest1 svcModel, DOMAIN.AgentsRequest domainModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(svcModel);
            ServiceTransformExtensions.ProcessSpecifiedDomainToService(svcModel, domainModel);
            return svcModel;
        }

        public static void DefineMappings()
        {
            PsMapper.CreateMap<DOMAIN.AgentsRequest, SERVICE.getAgentsRequest1>()
                .ForMember(svs => svs.getAgentsRequest, opt => opt.MapFrom(dm => dm.ToService()));
        }
    }
}