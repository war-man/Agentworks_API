using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MoneyGram.Common.Json;

namespace MoneyGram.Common.Localization
{
    public static class LocalizationProvider
    {
        public static readonly string DefaultApplication = "AgentWorks";

        private static readonly string DefaultLocale = "en-US";
        private static readonly string LanguageFilesPath = Path.Combine(JsonFileHelper.ExecutingDir(), "languages");

        public static string LanguageList(string applicationId)
        {
            var filePath = Path.Combine(LanguageFilesPath, applicationId, "languages.json");
            var languages = JsonFileHelper.GetFileContents(filePath) ?? string.Empty;
            return languages;
        }

        public static string GetLanguage(string applicationId, string langId)
        {
            var filePath = Path.Combine(LanguageFilesPath, applicationId, langId + ".json");
            var language = JsonFileHelper.GetFileContents(filePath) ?? string.Empty;
            return language;
        }

        public static bool LocaleExists(string applicationId, string langId)
        {
            var filePath = Path.Combine(LanguageFilesPath, applicationId, langId + ".json");
            return File.Exists(filePath);
        }

        public static string GetTranslation(string applicationId, string langId, string keyName)
        {
            var language = GetLanguage(applicationId, langId);

            return JObject.Parse(language)["Strings"][keyName]?.ToString();
        }

        public static string GetTranslation(string applicationId, IEnumerable<string> requestLanguages, string key, string defaultValue = null)
        {
            var locale = requestLanguages?.FirstOrDefault(lang => LocaleExists(applicationId, lang)) ?? DefaultLocale;
            return GetTranslation(applicationId, locale, key) ?? defaultValue;
        }

        public static IDictionary<string, string> GetTranslations(string applicationId, string langId, IEnumerable<string> keyNames)
        {
            var language = GetLanguage(applicationId, langId);

            var parsedLanguage = JObject.Parse(language)["Strings"];

            return keyNames.ToDictionary(keyName => keyName, keyName => parsedLanguage[keyName]?.ToString());
        }

        public static Dictionary<string, string> GetLanguages(string applicationId, Dictionary<string, DateTime> requestedLanguages)
        {
            var allLangFiles = Directory.GetFiles(Path.Combine(LanguageFilesPath, applicationId), "??-??.json", SearchOption.AllDirectories);
            var langFilesToReturn = requestedLanguages == null ? new List<string>(allLangFiles) : new List<string>();
            if (requestedLanguages != null)
            {
                foreach (var langFile in allLangFiles)
                {
                    var langCode = Path.GetFileNameWithoutExtension(langFile);

                    if (requestedLanguages.ContainsKey(langCode))
                    {
                        var versionNewer = requestedLanguages[langCode] == null
                            ? true
                            : File.GetLastWriteTime(langFile) > requestedLanguages[langCode];
                        if (versionNewer)
                        {
                            langFilesToReturn.Add(langFile);
                        }
                    }
                }
            }

            var languages = new Dictionary<string, string>();
            foreach (var langFileName in langFilesToReturn)
            {
                var filePath = Path.Combine(LanguageFilesPath, applicationId, langFileName);
                var fileContents = JsonFileHelper.GetFileContents(filePath);
                if (fileContents != null)
                {
                    languages.Add(Path.GetFileNameWithoutExtension(Path.Combine(LanguageFilesPath, applicationId, langFileName)),
                        fileContents);
                }
            }
            return languages;
        }
    }
}