using System;

namespace MoneyGram.PartnerService.DomainModel
{
    [Serializable]
    public class AgentPosDevice
    {
        public decimal AgentId;

        public decimal PosNumber;

        public bool PosNumberFieldSpecified;

        public string Name;
    }
}
