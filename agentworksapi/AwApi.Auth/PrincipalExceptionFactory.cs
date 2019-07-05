using AwApi.ViewModels;
using MoneyGram.Common.Localization;
using System.Collections.Generic;
using System.Net;

namespace AwApi.Auth
{
    public static class PrincipalExceptionFactory
    {
        public static PrincipalException Create(PrincipalExceptionType type, string msg = "", List<string> missingClaims = null, HttpStatusCode StatusCode = HttpStatusCode.Unauthorized)
        {
            var exceptionMsg = !string.IsNullOrWhiteSpace(msg) ? msg : LocalizationKeys.SecurityException;

            var missingClaimsInteralDetails = missingClaims != null ? string.Format("Missing Claims {0}", string.Join(",", missingClaims.ToArray())) : string.Empty;
            return new PrincipalException(exceptionMsg)
            {
                Code = (int) type,
                InternalDetails = missingClaimsInteralDetails,
                StatusCode = StatusCode
            };
        }
    }

    public enum PrincipalExceptionType
    {
        OpenAm = 10,
        PartnerService = 20,
        AgentConnect = 30,
        NonTransactionalDevice = 40
    }
}