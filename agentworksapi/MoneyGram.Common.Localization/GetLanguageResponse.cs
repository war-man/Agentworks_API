using System;
using Newtonsoft.Json.Linq;

namespace MoneyGram.Common.Localization
{
    [Serializable]
    public sealed class GetLanguageResponse
    {
        public JObject Language { get; set; }
    }
}