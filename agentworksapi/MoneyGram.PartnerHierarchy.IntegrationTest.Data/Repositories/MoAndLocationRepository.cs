using System.IO;
using MoneyGram.Common.Json;
using MoneyGram.PartnerHierarchy.IntegrationTest.Data.MoAndLocation;

namespace MoneyGram.PartnerHierarchy.IntegrationTest.Data.Repositories
{
    public class MoAndLocationRepository
    {
        public MoAndLocationsData MoAndLocations { get; set; }

        private readonly string _configurationFilePath = Path.Combine(JsonFileHelper.ExecutingDir(), "Data", "MoAndLocations.json");

        public MoAndLocationRepository()
        {
            var jsonData = JsonFileHelper.GetFileContents(_configurationFilePath);

            MoAndLocations = !string.IsNullOrEmpty(jsonData)
                ? JsonProcessor.DeserializeObject<MoAndLocationsData>(jsonData): new MoAndLocationsData();           
        }
    }
}
