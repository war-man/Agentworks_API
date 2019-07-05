using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using AwApi.ViewModels;
using AwApi.ViewModels.Location;
using DOMAIN = MoneyGram.AgentConnect.DomainModel;
using AwApi.Integration.Models;

namespace AwApi.Integration
{
    public static class AuthIntegration
    {

        public static string GetClaim(string claim)
        {
            return GetClaimValue(claim);
        }

        public static bool HasClaims()
        {
            try
            {
                var principal = (GenericPrincipal)Thread.CurrentPrincipal;
                return principal?.Claims != null && principal.Claims.Any();
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static string GetToken()
        {
            return GetClaim(ClaimsNames.Token);
        }

        public static DOMAIN.Agent GetAgent()
        {
            var claims = Claims;
            var acLanguage = GetClaimValue(ClaimsNames.Language) ?? string.Empty;

            var trainingModeEnabled = Thread.CurrentPrincipal.IsInRole(MgiAwRole.TellerTraining);

            return new DOMAIN.Agent
            {
                AgentId = GetClaimValue(ClaimsNames.MgiDeviceAgentLocationId),
                AgentPassword = GetClaimValue(ClaimsNames.AgentPassword),
                AgentSequence = GetClaimValue(ClaimsNames.MgiDevicePosNumber),
                OperatorName = GetClaimValue(ClaimsNames.Sub),
                IsInTrainingMode = trainingModeEnabled,
                Language = acLanguage,
                Otp = GetClaimValue(ClaimsNames.Otp)
            };
        }

        private static string ConvertTo5CharacterReceiptLanguage(string agentProfileReceiptLanguage)
        {
            if (string.IsNullOrEmpty(agentProfileReceiptLanguage) || agentProfileReceiptLanguage.Length != 3)
            {
                return agentProfileReceiptLanguage;
            }
            switch (agentProfileReceiptLanguage.ToLower())
            {
                case "eng":
                    return "en-US";
                case "fra":
                    return "fr-FR";
                case "spa":
                    return "es-MX";
                case "deu":
                    return "de-DE";
                case "zho":
                    return "zh-CN";
                case "srp":
                    return "sr-RS";
                case "mkd":
                    return "mk-MK";
                case "ita":
                    return "it-IT";
                case "sqi":
                    return "sq-AL";
                case "ron":
                    return "ro-RO";
                case "nid":
                    return "nl-NL";
                case "swe":
                    return "sv-SE";
                case "dan":
                    return "da-DK";
                case "nor":
                    return "no-NO";
                default:
                    return agentProfileReceiptLanguage;
            }
        }
        public static Operator GetOperator()
        {
            var claims = Claims;
            var trainingModeEnabled = Thread.CurrentPrincipal.IsInRole(MgiAwRole.TellerTraining);

            return new Operator
            {
                FamilyName = GetClaimValue(ClaimsNames.FamilyName),
                GivenName = GetClaimValue(ClaimsNames.GivenName),
                UserName = GetClaimValue(ClaimsNames.Sub),
                Language = GetClaimValue(ClaimsNames.Language),
                IsInTrainingMode = trainingModeEnabled,
                LastLogin = GetLastLoginTimeStamp(),
                Roles = Roles.Select(role => role.Value).ToList(),
                TransactionalLimits = GetTransactionalLimits()
            };
        }

        public static DateTime? GetLastLoginTimeStamp()
        {
            var mgiUserLastLoginTs = GetClaim(ClaimsNames.MgiUserLastLoginTs);

            if (string.IsNullOrWhiteSpace(mgiUserLastLoginTs))
            {
                return null;
            }

            var lastLoginFormat = "yyyyMMddHHmmss.fff";

            var lastLogIn = DateTime.ParseExact(mgiUserLastLoginTs, lastLoginFormat, CultureInfo.InvariantCulture);

            return lastLogIn;
        }

        public static Device GetDevice()
        {
            var deviceType = GetClaim(ClaimsNames.DeviceType);
            return new Device
            {
                DeviceSecurityLevel = GetClaim(ClaimsNames.DSL),
                PlatformVersionEOLDate = GetClaim(ClaimsNames.PlatformVersionEOLDate),
                HostVersionEOLDate = GetClaim(ClaimsNames.HostVersionEOLDate)
            };
        }

        public static string GetDeviceId()
        {
            return GetClaim(ClaimsNames.MgiDeviceId);
        }

        public static string GetAgentName()
        {
            return GetClaim(ClaimsNames.AgentName);
        }

        public static string GetMainOfficeId()
        {
            return GetClaim(ClaimsNames.MgiMainOfficeId);
        }

        public static string GetStoreName()
        {
            return GetClaim(ClaimsNames.StoreName);
        }

        public static string GetAgentPrimaryReceiptLanguage()
        {
            var threeCharLanguage = GetClaim(ClaimsNames.PrimaryReceiptLanguage);
            return ConvertTo5CharacterReceiptLanguage(threeCharLanguage);
        }

        public static string GetAgentSecondaryReceiptLanguage()
        {
            var threeCharLanguage = GetClaim(ClaimsNames.SecondaryReceiptLanguage);
            return ConvertTo5CharacterReceiptLanguage(threeCharLanguage);
        }

        public static TransactionalLimits GetTransactionalLimits()
        {
            var claims = Claims;
            return new TransactionalLimits
            {
                MaxBillPayAmount = GetTransactionalLimit(claims, ClaimsNames.MgiAppAwMaxBillPayAmount),
                MaxAmendAmount = GetTransactionalLimit(claims, ClaimsNames.MgiAppAwMaxAmendAmount),
                MaxReceiveAmount = GetTransactionalLimit(claims, ClaimsNames.MgiAppAwMaxReceiveAmount),
                MaxSendAmount = GetTransactionalLimit(claims, ClaimsNames.MgiAppAwMaxSendAmount),
                MaxSendCancellationAmount = GetTransactionalLimit(claims, ClaimsNames.MgiAppAwMaxSendCancellationAmount),
                MaxReceiveCancellationAmount =
                    GetTransactionalLimit(claims, ClaimsNames.MgiAppAwMaxReceiveCancellationAmount),
                MaxMoneyOrderAmount = GetTransactionalLimit(claims, ClaimsNames.MgiAppAwMaxMoneyOrderAmount),
                MaxVendorPaymentAmount = GetTransactionalLimit(claims, ClaimsNames.MgiAppAwMaxVendorPaymentAmount)
            };
        }

        private static decimal? GetTransactionalLimit(List<Claim> claims, string claimName)
        {
            var transactionalLimit = GetClaimValue(claimName);
            // If the claim is an empty string, that means unlimited transaction amounts, set to null
            return string.IsNullOrWhiteSpace(transactionalLimit) ? null : (decimal?)decimal.Parse(transactionalLimit);
        }

        public static IEnumerable<string> GetAgentLocations()
        {
            return GetClaimValues(ClaimsNames.MgiAgentLocationId);
        }

        public static User GetUser()
        {
            var agentLocations = GetAgentLocations().ToList();

            var agentList = agentLocations.Select(agentLocationId => new UserAgentActivity
            {
                AgentId = agentLocationId.ToLower() == "all" ? null : agentLocationId,
                UserActivityType = ActivityType.TRN
            }).ToList();

            return new User
            {
                MainOfficeAgentId = GetMainOfficeId(),
                AllLocationsAllowed = agentLocations.First()?.ToLower() == "all",
                UserAgentList = agentList,
                Status = UserStatus.External
            };
        }

        public static ProductAuthorizations GetProductAuthorizations()
        {
            return new ProductAuthorizations
            {
                CanSend = GetBooleanClaimValue(ClaimsNames.CanSend),
                CanSendCompletion = GetBooleanClaimValue(ClaimsNames.CanSendCompletion),
                CanAmend = GetBooleanClaimValue(ClaimsNames.CanAmend),
                CanCancel = GetBooleanClaimValue(ClaimsNames.CanCancel),
                CanSendReversal = GetBooleanClaimValue(ClaimsNames.CanSendReversal),
                CanFeeRefund = GetBooleanClaimValue(ClaimsNames.CanFeeRefund),
                CanReceive = GetBooleanClaimValue(ClaimsNames.CanReceive),
                CanReceiveCompletion = GetBooleanClaimValue(ClaimsNames.CanReceiveCompletion),
                CanReceiveReversal = GetBooleanClaimValue(ClaimsNames.CanReceiveReversal),
                CanBillPay = GetBooleanClaimValue(ClaimsNames.CanBillPay),
                CanBillPayCompletion = GetBooleanClaimValue(ClaimsNames.CanBillPayCompletion),
                CanLoadPrepaidCard = GetBooleanClaimValue(ClaimsNames.CanLoadPrepaidCard),
                CanMoneyOrder = GetBooleanClaimValue(ClaimsNames.CanMoneyOrder),
                CanVendorPay = GetBooleanClaimValue(ClaimsNames.CanVendorPay),
                CanExpressPayment = GetBooleanClaimValue(ClaimsNames.CanExpressPayment),
                CanUtilityBillPay = GetBooleanClaimValue(ClaimsNames.CanUtilityBillPay)
            };
        }

        public static string GetDeviceAgentLocation()
        {
            return GetClaim(ClaimsNames.MgiDeviceAgentLocationId);
        }

        private static List<Claim> Claims
        {
            get
            {
                var principal = (GenericPrincipal)Thread.CurrentPrincipal;
                return principal.Claims
                    .Where(claim => !claim.Type.Contains("role"))
                    .ToList();
            }
        }

        private static List<Claim> Roles
        {
            get
            {
                var principal = (GenericPrincipal)Thread.CurrentPrincipal;
                return principal.Claims
                    .Where(claim => claim.Type.Contains("role"))
                    .ToList();
            }
        }

        private static string GetClaimValue(string claimType)
        {
            var principal = (GenericPrincipal)Thread.CurrentPrincipal;
            return principal?.Claims?.FirstOrDefault(claim => claim.Type == claimType)?.Value;
        }

        public static IEnumerable<string> GetClaimValues(string claimType)
        {
            var principal = (GenericPrincipal)Thread.CurrentPrincipal;
            return principal?.Claims?.Where(claim => claim.Type == claimType)?.Select(claim => claim.Value).ToList();
        }

        private static bool GetBooleanClaimValue(string claimType)
        {
            var claimValue = GetClaimValue(claimType);
            return !string.IsNullOrEmpty(claimValue) ? claimValue.ToLower().Equals("true") : false;
        }
    }
}