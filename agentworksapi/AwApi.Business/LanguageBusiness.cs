using System;
using System.Collections.Generic;
using AwApi.Integration;
using MoneyGram.Common.Extensions;
using Newtonsoft.Json.Linq;
using AwApi.EntityMapper;
using AwApi.ViewModels;
using MoneyGram.Common.Localization;

namespace AwApi.Business
{
    public class LanguageBusiness : ILanguageBusiness
    {
        protected readonly ILanguageIntegration _languageIntegration;
        public LanguageBusiness(ILanguageIntegration languageIntegration)
        {
            languageIntegration.ThrowIfNull(nameof(languageIntegration));

            _languageIntegration = languageIntegration;
        }

        public ApiResponse<LanguageListResponse, ApiData> LanguageList(string applicationId)
        {
            var serializedLanguageList = _languageIntegration.LanguageList(applicationId);
            var respVm = new LanguageListResponse
            {
                LanguageList = JObject.Parse(serializedLanguageList)
            };

            var apiResp = new ApiResponse<LanguageListResponse, ApiData>
            {
                BusinessMetadata = MapperHelper.SetResponseProperties(null, DataSource.Language),
                ResponseData = respVm
            };

            return apiResp;
        }

        public ApiResponse<GetLanguageResponse, ApiData> GetLanguage(string applicationId, string langId)
        {
            var serializedLanguage = _languageIntegration.GetLanguage(applicationId, langId);

            var respVm = new GetLanguageResponse
            {
                Language = JObject.Parse(serializedLanguage)
            };

            var apiResp = new ApiResponse<GetLanguageResponse, ApiData>
            {
                BusinessMetadata = MapperHelper.SetResponseProperties(null, DataSource.Language),
                ResponseData = respVm
            };

            return apiResp;
        }

        public ApiResponse<GetLanguagesResponse, ApiData> GetLanguages(string applicationId, Dictionary<string, DateTime> requestedLanguages)
        {
            var serializedLanguages = _languageIntegration.GetLanguages(applicationId, requestedLanguages);
            var languages = new Dictionary<string, JObject>();
            foreach (var lang in serializedLanguages)
            {
                var parsedLang = JObject.Parse(lang.Value);
                var parseLangVersion = parsedLang["Version"].Value<DateTime>();

                if (!requestedLanguages.ContainsKey(lang.Key))
                {
                    continue;
                }
                if (requestedLanguages[lang.Key] < parseLangVersion)
                {
                    languages.Add(lang.Key, parsedLang);
                }
            }

            var respVm = new GetLanguagesResponse
            {
                Languages = languages
            };

            var apiResp = new ApiResponse<GetLanguagesResponse, ApiData>
            {
                BusinessMetadata = MapperHelper.SetResponseProperties(null, DataSource.Language),
                ResponseData = respVm
            };

            return apiResp;
        }
    }
}