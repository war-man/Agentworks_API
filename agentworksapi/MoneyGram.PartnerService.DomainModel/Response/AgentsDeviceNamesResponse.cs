using System;
using System.Collections.Generic;

namespace MoneyGram.PartnerService.DomainModel.Response
{
    [Serializable]
    public class AgentsDeviceNamesResponse : BaseServiceMessage 
    {
        public List<AgentPosDevice> AgentPosDeviceListResult { get; set; }
    }
}
