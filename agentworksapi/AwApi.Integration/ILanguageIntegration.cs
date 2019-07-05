using System;
using System.Collections.Generic;

namespace AwApi.Integration
{
    public interface ILanguageIntegration
    {
        string LanguageList(string applicationId);
        string GetLanguage(string applicationId, string langId);
        Dictionary<string, string> GetLanguages(string applicationId, Dictionary<string, DateTime> requestedLanguages);
    }
}
