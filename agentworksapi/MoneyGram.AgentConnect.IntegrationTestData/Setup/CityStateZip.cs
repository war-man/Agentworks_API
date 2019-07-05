namespace MoneyGram.AgentConnect.IntegrationTest.Data.Setup
{
    public static class CityStateZip
    {
        public static CityStateZipInfo Minneapolis => new CityStateZipInfo
        {
            City = "Minneapolis",
            CountrySubdivisionCode = "US-MN",
            PostalCode = "55410"
        };

        public static CityStateZipInfo Chicago => new CityStateZipInfo
        {
            City = "Chicago",
            CountrySubdivisionCode = "US-IL",
            PostalCode = "60606"
        };

        public static CityStateZipInfo NewYork => new CityStateZipInfo
        {
            City = "New York",
            CountrySubdivisionCode = "US-NY",
            PostalCode = "10001"
        };
    }

    public class CityStateZipInfo
    {
        public string City { get; set; }

        public string CountrySubdivisionCode { get; set; }

        public string PostalCode { get; set; }
    }
}