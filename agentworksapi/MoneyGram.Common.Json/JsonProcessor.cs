using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace MoneyGram.Common.Json
{
    public class JsonProcessor
    {
        public static string SerializeObject<T>(T obj, bool camelCase = true, bool indented = false, bool typeNameHandlingAuto = false)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            if (camelCase == true)
            {
                jsonSerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            }
            if (typeNameHandlingAuto)
            {
                jsonSerializerSettings.TypeNameHandling = TypeNameHandling.Auto;
            }
            if (indented)
            {
                jsonSerializerSettings.Formatting = Formatting.Indented;
            }
            return JsonConvert.SerializeObject(obj, jsonSerializerSettings);
        }

        public static T DeserializeObject<T>(string serializedObj, bool typeNameHandlingAuto = false)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            if (typeNameHandlingAuto)
            {
                jsonSerializerSettings.TypeNameHandling = TypeNameHandling.Auto;
            }
            return JsonConvert.DeserializeObject<T>(serializedObj, jsonSerializerSettings);
        }

        public static List<string> ToStringList(KeyValuePair<string, object> jsonArrayObj)
        {
            return ((JArray) jsonArrayObj.Value).Select(x => x.Value<string>()).ToList();
        }
    }
}