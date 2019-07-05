using MoneyGram.Common.Diagnostics;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangProcessor.FileParsers
{
    public class TabDelimitedSourceDeserializer : ISourceDeserializer
    {
        private static ILog log = LogManager.GetLogger(typeof(TabDelimitedSourceDeserializer));

        public IEnumerable<KeyValuePair<string, string>> Deserialize(string path)
        {
            var localizedStrings = new List<KeyValuePair<string, string>>();
            Encoding encoding;
            using(var streamReader = new StreamReader(path))
            {
                streamReader.Peek();
                encoding = streamReader.CurrentEncoding;
                string line = null;
                while((line = streamReader.ReadLine()) != null)
                {
                    var parsedLine = line.Split('\t');

                    if(parsedLine != null && parsedLine.Length > 1)
                    {
                        var localizedString = string.Join("\t", parsedLine.Skip(1));
                        localizedStrings.Add(new KeyValuePair<string, string>(parsedLine[0], localizedString));
                    }
                }
            }

            var sortedStrings = localizedStrings.OrderBy(x => x.Key).ToList();
        
            if(!sortedStrings.SequenceEqual(localizedStrings))
            {
                this.WriteToFile(sortedStrings, path, encoding);
            }

            return sortedStrings;
        }

        public IDictionary<string, IEnumerable<KeyValuePair<string, string>>> Deserialize(IEnumerable<string> paths)
        {
            log.Info("About to deserialize: " + string.Join(", ", paths));

            var localizedStrings = new ConcurrentDictionary<string, IEnumerable<KeyValuePair<string, string>>>();

            Parallel.ForEach(paths, (path) => 
            {             
                localizedStrings.TryAdd(Path.GetFileNameWithoutExtension(path), this.Deserialize(path));
            });

            log.Info(paths.Count() + " languages successfully deserialized");

            return localizedStrings;
        }

        private void WriteToFile(IEnumerable<KeyValuePair<string, string>> rawTranslations, string path, Encoding encoding)
        {
            using (var streamWriter = new StreamWriter(path, false, encoding))
            {
                var lines = rawTranslations.Select(x => x.Key + '\t' + x.Value);
                var fileData = string.Join("\n", lines);
                streamWriter.Write(fileData);
            }
        }
    }
}
