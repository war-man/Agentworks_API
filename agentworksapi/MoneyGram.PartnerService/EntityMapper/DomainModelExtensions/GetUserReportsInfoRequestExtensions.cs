using DOMAIN = MoneyGram.PartnerService.DomainModel;
using SERVICE = MoneyGram.PartnerService.Service;

namespace MoneyGram.PartnerService.EntityMapper.DomainModelExtensions
{
    public static class GetUserReportsInfoRequestExtensions
    {
        public static SERVICE.GetUserReportsInfoRequest ToService(this DOMAIN.UserReportsInfoRequest req)
        {
            var serviceModel = PsMapper.Map<DOMAIN.UserReportsInfoRequest, SERVICE.GetUserReportsInfoRequest>(req)
                .AdditionalOperations();
            return serviceModel;
        }

        public static SERVICE.GetUserReportsInfoRequest AdditionalOperations(this SERVICE.GetUserReportsInfoRequest svcModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(svcModel);
            return svcModel;
        }

        public static void DefineMappings()
        {
            PsMapper.CreateMap<DOMAIN.UserReportsInfoRequest, SERVICE.GetUserReportsInfoRequest>()
                .ForMember(svs => svs.header, opt => opt.MapFrom(dm => dm.header.ToService()));
        }
    }
}