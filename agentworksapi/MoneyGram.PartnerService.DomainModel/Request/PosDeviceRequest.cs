using System;

namespace MoneyGram.PartnerService.DomainModel.Request
{
    [Serializable]
    public class PosDeviceRequest : BaseServiceMessage
    {
        public decimal AgentId { get; set; }

        public int PoeCode { get; set; }
    }
}
