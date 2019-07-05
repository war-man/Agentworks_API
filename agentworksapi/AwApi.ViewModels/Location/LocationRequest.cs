using System;

namespace AwApi.ViewModels.Location
{
    [Serializable]
    public class LocationRequest
    {
        public string StoreNameNumberAgentId { get; set; }
        public string SubLevelNameId { get; set; }
        public string StateProvince { get; set; }
        public string AgentName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string StateCode { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string Phone { get; set; }
        public int HierarchyLevel { get; set; }
        public decimal MainOfficeId { get; set; }
    }
}