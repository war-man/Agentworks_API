using System.Collections.Generic;

namespace MoneyGram.PartnerHierarchy.IntegrationTest.Data.MoAndLocation
{
    public class MoAndLocationsData
    {
        public List<LocationDetail> LocationDetails { get;set; }

        public MoAndLocationsData()
        {
            LocationDetails = new List<LocationDetail>();
        }

    }
}