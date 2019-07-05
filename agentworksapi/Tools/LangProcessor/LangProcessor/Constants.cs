using System.Text.RegularExpressions;

namespace LangProcessor
{
    public static class Constants
    {
        public static string DefaultSourceDirectory = "source";

        public static string DefaultDestinationDirectory = "dist";

        public static string BaseLanguage = "en-US";

        public static string JsonFileExtension = ".json";

        public static string TsvFileExtension = ".tsv";

        public static string XmlFileExtension = ".xml";

        public static string LangSourceMetadata = "LangSourceMetadata";

        public static string Languages = "languages";

        public static string MissingKeys = "missingKeys";

        public static string DuplicateKeys = "duplicateKeys";

        public static string[] DestMetadataFiles = new[] { Languages, MissingKeys, DuplicateKeys };

        public static Regex SourceFileRegex = new Regex(@"([a-zA-Z-]{5})\.source");
    }
}
