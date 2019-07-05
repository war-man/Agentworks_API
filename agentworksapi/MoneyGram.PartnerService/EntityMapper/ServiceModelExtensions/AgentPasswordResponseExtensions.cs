using DOMAIN = MoneyGram.PartnerService.DomainModel.Response;
using SERVICE = MoneyGram.PartnerService.Service;

namespace MoneyGram.PartnerService.EntityMapper.ServiceModelExtensions
{
    public static class AgentPasswordResponseExtensions
    {
        public static DOMAIN.AgentPasswordResponse ToDomain(this SERVICE.GetAgentPasswordResponse req)
        {
            var domainModel = PsMapper.Map<SERVICE.GetAgentPasswordResponse, DOMAIN.AgentPasswordResponse>(req)
                .AdditionalOperations();
            return domainModel;
        }

        public static DOMAIN.AgentPasswordResponse AdditionalOperations(this DOMAIN.AgentPasswordResponse dmModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(dmModel);
            return dmModel;
        }

        public static void DefineMappings()
        {
            PsMapper.CreateMap<SERVICE.GetAgentPasswordResponse, DOMAIN.AgentPasswordResponse>()
                .ForMember(dm => dm.header, opt => opt.MapFrom(svs => svs.header.ToDomain()));
        }
    }
}
