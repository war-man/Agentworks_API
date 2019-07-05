using DOMAIN = MoneyGram.PartnerService.DomainModel;
using SERVICE = MoneyGram.PartnerService.Service;

namespace MoneyGram.PartnerService.EntityMapper.DomainModelExtensions
{
    public static class AgentPosDeviceExtensions
    {
        public static SERVICE.AgentPosDevice ToService(this DOMAIN.AgentPosDevice req)
        {
            var serviceModel = PsMapper.Map<DOMAIN.AgentPosDevice, SERVICE.AgentPosDevice>(req)
                .AdditionalOperations();
            return serviceModel;
        }

        public static SERVICE.AgentPosDevice AdditionalOperations(this SERVICE.AgentPosDevice svcModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(svcModel);
            return svcModel;
        }

        public static void DefineMappings()
        {
            PsMapper.CreateMap<DOMAIN.AgentPosDevice, SERVICE.AgentPosDevice>()
                .ForMember(svs => svs.posNumberSpecified, opt => opt.MapFrom(dm => dm.PosNumberFieldSpecified));
        }
    }
}