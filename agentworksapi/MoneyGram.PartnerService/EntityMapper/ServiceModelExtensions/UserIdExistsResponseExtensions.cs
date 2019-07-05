using DOMAIN = MoneyGram.PartnerService.DomainModel.Response;
using SERVICE = MoneyGram.PartnerService.Service;

namespace MoneyGram.PartnerService.EntityMapper.ServiceModelExtensions
{
    public static class UserIdExistsResponseExtensions
    {
        public static DOMAIN.UserIdExistsResponse ToDomain(this SERVICE.GetUserIdExistsResponse req)
        {
            var domainModel = PsMapper.Map<SERVICE.GetUserIdExistsResponse, DOMAIN.UserIdExistsResponse>(req)
                .AdditionalOperations();
            return domainModel;
        }

        public static DOMAIN.UserIdExistsResponse AdditionalOperations(this DOMAIN.UserIdExistsResponse dmModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(dmModel);
            return dmModel;
        }

        public static void DefineMappings()
        {
            PsMapper.CreateMap<SERVICE.GetUserIdExistsResponse, DOMAIN.UserIdExistsResponse>()
                .ForMember(dm => dm.header, opt => opt.MapFrom(svs => svs.header.ToDomain()));
        }
    }
}
