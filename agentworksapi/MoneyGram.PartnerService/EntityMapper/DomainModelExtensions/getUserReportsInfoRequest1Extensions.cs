using DOMAIN = MoneyGram.PartnerService.DomainModel;
using SERVICE = MoneyGram.PartnerService.Service;

namespace MoneyGram.PartnerService.EntityMapper.DomainModelExtensions
{
    public static class getUserReportsInfoRequest1Extensions
    {
        public static SERVICE.getUserReportsInfoRequest1 ToUserReportsInfoRequest1Service(this DOMAIN.UserReportsInfoRequest req)
        {
            var serviceModel = PsMapper.Map<DOMAIN.UserReportsInfoRequest, SERVICE.getUserReportsInfoRequest1>(req)
                .AdditionalOperations();
            return serviceModel;
        }

        public static SERVICE.getUserReportsInfoRequest1 AdditionalOperations(this SERVICE.getUserReportsInfoRequest1 svcModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(svcModel);
            return svcModel;
        }

        public static void DefineMappings()
        {
            PsMapper.CreateMap<DOMAIN.UserReportsInfoRequest, SERVICE.getUserReportsInfoRequest1>()
                .ForMember(svs => svs.getUserReportsInfoRequest, opt => opt.MapFrom(dm => dm));
        }
    }
}