using System.Collections.Generic;

namespace LangProcessor.Models
{
    public class LanguageInfo : LanguageMetadata
    {
        public SortedDictionary<string, string> Strings { get; set; }
    }
}
