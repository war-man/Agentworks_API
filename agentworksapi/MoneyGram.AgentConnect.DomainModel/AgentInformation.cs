using MoneyGram.Common.Attributes;
using System;
using System.Collections.Generic;

namespace MoneyGram.AgentConnect.DomainModel
{
    public enum AgentService
    {
        [EnumMetadata("walmart_findlocation_receive_service", 0)]
        ReceiveMoney,
        [EnumMetadata("walmart_findlocation_send_service", 1)]
        SendMoney
    }

    [Serializable]
    public class AgentInformation
    {
        public AgentInformation()
        {
            HoursOfOperation = new List<string>();
            Services = new List<AgentService>();
            UniqueId = Guid.NewGuid();
        }

        public Guid UniqueId { get; set; }
        public string AgentName { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string FullAddress => $"{Address}{(string.IsNullOrWhiteSpace(City) ? "" : ", " + City)}{(string.IsNullOrWhiteSpace(State) ? "" : ", " + State)}";
        public string AgentPhone { get; set; }
        public List<string> HoursOfOperation { get; private set; }
        public List<AgentService> Services { get; private set; }
    }
}