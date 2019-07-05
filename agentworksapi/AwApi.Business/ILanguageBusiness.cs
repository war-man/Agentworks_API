using System;
using System.Collections.Generic;
using AwApi.ViewModels;
using MoneyGram.Common.Localization;

namespace AwApi.Business
{
    public interface ILanguageBusiness
    {
        ApiResponse<LanguageListResponse, ApiData> LanguageList(string applicationId);
        ApiResponse<GetLanguageResponse, ApiData> GetLanguage(string applicationId, string langId);
        ApiResponse<GetLanguagesResponse, ApiData> GetLanguages(string applicationId, Dictionary<string, DateTime> requestedLanguages);
    }
}