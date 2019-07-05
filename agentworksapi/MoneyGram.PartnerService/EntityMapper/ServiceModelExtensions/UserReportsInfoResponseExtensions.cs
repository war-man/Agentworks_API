using DOMAIN = MoneyGram.PartnerService.DomainModel;
using SERVICE = MoneyGram.PartnerService.Service;

namespace MoneyGram.PartnerService.EntityMapper.ServiceModelExtensions
{
    public static class UserReportsInfoResponseExtensions
    {
        public static DOMAIN.UserReportsInfoResponse ToDomain(this SERVICE.UserReportsInfo req)
        {
            var domainModel = PsMapper.Map<SERVICE.UserReportsInfo, DOMAIN.UserReportsInfoResponse>(req)
                .AdditionalOperations();
            return domainModel;
        }

        public static DOMAIN.UserReportsInfoResponse AdditionalOperations(this DOMAIN.UserReportsInfoResponse dmModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(dmModel);
            return dmModel;
        }

        public static void DefineMappings()
        {
            PsMapper.CreateMap<SERVICE.UserReportsInfo, DOMAIN.UserReportsInfoResponse>()
                .ForMember(dm => dm.PosNumberFieldSpecified, opt => opt.MapFrom(svs => svs.posNumberSpecified));
        }
    }
}
