using DOMAIN = MoneyGram.PartnerService.DomainModel;
using SERVICE = MoneyGram.PartnerService.Service;

namespace MoneyGram.PartnerService.EntityMapper.DomainModelExtensions
{
    public static class GetPOEAgentLocationMoRequestExtensions
    {
        public static SERVICE.GetPOEAgentLocationMoRequest ToPOEAgentLocationMoRequestService(this DOMAIN.POEAgentLocationMoRequest req)
        {
            var serviceModel = PsMapper.Map<DOMAIN.POEAgentLocationMoRequest, SERVICE.GetPOEAgentLocationMoRequest>(req)
                .AdditionalOperations();
            return serviceModel;
        }

        public static SERVICE.GetPOEAgentLocationMoRequest AdditionalOperations(this SERVICE.GetPOEAgentLocationMoRequest svcModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(svcModel);
            return svcModel;
        }

        public static void DefineMappings()
        {
            PsMapper.CreateMap<DOMAIN.POEAgentLocationMoRequest, SERVICE.GetPOEAgentLocationMoRequest>()
                .ForMember(svs => svs.header, opt => opt.MapFrom(dm => dm.header.ToService()))
                .ForMember(svs => svs.POECode, opt => opt.MapFrom(dm => dm.POECode));
        }
    }
}