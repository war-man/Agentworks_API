using System;

namespace MoneyGram.PartnerService.DomainModel
{
    [Serializable]
    public class LocationsForMoRequest :  BaseServiceMessage 
    {
        public string StoreNameNumberAgentId {get;set;}
        public string SubLevelNameId {get;set;}
		public string City {get;set;}
		public string StateProvince {get;set;}
		public string Country {get;set;}
        public string ZipCode { get; set; }
        public decimal MainOfficeId { get; set; }
    }
}
