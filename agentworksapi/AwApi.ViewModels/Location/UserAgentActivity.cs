using System;

namespace AwApi.ViewModels.Location
{
    [Serializable]
    public class UserAgentActivity
    {
        public string AgentId { get; set; }
        public ActivityType UserActivityType { get; set; }
    }
}