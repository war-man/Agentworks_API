using AwApi.Auth;
using AwApi.Business;
using MoneyGram.Common.Extensions;
using System.Web.Http;
using AwApi.ViewModels;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.Common.Localization;

namespace AwApi.Controllers
{
    [AwAuthorization(MgiAwRole.Teller), AwDeviceAuthorization(DeviceSecurityLevel.Transactional)]
    [RoutePrefix("api/bank")]
    public class BankController : ApiController
    {
        private readonly IBankBusiness bankBusiness;

        public BankController(IBankBusiness bankBusiness)
        {
            bankBusiness.ThrowIfNull(nameof(bankBusiness));

            this.bankBusiness = bankBusiness;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="getBankDetailsByLevelRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("by-level")]
        public IHttpActionResult GetBankDetailsByLevel(GetBankDetailsByLevelRequest getBankDetailsByLevelRequest)
        {
            var respVm = bankBusiness.GetBankDetailsByLevel(getBankDetailsByLevelRequest);

            return Ok(respVm);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult GetBankDetails(GetBankDetailsRequest getBankDetailsRequest)
        {
            getBankDetailsRequest.InfoKey = "bankIdentifier_WithLookup";

            var responseVm = bankBusiness.GetBankDetails(getBankDetailsRequest);

            return Ok(responseVm);
        }

        private string StartUriForGetBankDetailsByLevel(GetBankDetailsByLevelRequest req)
        {
            var scheme = $"{Request.RequestUri.Scheme}://";
            var host = Request.RequestUri.Host;
            var port = Request.RequestUri.Port == 80 ? string.Empty : $":{Request.RequestUri.Port}";
            var methodRoute = $"/api/bank/by-level";
            var variableParams = string.Empty;
            var queryStr = string.IsNullOrEmpty(Request.RequestUri.Query) ? string.Empty : Request.RequestUri.Query;

            var uri = $"{scheme}{host}{port}{methodRoute}{variableParams}{queryStr}";

            return uri;
        }

        private string NextUriForGetBankDetailsByLevel(GetBankDetailsByLevelRequest req)
        {
            var scheme = $"{Request.RequestUri.Scheme}://";
            var host = Request.RequestUri.Host;
            var port = Request.RequestUri.Port == 80 ? string.Empty : $":{Request.RequestUri.Port}";
            var methodRoute = $"/api/bank/by-level";
            var queryStr = string.IsNullOrEmpty(Request.RequestUri.Query) ? string.Empty : Request.RequestUri.Query;

            var uri = $"{scheme}{host}{port}{methodRoute}{queryStr}";

            return uri;
        }

        private string NotesForGetBankDetailsByLevel()
        {
            var userLang = ClaimsManager.GetClaimValue(ClaimsNames.Language);
            var notes = LocalizationProvider.GetTranslation(LocalizationProvider.DefaultApplication, userLang, "STRKEY_API_GETBANKDETAILS_NOTES");
            return notes;
        }
    }
}