using System.Collections.Generic;

namespace LangProcessor.FileParsers
{
    public interface ISourceDeserializer
    {
        IEnumerable<KeyValuePair<string, string>> Deserialize(string path);

        IDictionary<string, IEnumerable<KeyValuePair<string, string>>> Deserialize(IEnumerable<string> paths);
    }
}
