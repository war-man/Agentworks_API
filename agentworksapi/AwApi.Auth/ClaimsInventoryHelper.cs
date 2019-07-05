using AwApi.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace AwApi.Auth
{
    public static class ClaimsInventoryHelper
    {
        // This list comes from ClaimsNames.cs
        private static readonly List<string> ExpectedOpenAmClaims = new List<string>
        {
            ClaimsNames.Sub,
            ClaimsNames.MgiAgentLocationId,
//            ClaimsNames.MgiUserLastLoginTs,
            ClaimsNames.GivenName,
//             Non-transactional devices will not get these claims
//            ClaimsNames.MgiDevicePosNumber,
//            ClaimsNames.MgiDeviceAgentId,
            ClaimsNames.MgiMainOfficeId,
//            ClaimsNames.Email,
            ClaimsNames.Name,
            ClaimsNames.FamilyName,
            ClaimsNames.Language,
            ClaimsNames.Roles
//             ClaimsNames.UpdatedAt,
//             ClaimsNames.MgiAppAwMaxReceiveAmount,
//             ClaimsNames.MgiAppAwMaxAmendAmount,
//             ClaimsNames.MgiAppAwMaxBillPayAmount,
//             ClaimsNames.MgiAppAwMaxSendCancellationAmount,
//             ClaimsNames.MgiAppAwMaxReceiveCancellationAmount,
//             ClaimsNames.MgiAppAwMaxSendAmount
//             ClaimsNames.MgiAppAwMaxMoneyOrderAmount,
//             ClaimsNames.MgiAppAwMaxVendorPaymentAmount
        };

        private static readonly List<string> ExpectedCamsDeviceClaims = new List<string>
        {
            ClaimsNames.MgiMainOfficeId,
            ClaimsNames.MgiDeviceAgentLocationId,
            ClaimsNames.MgiDevicePosNumber,
            ClaimsNames.Language,
            ClaimsNames.Roles
        };

        private static readonly List<string> ExpectedCamsUnregisteredDeviceClaims = new List<string>
        {
            ClaimsNames.MgiDeviceId,
            ClaimsNames.Roles
        };

        private static readonly List<string> ExpectedPartnerServiceClaims = new List<string>
        {
            ClaimsNames.AgentPassword
        };

        private static readonly List<string> ExpectedAgentProfileClaims = new List<string>
        {
            ClaimsNames.AgentName,
            ClaimsNames.StoreName,
            ClaimsNames.AgentTimeZone,
            ClaimsNames.AgentTelNo,
            ClaimsNames.PrimaryReceiptLanguage,
            ClaimsNames.CanSend,
            ClaimsNames.CanSendCompletion,
            ClaimsNames.CanAmend,
            ClaimsNames.CanCancel,
            ClaimsNames.CanSendReversal,
            ClaimsNames.CanFeeRefund,
            ClaimsNames.CanReceive,
            ClaimsNames.CanReceiveCompletion,
            ClaimsNames.CanReceiveReversal,
            ClaimsNames.CanBillPay,
            ClaimsNames.CanBillPayCompletion,
            ClaimsNames.CanLoadPrepaidCard,
            ClaimsNames.CanMoneyOrder,
            ClaimsNames.CanVendorPay,
            ClaimsNames.CanExpressPayment,
            ClaimsNames.CanUtilityBillPay
        };

        private static readonly List<string> ExpectedRolesClaims = new List<string>
        {
            ClaimsNames.Roles
        };

        public static List<string> CheckAll(List<Claim> claims, bool ignoreRoles = false)
        {
            var expectedClaims = new List<string>();
            expectedClaims.AddRange(ExpectedOpenAmClaims);
            expectedClaims.AddRange(ExpectedPartnerServiceClaims);
            expectedClaims.AddRange(ExpectedAgentProfileClaims);
            if(ignoreRoles)
            {
                expectedClaims.Remove(ClaimsNames.Roles);
            }
            if(!MgiAwRole.IsInTransactionalRole(claims))
            {
                expectedClaims.Remove(ClaimsNames.MgiAppAwMaxSendAmount);
                expectedClaims.Remove(ClaimsNames.MgiAppAwMaxBillPayAmount);
                expectedClaims.Remove(ClaimsNames.MgiAppAwMaxReceiveAmount);
                expectedClaims.Remove(ClaimsNames.MgiAppAwMaxSendCancellationAmount);
                expectedClaims.Remove(ClaimsNames.MgiAppAwMaxReceiveCancellationAmount);
                expectedClaims.Remove(ClaimsNames.MgiAppAwMaxAmendAmount);
                expectedClaims.Remove(ClaimsNames.MgiAppAwMaxMoneyOrderAmount);
                expectedClaims.Remove(ClaimsNames.MgiAppAwMaxVendorPaymentAmount);
            }
            return Compare(claims, expectedClaims);
        }

        public static List<string> CheckCamsClaims(List<Claim> claims)
        {
            var expectedClaims = new List<string>();
            expectedClaims.AddRange(ExpectedOpenAmClaims);

            if(!MgiAwRole.IsInTransactionalRole(claims))
            {
                expectedClaims.Remove(ClaimsNames.MgiAppAwMaxSendAmount);
                expectedClaims.Remove(ClaimsNames.MgiAppAwMaxBillPayAmount);
                expectedClaims.Remove(ClaimsNames.MgiAppAwMaxReceiveAmount);
                expectedClaims.Remove(ClaimsNames.MgiAppAwMaxSendCancellationAmount);
                expectedClaims.Remove(ClaimsNames.MgiAppAwMaxReceiveCancellationAmount);
                expectedClaims.Remove(ClaimsNames.MgiAppAwMaxAmendAmount);
                expectedClaims.Remove(ClaimsNames.MgiAppAwMaxMoneyOrderAmount);
                expectedClaims.Remove(ClaimsNames.MgiAppAwMaxVendorPaymentAmount);
            }

            return Compare(claims, expectedClaims);
        }

        public static List<string> CheckCamsDeviceClaims(List<Claim> claims)
        {
            return Compare(claims, ExpectedCamsDeviceClaims);
        }

        public static List<string> CheckCamsUnregisteredDeviceClaims(List<Claim> claims)
        {
            return Compare(claims, ExpectedCamsUnregisteredDeviceClaims);
        }

        public static List<string> CheckPartnerServiceClaims(List<Claim> claims)
        {
            return Compare(claims, ExpectedPartnerServiceClaims);
        }

        public static List<string> CheckAgentProfileClaims(List<Claim> claims)
        {
            return Compare(claims, ExpectedAgentProfileClaims);
        }

        public static List<string> CheckRolesClaims(List<string> claims)
        {
            var missingClaims = ExpectedRolesClaims.Except(claims);
            return missingClaims.ToList();
        }

        private static readonly List<string> excludedOpenAmClaims = new List<string>
        {
            ClaimsNames.Country
        };

        public static void RemoveExcludedOpenAmClaims(List<Claim> claims)
        {
            claims.RemoveAll(c => excludedOpenAmClaims.Contains(c.Type));
        }

        private static List<string> Compare(List<Claim> CompareList, List<string> CompareToList)
        {
            var claimsInventory = CompareList.Select(x => x.Type);
            var missingClaims = CompareToList.Except(claimsInventory);
            return missingClaims.ToList();
        }
    }
}