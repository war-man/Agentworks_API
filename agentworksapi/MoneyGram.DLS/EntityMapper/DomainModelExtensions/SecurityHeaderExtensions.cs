using DOMAIN = MoneyGram.DLS.DomainModel;
using SERVICE = MoneyGram.DLS.Service;

namespace MoneyGram.DLS.EntityMapper.DomainModelExtensions
{
    public static class SecurityHeaderExtensions
    {
        public static SERVICE.SecurityHeader ToService(this DOMAIN.SecurityHeader req)
        {
            var serviceModel = DLSMapper.Map<DOMAIN.SecurityHeader, SERVICE.SecurityHeader>(req)
                .AdditionalOperations();
            return serviceModel;
        }

        public static SERVICE.SecurityHeader AdditionalOperations(this SERVICE.SecurityHeader svcModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(svcModel);
            return svcModel;
        }

        public static void DefineMappings()
        {
            DLSMapper.CreateMap<DOMAIN.SecurityHeader, SERVICE.SecurityHeader>();
        }
    }
}