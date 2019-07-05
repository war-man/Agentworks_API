using DOMAIN = MoneyGram.PartnerService.DomainModel;
using SERVICE = MoneyGram.PartnerService.Service;

namespace MoneyGram.PartnerService.EntityMapper.ServiceModelExtensions
{
    public static class SecurityHeaderExtensions
    {
        public static DOMAIN.SecurityHeader ToDomain(this SERVICE.SecurityHeader req)
        {
            var domainModel = PsMapper.Map<SERVICE.SecurityHeader, DOMAIN.SecurityHeader>(req)
                .AdditionalOperations();
            return domainModel;
        }

        public static DOMAIN.SecurityHeader AdditionalOperations(this DOMAIN.SecurityHeader dmModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(dmModel);
            return dmModel;
        }

        public static void DefineMappings()
        {
            PsMapper.CreateMap<SERVICE.SecurityHeader, DOMAIN.SecurityHeader>()
                .ForMember(dm => dm.UserID, opt => opt.MapFrom(svs => svs != null ? svs.userID : null));
        }
    }
}
