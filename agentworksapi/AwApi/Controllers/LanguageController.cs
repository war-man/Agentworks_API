using System;
using System.Collections.Generic;
using System.Web.Http;
using AwApi.Auth;
using AwApi.Business;
using MoneyGram.Common.Extensions;
//using System.Web.Http.Cors;

namespace AwApi.Controllers
{
    // To enable CORS on controller level, use EnableCors attribute
    //[EnableCors( origins: "http://localhost:83,http://localhost:84", headers: "*", methods: "*" )]
    [AwAuthorization]
    [RoutePrefix("api/language")]
    public class LanguageController : ApiController
    {
        private readonly ILanguageBusiness _business;

        public LanguageController(ILanguageBusiness business)
        {
            business.ThrowIfNull(nameof(business));

            _business = business;
        }

        [HttpGet]
        [Route("{applicationId}")]
        public IHttpActionResult GetVersions(string applicationId)
        {
            applicationId.ThrowIfNull(nameof(applicationId));

            var languages = _business.LanguageList(applicationId);
            return Ok(languages);
        }

        [HttpGet]
        [Route("{applicationId}/{langId:length(5)}")]
        public IHttpActionResult Get(string applicationId, string langId)
        {
            applicationId.ThrowIfNullOrEmpty(nameof(applicationId));
            langId.ThrowIfNullOrEmpty(nameof(langId));

            var language = _business.GetLanguage(applicationId, langId);
            return Ok(language);
        }

        [HttpPost]
        [Route("{applicationId}")]
        public IHttpActionResult Get([FromUri] string applicationId, [FromBody] Dictionary<string, DateTime> requestedLanguages)
        {
            applicationId.ThrowIfNull(nameof(applicationId));
            requestedLanguages.ThrowIfNullOrEmpty(nameof(requestedLanguages));

            var languages = _business.GetLanguages(applicationId, requestedLanguages);
            return Ok(languages);
        }
    }
}