using System;

namespace MoneyGram.PartnerService.DomainModel
{
    [Serializable]
    public class UserReportsInfoRequest : BaseServiceMessage 
    {
        public decimal MainOfficeId { get; set; }
        public bool MainOfficeIdSpecified { get; set; }
        public System.DateTime FromDate { get; set; }
        public bool FromDateSpecified { get; set; }
        public System.DateTime ToDate { get; set; }
        public bool ToDateSpecified { get; set; }
        public decimal[] Locations { get; set; }
    }
}
