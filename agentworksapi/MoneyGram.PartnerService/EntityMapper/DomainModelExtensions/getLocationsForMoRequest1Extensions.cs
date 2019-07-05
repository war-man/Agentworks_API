using DOMAIN = MoneyGram.PartnerService.DomainModel;
using SERVICE = MoneyGram.PartnerService.Service;

namespace MoneyGram.PartnerService.EntityMapper.DomainModelExtensions
{
    public static class getLocationsForMoRequest1Extensions
    {
        public static SERVICE.getLocationsForMoRequest1 ToLocationsForMoRequest1Service(this DOMAIN.LocationsForMoRequest req)
        {
            var serviceModel = PsMapper.Map<DOMAIN.LocationsForMoRequest, SERVICE.getLocationsForMoRequest1>(req)
                .AdditionalOperations();
            return serviceModel;
        }

        public static SERVICE.getLocationsForMoRequest1 AdditionalOperations(this SERVICE.getLocationsForMoRequest1 svcModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(svcModel);
            return svcModel;
        }

        public static void DefineMappings()
        {
            PsMapper.CreateMap<DOMAIN.LocationsForMoRequest, SERVICE.getLocationsForMoRequest1>()
                .ForMember(svs => svs.getLocationsForMoRequest, opt => opt.MapFrom(dm => dm.ToService()));
        }
    }
}