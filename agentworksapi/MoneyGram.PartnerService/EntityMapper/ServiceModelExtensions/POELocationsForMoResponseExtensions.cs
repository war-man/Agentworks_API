using System.Linq;
using DOMAIN = MoneyGram.PartnerService.DomainModel;
using SERVICE = MoneyGram.PartnerService.Service;

namespace MoneyGram.PartnerService.EntityMapper.ServiceModelExtensions
{
    public static class POELocationsForMoResponseExtensions
    {
        public static DOMAIN.POELocationsForMoResponse ToDomain(this SERVICE.GetPOELocationsForMoResponse req)
        {
            var domainModel = PsMapper.Map<SERVICE.GetPOELocationsForMoResponse, DOMAIN.POELocationsForMoResponse>(req)
                .AdditionalOperations();
            return domainModel;
        }

        public static DOMAIN.POELocationsForMoResponse AdditionalOperations(this DOMAIN.POELocationsForMoResponse dmModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(dmModel);
            return dmModel;
        }

        public static void DefineMappings()
        {
            PsMapper.CreateMap<SERVICE.GetPOELocationsForMoResponse, DOMAIN.POELocationsForMoResponse>()
                .ForMember(dm => dm.AgentList, opt => opt.MapFrom(svs => svs.agentList != null ? svs.agentList.ToList().ToAgentList() : null))
                .ForMember(dm => dm.header, opt => opt.MapFrom(svs => svs.header.ToDomain()));
        }
    }

}
