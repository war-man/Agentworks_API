using System;

namespace MoneyGram.AgentConnect.DomainModel
{
    [Serializable]
    public class Agent
    {
        public string AgentId { get; set; }
        public string AgentPassword { get; set; }
        public string AgentSequence { get; set; }
        public int? UnitProfileId { get; set; }
        public string OperatorName { get; set; }
        public string Language { get; set; }
        public int HierarchyLevel { get; set; }
        public string Otp { get; set; }
        public bool IsInTrainingMode { get; set; }
    }
}