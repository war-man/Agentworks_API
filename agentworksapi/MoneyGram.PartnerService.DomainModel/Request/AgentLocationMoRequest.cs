using System;

namespace MoneyGram.PartnerService.DomainModel
{
    [Serializable]
    public class AgentLocationMoRequest: BaseServiceMessage
    {
        public decimal AgentId { get; set; }
    }
}
