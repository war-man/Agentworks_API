using System.Configuration;
using System.IO;
using MoneyGram.Common.Json;

namespace MoneyGram.AgentConnect.IntegrationTest.Data
{
    public static class TestConfig
    {
        private static IntegrationTestSettingsModel settings;

        private static string jsonFilePath => Path.Combine(JsonFileHelper.ExecutingDir(), ConfigurationManager.AppSettings["IntegrationTestSettingsFileName"]);

        public static IntegrationTestSettingsModel TestSettings
        {
            get
            {
                if(File.Exists(jsonFilePath))
                {
                    var jsonData = File.ReadAllText(jsonFilePath);
                    settings = JsonProcessor.DeserializeObject<IntegrationTestSettingsModel>(jsonData);
                }

                return settings;
            }
            set
            {
                var jsonData = JsonProcessor.SerializeObject(value);
                File.WriteAllText(jsonFilePath, jsonData);
            }
        }
    }
}