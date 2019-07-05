using System;
using System.Collections.Generic;
using MoneyGram.Common.Localization;

namespace AwApi.Integration
{
    public class LanguageIntegration : ILanguageIntegration
    {
        public string LanguageList(string applicationId)
        {
            return LocalizationProvider.LanguageList(applicationId);
        }

        public string GetLanguage(string applicationId, string langId)
        {
            return LocalizationProvider.GetLanguage(applicationId, langId);
        }

        public Dictionary<string, string> GetLanguages(string applicationId, Dictionary<string, DateTime> requestedLanguages)
        {
            return LocalizationProvider.GetLanguages(applicationId, requestedLanguages);
        }
    }
}