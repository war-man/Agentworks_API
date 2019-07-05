using DOMAIN = MoneyGram.DLS.DomainModel;
using SERVICE = MoneyGram.DLS.Service;

namespace MoneyGram.DLS.EntityMapper.DomainModelExtensions
{
    public static class AgentHeaderExtensions
    {
        public static SERVICE.AgentHeader ToService(this DOMAIN.AgentHeader req)
        {
            var serviceModel = DLSMapper.Map<DOMAIN.AgentHeader, SERVICE.AgentHeader>(req)
                .AdditionalOperations();
            return serviceModel;
        }

        public static SERVICE.AgentHeader AdditionalOperations(this SERVICE.AgentHeader svcModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(svcModel);
            return svcModel;
        }

        public static void DefineMappings()
        {
            DLSMapper.CreateMap<DOMAIN.AgentHeader, SERVICE.AgentHeader>();
        }
    }
}