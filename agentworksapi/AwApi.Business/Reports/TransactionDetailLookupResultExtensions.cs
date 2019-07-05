using System;
using MoneyGram.DLS.DomainModel;

namespace AwApi.Business.Reports
{
    public static class TransactionDetailLookupResultExtensions
    {
        public static bool IsReceiveTransaction(this TransactionDetailLookupResult lookupResult)
        {
            return lookupResult.EventCode == DLSActivityTypeCode.REC.ToString() ||
                   lookupResult.EventCode == DLSActivityTypeCode.RRC.ToString() ||
                   lookupResult.EventCode == DLSActivityTypeCode.REF.ToString() ||
                   lookupResult.EventCode == DLSActivityTypeCode.RSN.ToString();
        }

        public static bool IsSendTransaction(this TransactionDetailLookupResult lookupResult)
        {
            return lookupResult.EventCode == DLSActivityTypeCode.SEN.ToString();
        }

        public static string FormattedPosId(this TransactionDetailLookupResult lookupResult)
        {
            if (string.IsNullOrEmpty(lookupResult.PosId))
            {
                return "-";
            }

            return lookupResult.PosId == "0" ? ReportConstants.Common.SUPER_AGENT : lookupResult.PosId;
        }

        public static string GetTransactionTypeName(this TransactionDetailLookupResult lookupResult)
        {
            if (!Enum.TryParse(lookupResult.EventCode, out DLSActivityTypeCode activityType))
                return ReportConstants.Common.TRAN_TYPE_UNKNOWN;

            switch (activityType)
            {
                case DLSActivityTypeCode.REC:
                    return ReportConstants.Common.TRAN_TYPE_RECEIVE;
                case DLSActivityTypeCode.SEN:
                    return ReportConstants.Common.TRAN_TYPE_SEND;
                case DLSActivityTypeCode.RSN:
                case DLSActivityTypeCode.REF:
                    return ReportConstants.Common.TRAN_TYPE_REFUND;
                case DLSActivityTypeCode.RRC:
                    return ReportConstants.Common.TRAN_TYPE_REVERSAL;
                
                default:
                    return ReportConstants.Common.TRAN_TYPE_UNKNOWN;
            }
        }
    }
}