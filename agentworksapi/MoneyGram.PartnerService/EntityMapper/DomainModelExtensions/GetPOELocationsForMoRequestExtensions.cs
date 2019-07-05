using DOMAIN = MoneyGram.PartnerService.DomainModel;
using SERVICE = MoneyGram.PartnerService.Service;

namespace MoneyGram.PartnerService.EntityMapper.DomainModelExtensions
{
    public static class GetPOELocationsForMoRequestExtensions
    {
        public static SERVICE.GetPOELocationsForMoRequest ToService(this DOMAIN.POELocationsForMoRequest req)
        {
            var serviceModel = PsMapper.Map<DOMAIN.POELocationsForMoRequest, SERVICE.GetPOELocationsForMoRequest>(req)
                .AdditionalOperations();
            return serviceModel;
        }

        public static SERVICE.GetPOELocationsForMoRequest AdditionalOperations(this SERVICE.GetPOELocationsForMoRequest svcModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(svcModel);
            return svcModel;
        }

        public static void DefineMappings()
        {
            PsMapper.CreateMap<DOMAIN.POELocationsForMoRequest, SERVICE.GetPOELocationsForMoRequest>()
                .ForMember(svs => svs.mainOfficeIdSpecified, opt => opt.MapFrom(dm => dm.MainOfficeId > 0))
                .ForMember(svs => svs.header, opt => opt.MapFrom(dm => dm.header.ToService()))
                .ForMember(svs => svs.POECode, opt => opt.MapFrom(dm => dm.POECode))
                .ForMember(svs => svs.POECodeSpecified, opt => opt.MapFrom(x => x.POECode != null));

        }
    }
}