
namespace MoneyGram.PartnerService.DomainModel.Request
{
    public class AgentPasswordRequest : BaseServiceMessage 
    {
        public decimal AgentId { get; set; }

        public bool AgentIdSpecified { get; set; }

        public decimal PosNumber { get; set; }

        public bool PosNumberSpecified { get; set; }
    }
}
