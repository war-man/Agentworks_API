using DOMAIN = MoneyGram.DLS.DomainModel;
using SERVICE = MoneyGram.DLS.Service;

namespace MoneyGram.DLS.EntityMapper.ServiceModelExtensions
{
    public static class ClientHeaderExtensions
    {
        public static DOMAIN.ClientHeader ToDomain(this SERVICE.ClientHeader req)
        {
            var domainModel = DLSMapper.Map<SERVICE.ClientHeader, DOMAIN.ClientHeader>(req)
                .AdditionalOperations();
            return domainModel;
        }

        public static DOMAIN.ClientHeader AdditionalOperations(this DOMAIN.ClientHeader dmModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(dmModel);
            return dmModel;
        }

        public static void DefineMappings()
        {
            DLSMapper.CreateMap<SERVICE.ClientHeader, DOMAIN.ClientHeader>();
        }
    }
}