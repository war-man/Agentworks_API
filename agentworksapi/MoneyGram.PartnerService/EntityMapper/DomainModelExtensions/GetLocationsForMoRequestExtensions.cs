using DOMAIN = MoneyGram.PartnerService.DomainModel;
using SERVICE = MoneyGram.PartnerService.Service;

namespace MoneyGram.PartnerService.EntityMapper.DomainModelExtensions
{
    public static class GetLocationsForMoRequestExtensions
    {
        public static SERVICE.GetLocationsForMoRequest ToService(this DOMAIN.LocationsForMoRequest req)
        {
            var serviceModel = PsMapper.Map<DOMAIN.LocationsForMoRequest, SERVICE.GetLocationsForMoRequest>(req)
                .AdditionalOperations();
            return serviceModel;
        }

        public static SERVICE.GetLocationsForMoRequest AdditionalOperations(this SERVICE.GetLocationsForMoRequest svcModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(svcModel);
            return svcModel;
        }

        public static void DefineMappings()
        {
            PsMapper.CreateMap<DOMAIN.LocationsForMoRequest, SERVICE.GetLocationsForMoRequest>()
                .ForMember(svs => svs.mainOfficeIdSpecified, opt => opt.MapFrom(dm => dm.MainOfficeId > 0))
                .ForMember(svs => svs.header, opt => opt.MapFrom(dm => dm.header.ToService()));
        }
    }
}