using System;
using System.Collections.Generic;
using System.Linq;

namespace AcUpgradeTool
{
    public static class Extensions
    {
        public static string Format(this string toFormat, Dictionary<string, string> replacements)
        {
            foreach (var replacement in replacements)
            {
                toFormat = toFormat.Replace("{" + replacement.Key + "}", replacement.Value);
            }

            return toFormat;
        }

        public static string Capitalize(this string toCapitalize)
        {
            return toCapitalize.First().ToString().ToUpper() + toCapitalize.Substring(1);
        }

        public static string ToCamelCase(this string toCamel)
        {
            return toCamel.First().ToString().ToLower() + toCamel.Substring(1);
        }

        public static bool IsRequest(this Type type)
        {
            return type.Name.ToLower().Contains("request");
        }

        public static bool IsResponse(this Type type)
        {
            var responseTypes = new List<string>
            {
                "response",
                "payload"
            };

            return responseTypes.Any(x => type.Name.ToLower().Contains(x));
        }
    }
}