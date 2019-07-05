using DOMAIN = MoneyGram.DLS.DomainModel;
using SERVICE = MoneyGram.DLS.Service;

namespace MoneyGram.DLS.EntityMapper.DomainModelExtensions
{
    public static class ClientHeaderExtensions
    {
        public static SERVICE.ClientHeader ToService(this DOMAIN.ClientHeader req)
        {
            var serviceModel = DLSMapper.Map<DOMAIN.ClientHeader, SERVICE.ClientHeader>(req)
                .AdditionalOperations();
            return serviceModel;
        }

        public static SERVICE.ClientHeader AdditionalOperations(this SERVICE.ClientHeader svcModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(svcModel);
            return svcModel;
        }

        public static void DefineMappings()
        {
            DLSMapper.CreateMap<DOMAIN.ClientHeader, SERVICE.ClientHeader>();
        }
    }
}