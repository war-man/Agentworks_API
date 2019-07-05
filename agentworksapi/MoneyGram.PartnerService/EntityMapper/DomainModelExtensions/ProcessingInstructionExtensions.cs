using DOMAIN = MoneyGram.PartnerService.DomainModel;
using SERVICE = MoneyGram.PartnerService.Service;

namespace MoneyGram.PartnerService.EntityMapper.DomainModelExtensions
{
    public static class ProcessingInstructionExtensions
    {
        public static SERVICE.ProcessingInstruction ToService(this DOMAIN.ProcessingInstruction req)
        {
            var serviceModel = PsMapper.Map<DOMAIN.ProcessingInstruction, SERVICE.ProcessingInstruction>(req)
                .AdditionalOperations();
            return serviceModel;
        }

        public static SERVICE.ProcessingInstruction AdditionalOperations(this SERVICE.ProcessingInstruction svcModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(svcModel);
            return svcModel;
        }

        public static void DefineMappings()
        {
            PsMapper.CreateMap<DOMAIN.ProcessingInstruction, SERVICE.ProcessingInstruction>()
                .ForMember(svs => svs.echoRequestFlagSpecified, opt => opt.MapFrom(dm => dm.EchoRequestFlagFieldSpecified))
                .ForMember(svs => svs.invocationMethodSpecified, opt => opt.MapFrom(dm => dm.InvocationMethodFieldSpecified))
                .ForMember(svs => svs.readOnlyFlagSpecified, opt => opt.MapFrom(dm => dm.ReadOnlyFlagFieldSpecified))
                .ForMember(svs => svs.returnErrorsAsExceptionSpecified, opt => opt.MapFrom(dm => dm.ReturnErrorsAsExceptionFieldSpecified));
        }
    }
}