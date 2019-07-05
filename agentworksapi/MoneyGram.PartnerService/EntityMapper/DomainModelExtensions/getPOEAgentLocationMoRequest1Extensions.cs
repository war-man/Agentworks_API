using DOMAIN = MoneyGram.PartnerService.DomainModel;
using SERVICE = MoneyGram.PartnerService.Service;

namespace MoneyGram.PartnerService.EntityMapper.DomainModelExtensions
{
    public static class getPOEAgentLocationMoRequest1Extensions
    {
        public static SERVICE.getPOEAgentLocationMoRequest1 ToService(this DOMAIN.POEAgentLocationMoRequest req)
        {
            var serviceModel = PsMapper.Map<DOMAIN.POEAgentLocationMoRequest, SERVICE.getPOEAgentLocationMoRequest1>(req)
                .AdditionalOperations();
            return serviceModel;
        }

        public static SERVICE.getPOEAgentLocationMoRequest1 AdditionalOperations(this SERVICE.getPOEAgentLocationMoRequest1 svcModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(svcModel);
            return svcModel;
        }

        public static void DefineMappings()
        {
            PsMapper.CreateMap<DOMAIN.POEAgentLocationMoRequest, SERVICE.getPOEAgentLocationMoRequest1>()
                .ForMember(svs => svs.getPOEAgentLocationMoRequest, opt => opt.MapFrom(dm => dm.ToPOEAgentLocationMoRequestService()));
        }
    }
}