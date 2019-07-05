using DOMAIN = MoneyGram.DLS.DomainModel;
using SERVICE = MoneyGram.DLS.Service;

namespace MoneyGram.DLS.EntityMapper.ServiceModelExtensions
{
    public static class AgentHeaderExtensions
    {
        public static DOMAIN.AgentHeader ToDomain(this SERVICE.AgentHeader req)
        {
            var domainModel = DLSMapper.Map<SERVICE.AgentHeader, DOMAIN.AgentHeader>(req)
                .AdditionalOperations();
            return domainModel;
        }

        public static DOMAIN.AgentHeader AdditionalOperations(this DOMAIN.AgentHeader dmModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(dmModel);
            return dmModel;
        }

        public static void DefineMappings()
        {
            DLSMapper.CreateMap<SERVICE.AgentHeader, DOMAIN.AgentHeader>();
        }
    }
}