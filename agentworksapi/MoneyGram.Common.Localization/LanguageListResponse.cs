using System;
using Newtonsoft.Json.Linq;

namespace MoneyGram.Common.Localization
{
    [Serializable]
    public sealed class LanguageListResponse
    {
        public JObject LanguageList { get; set; }
    }
}