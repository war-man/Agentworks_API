namespace LangProcessor.Models
{
    public class LanguageMetadata : VersionedLanguage
    {
        public string LanguageCode { get; set; }
        public string Language { get; set; }
        public string LocalizedLanguageName { get; set; }
    }
}
