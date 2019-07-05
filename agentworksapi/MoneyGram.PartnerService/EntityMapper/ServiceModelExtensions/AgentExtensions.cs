using DOMAIN = MoneyGram.PartnerService.DomainModel;
using SERVICE = MoneyGram.PartnerService.Service;

namespace MoneyGram.PartnerService.EntityMapper.ServiceModelExtensions
{
    public static class AgentExtensions
    {
        public static DOMAIN.Agent ToDomain(this SERVICE.Agent req)
        {
            var domainModel = PsMapper.Map<SERVICE.Agent, DOMAIN.Agent>(req)
                .AdditionalOperations();
            return domainModel;
        }

        public static DOMAIN.Agent AdditionalOperations(this DOMAIN.Agent dmModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(dmModel);
            return dmModel;
        }

        public static void DefineMappings()
        {
            PsMapper.CreateMap<SERVICE.Agent, DOMAIN.Agent>()
                .Ignore(src => src.Pos)
                .Ignore(src => src.activityType)
                .Ignore(src => src.DefaultWidget);
        }
    }
}
