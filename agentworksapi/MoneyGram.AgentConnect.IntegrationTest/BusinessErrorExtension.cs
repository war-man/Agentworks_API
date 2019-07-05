using System;
using System.Collections.Generic;
using System.Linq;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace MoneyGram.AgentConnect.IntegrationTest
{
    public static class BusinessErrorExtension
    {
        public static string FormatToLogText(this BusinessError businessError)
        {
            return $"{businessError.ErrorCode}: {businessError.Message}" + (!string.IsNullOrEmpty(businessError.OffendingField) ? $" - {businessError.OffendingField}" : string.Empty);
        }

        public static string Log(this List<BusinessError> listOfErrors)
        {
            if (listOfErrors == null)
            {
                return string.Empty;
            }

            return string.Join(Environment.NewLine, listOfErrors.Select(x => x.FormatToLogText()));
        }
    }
}