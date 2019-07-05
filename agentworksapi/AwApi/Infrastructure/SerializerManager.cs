using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AwApi.Infrastructure
{
    public static class SerializerManager
    {
        public static void ApplyCustomSettings(MediaTypeFormatterCollection formatters)
        {
            // This API will support Json responses only, remove all other formatters
            var formattersToRemove = new List<MediaTypeFormatter>();
            formattersToRemove.AddRange(formatters.Where(f => f.GetType() != typeof(JsonMediaTypeFormatter)));
            foreach (var formatterToRemove in formattersToRemove)
            {
                formatters.Remove(formatterToRemove);
            }

            // Configure Json formatter
            ConfigureJsonSerialization(formatters.JsonFormatter);

            // Adding Formatter for file download requests
            formatters.Add(new OctetStreamMediaFormatter());
        }

        private static void ConfigureJsonSerialization(JsonMediaTypeFormatter jsonMediaTypeFormatter)
        {
            // Format Json without indentation and extra whitespace
            jsonMediaTypeFormatter.SerializerSettings.Formatting = Formatting.None;
            // Convert C# Models' Pascal case names to JavaScript View Model Camel case names
            // Customer.FirstName = "Dmitri" will serialize to {"customer": {"firstName": "Dmitri"}}
            jsonMediaTypeFormatter.SerializerSettings.ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            // Ignore circular references in data when serializing to JSON
            jsonMediaTypeFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            // Explicitly exclude null properties from serialization
            jsonMediaTypeFormatter.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
        }
    }
}