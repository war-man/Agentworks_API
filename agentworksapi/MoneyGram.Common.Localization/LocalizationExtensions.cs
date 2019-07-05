using System.Collections.Generic;
using System.Linq;

namespace MoneyGram.Common.Localization
{
    public static class LocalizationExtensions
    {
        public static readonly string[] LocalizedStringPrefixes = {"STRKEY"};


        public static IEnumerable<string> RequestLanguages(this string acceptLanguageHeaders)
        {
            var requestLanguages =
                acceptLanguageHeaders?.Split(',').Select(x => x.Trim()).ToList();
            return requestLanguages;
        }

        public static bool IsLocalizedStringKey(this string stringKey)
        {
            return LocalizedStringPrefixes.Any(prefix => stringKey.ToLower().StartsWith(prefix.ToLower()));
        }
    }
}