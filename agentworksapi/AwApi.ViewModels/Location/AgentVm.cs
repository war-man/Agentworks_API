namespace AwApi.ViewModels.Location
{
    public class AgentVm
    {      
        public string id { get; set; }
        public string name { get; set; }
        public string addressLine1 { get; set; }
        public string addressLine2 { get; set; }
        public string addressLine3 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string stateCode { get; set; }
        public string country { get; set; }
        public string postalCode { get; set; }
        public string phone { get; set; }
        public int hierarchyLevel { get; set; }
        //removed as part of code review comment.
        //public string legacyAgentNum { get; set; }
        public string timeZoneName { get; set; }
        public string storeNumber { get; set; }
        public string activityType { get; set; }
    }
}
