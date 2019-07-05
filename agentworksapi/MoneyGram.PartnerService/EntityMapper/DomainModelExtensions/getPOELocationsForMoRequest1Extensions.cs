using DOMAIN = MoneyGram.PartnerService.DomainModel;
using SERVICE = MoneyGram.PartnerService.Service;

namespace MoneyGram.PartnerService.EntityMapper.DomainModelExtensions
{
    public static class getPOELocationsForMoRequest1Extensions
    {
        public static SERVICE.getPOELocationsForMoRequest1 ToPOELocationsForMoRequest1Service(this DOMAIN.POELocationsForMoRequest req)
        {
            var serviceModel = PsMapper.Map<DOMAIN.POELocationsForMoRequest, SERVICE.getPOELocationsForMoRequest1>(req)
                .AdditionalOperations();
            return serviceModel;
        }

        public static SERVICE.getPOELocationsForMoRequest1 AdditionalOperations(this SERVICE.getPOELocationsForMoRequest1 svcModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(svcModel);
            return svcModel;
        }

        public static void DefineMappings()
        {
            PsMapper.CreateMap<DOMAIN.POELocationsForMoRequest, SERVICE.getPOELocationsForMoRequest1>()
                .ForMember(svs => svs.getPOELocationsForMoRequest, opt => opt.MapFrom(dm => dm.ToService()));
        }
    }
}