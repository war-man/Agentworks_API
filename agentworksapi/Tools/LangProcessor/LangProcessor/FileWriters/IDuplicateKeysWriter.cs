using System.Collections.Generic;

namespace LangProcessor.FileWriters
{
    public interface IDuplicateKeysWriter
    {
        bool CreateDuplicateKeysReport(string destinationDirectory, IDictionary<string, IEnumerable<KeyValuePair<string, string>>> rawTranslations);
    }
}