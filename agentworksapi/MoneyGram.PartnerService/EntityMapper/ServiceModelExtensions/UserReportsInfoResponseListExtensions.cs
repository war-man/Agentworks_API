using System.Linq;
using DOMAIN = MoneyGram.PartnerService.DomainModel;
using SERVICE = MoneyGram.PartnerService.Service;

namespace MoneyGram.PartnerService.EntityMapper.ServiceModelExtensions
{
    public static class UserReportsInfoResponseListExtensions
    {
        public static DOMAIN.UserReportsInfoResponseList ToDomain(this SERVICE.GetUserReportsInfoResponse req)
        {
            var domainModel = PsMapper.Map<SERVICE.GetUserReportsInfoResponse, DOMAIN.UserReportsInfoResponseList>(req)
                .AdditionalOperations();
            return domainModel;
        }

        public static DOMAIN.UserReportsInfoResponseList AdditionalOperations(this DOMAIN.UserReportsInfoResponseList dmModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(dmModel);
            return dmModel;
        }

        public static void DefineMappings()
        {
            PsMapper.CreateMap<SERVICE.GetUserReportsInfoResponse, DOMAIN.UserReportsInfoResponseList>()
                .ForMember(dm => dm.UserReportsInfoList, opt => opt.MapFrom(svs => svs.userReportsInfoRequestList != null ? svs.userReportsInfoRequestList.ToList().ToUserReportsInfoResponseList() : null))
                .ForMember(dm => dm.header, opt => opt.MapFrom(svs => svs.header.ToDomain()));
        }
    }
}
