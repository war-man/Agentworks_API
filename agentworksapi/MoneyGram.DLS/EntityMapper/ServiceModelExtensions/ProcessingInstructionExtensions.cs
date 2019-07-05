using DOMAIN = MoneyGram.DLS.DomainModel;
using SERVICE = MoneyGram.DLS.Service;

namespace MoneyGram.DLS.EntityMapper.ServiceModelExtensions
{
    public static class ProcessingInstructionExtensions
    {
        public static DOMAIN.ProcessingInstruction ToDomain(this SERVICE.ProcessingInstruction req)
        {
            var domainModel = DLSMapper.Map<SERVICE.ProcessingInstruction, DOMAIN.ProcessingInstruction>(req)
                .AdditionalOperations();
            return domainModel;
        }

        public static DOMAIN.ProcessingInstruction AdditionalOperations(this DOMAIN.ProcessingInstruction dmModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(dmModel);
            return dmModel;
        }

        public static void DefineMappings()
        {
            DLSMapper.CreateMap<SERVICE.ProcessingInstruction, DOMAIN.ProcessingInstruction>();
        }
    }
}