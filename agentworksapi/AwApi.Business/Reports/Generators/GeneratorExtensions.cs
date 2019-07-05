using MoneyGram.DLS.DomainModel;
using System;

namespace AwApi.Business.Reports.Generators
{
    public static class GeneratorExtensions
    {
        public static DateTime GetLocalTime(this TransactionDetailLookupResult lookupResult)
        {
            return Convert.ToDateTime(lookupResult.AgentLocalTime);
        }

        public static string GetUserId(this TransactionDetailLookupResult lookupResult)
        {
            var operatorId = string.IsNullOrEmpty(lookupResult.OperatorId) ? string.Empty : lookupResult.OperatorId.ToLower();
            var userId = string.IsNullOrEmpty(operatorId) ? "-" : operatorId;

            return userId;
        }
    }
}
