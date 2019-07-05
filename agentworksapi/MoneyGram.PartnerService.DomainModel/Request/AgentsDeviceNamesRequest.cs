using System;
using System.Collections.Generic;

namespace MoneyGram.PartnerService.DomainModel.Request
{
    [Serializable]
    public class AgentsDeviceNamesRequest : BaseServiceMessage 
    {
        public List<AgentPosDevice> AgentPosDeviceList { get; set; }
    }
}
