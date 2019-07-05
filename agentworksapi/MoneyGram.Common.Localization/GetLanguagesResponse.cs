using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace MoneyGram.Common.Localization
{
    [Serializable]
    public sealed class GetLanguagesResponse
    {
        public Dictionary<string, JObject> Languages { get; set; }
    }
}