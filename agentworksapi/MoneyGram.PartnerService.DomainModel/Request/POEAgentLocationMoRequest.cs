using System;

namespace MoneyGram.PartnerService.DomainModel
{
    [Serializable]
    public class POEAgentLocationMoRequest : BaseServiceMessage
    {
        public decimal AgentId { get; set; }
        public decimal POECode { get; set; }
    }
}
