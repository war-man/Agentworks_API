using DOMAIN = MoneyGram.PartnerService.DomainModel.Request;
using SERVICE = MoneyGram.PartnerService.Service;

namespace MoneyGram.PartnerService.EntityMapper.DomainModelExtensions
{
    public static class GetUserIdExistsRequestExtensions
    {
        public static SERVICE.GetUserIdExistsRequest ToService(this DOMAIN.UserIdExistsRequest req)
        {
            var serviceModel = PsMapper.Map<DOMAIN.UserIdExistsRequest, SERVICE.GetUserIdExistsRequest>(req)
                .AdditionalOperations();
            return serviceModel;
        }

        public static SERVICE.GetUserIdExistsRequest AdditionalOperations(this SERVICE.GetUserIdExistsRequest svcModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(svcModel);
            return svcModel;
        }

        public static void DefineMappings()
        {
            PsMapper.CreateMap<DOMAIN.UserIdExistsRequest, SERVICE.GetUserIdExistsRequest>()
                .ForMember(svs => svs.header, opt => opt.MapFrom(dm => dm.header.ToService()));
        }
    }
}