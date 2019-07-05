using System;

namespace MoneyGram.PartnerService.DomainModel.Response
{
    [Serializable]
    public class AgentPasswordResponse : BaseServiceMessage 
    {
        public string AgentPassword { get; set; }
    }
}