using System.Linq;
using DOMAIN = MoneyGram.PartnerService.DomainModel;
using SERVICE = MoneyGram.PartnerService.Service;

namespace MoneyGram.PartnerService.EntityMapper.ServiceModelExtensions
{
    public static class LocationsForMoResponseExtensions
    {
        public static DOMAIN.LocationsForMoResponse ToDomain(this SERVICE.GetLocationsForMoResponse req)
        {
            var domainModel = PsMapper.Map<SERVICE.GetLocationsForMoResponse, DOMAIN.LocationsForMoResponse>(req)
                .AdditionalOperations();
            return domainModel;
        }

        public static DOMAIN.LocationsForMoResponse AdditionalOperations(this DOMAIN.LocationsForMoResponse dmModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(dmModel);
            return dmModel;
        }

        public static void DefineMappings()
        {
            PsMapper.CreateMap<SERVICE.GetLocationsForMoResponse, DOMAIN.LocationsForMoResponse>()
                .ForMember(dm => dm.AgentList, opt => opt.MapFrom(svs => svs.agentList != null ? svs.agentList.ToList().ToAgentList() : null))
                .ForMember(dm => dm.header, opt => opt.MapFrom(svs => svs.header.ToDomain()));
        }
    }
}
