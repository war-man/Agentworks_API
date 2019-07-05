using DOMAIN = MoneyGram.DLS.DomainModel;
using SERVICE = MoneyGram.DLS.Service;

namespace MoneyGram.DLS.EntityMapper.ServiceModelExtensions
{
    public static class RoutingContextHeaderExtensions
    {
        public static DOMAIN.RoutingContextHeader ToDomain(this SERVICE.RoutingContextHeader req)
        {
            var domainModel = DLSMapper.Map<SERVICE.RoutingContextHeader, DOMAIN.RoutingContextHeader>(req)
                .AdditionalOperations();
            return domainModel;
        }

        public static DOMAIN.RoutingContextHeader AdditionalOperations(this DOMAIN.RoutingContextHeader dmModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(dmModel);
            return dmModel;
        }

        public static void DefineMappings()
        {
            DLSMapper.CreateMap<SERVICE.RoutingContextHeader, DOMAIN.RoutingContextHeader>();
        }
    }
}