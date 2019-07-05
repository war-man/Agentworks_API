using System;
using System.Collections.Generic;

namespace AwApi.ViewModels.Location
{
    [Serializable]
    public class LocationResVm: AgentsRespVm
    {
        //public AgentsRespVm Locations { get; set; }

        public LocationResVm()
        {
            this.Agents = new List<AgentVm>();
        }
    }
}
