using DOMAIN = MoneyGram.PartnerService.DomainModel.Request;
using SERVICE = MoneyGram.PartnerService.Service;

namespace MoneyGram.PartnerService.EntityMapper.DomainModelExtensions
{
    public static class getUserIdExistsRequest1Extensions
    {
        public static SERVICE.getUserIdExistsRequest1 ToUserIdExistsRequest1Service(this DOMAIN.UserIdExistsRequest req)
        {
            var serviceModel = PsMapper.Map<DOMAIN.UserIdExistsRequest, SERVICE.getUserIdExistsRequest1>(req)
                .AdditionalOperations();
            return serviceModel;
        }

        public static SERVICE.getUserIdExistsRequest1 AdditionalOperations(this SERVICE.getUserIdExistsRequest1 svcModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(svcModel);
            return svcModel;
        }

        public static void DefineMappings()
        {
            PsMapper.CreateMap<DOMAIN.UserIdExistsRequest, SERVICE.getUserIdExistsRequest1>()
                .ForMember(svs => svs.getUserIdExistsRequest, opt => opt.MapFrom(dm => dm));
        }
    }
}