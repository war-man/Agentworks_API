using System;
using System.Collections.Generic;
using System.Linq;
using AwApi.Integration;
using AwApi.Integration.Reports;
using AwApi.ViewModels.Reports;
using MoneyGram.Common.Extensions;
using MoneyGram.DLS.DomainModel;
using MoneyGram.DLS.DomainModel.Request;

namespace AwApi.Business.Reports.Generators
{
    public class DailyTranDetailReportGenerator : IDailyTranDetailReportGenerator
    {
        private readonly IDlsIntegration dlsIntegration;
        private readonly IPartnerServiceIntegration partnerServiceIntegration;

        public DailyTranDetailReportGenerator(IDlsIntegration dlsIntegration, IPartnerServiceIntegration partnerServiceIntegration)
        {
            this.dlsIntegration = dlsIntegration;
            this.partnerServiceIntegration = partnerServiceIntegration;
        }

        public List<BillPayReportDetail> BillPayReport(BPTransactionDetailLookupRequest lookupRequest, List<string> strPosIdList)
        {
            lookupRequest.ThrowIfNull(nameof(lookupRequest));

            var report = new List<BillPayReportDetail>();

            var lookupResponse = dlsIntegration.BPTransactionDetailLookup(lookupRequest, strPosIdList);
            if (lookupResponse == null || lookupResponse.GetDailyTransactionDetailLookupResultList == null)
            {
                return report;
            }

            foreach (var lookupResult in lookupResponse.GetDailyTransactionDetailLookupResultList)
            {
                var row = ProcessBillPayTransaction(lookupResult);
                report.Add(row);
            }

            return report;
        }

        public List<SendReportDetail> SendReport(MTTransactionDetailLookupRequest lookupRequest, List<string> strPosIdList)
        {
            lookupRequest.ThrowIfNull(nameof(lookupRequest));

            var sendReport = new List<SendReportDetail>();

            // DLS call for DailyTransaction
            var lookupResponse = dlsIntegration.MTTransactionDetailLookup(lookupRequest, strPosIdList);

            if (lookupResponse == null || lookupResponse.GetMTTransactionDetailLookupResultList == null)
            {
                return sendReport;
            }

            foreach (var lookupResult in lookupResponse.GetMTTransactionDetailLookupResultList)
            {
                if (lookupResult.IsSendTransaction())
                {
                    var sendRow = ProcessSendTransaction(lookupResult);
                    sendReport.Add(sendRow);
                }
            }

            return sendReport;
        }

        public List<ReceiveReportDetail> ReceiveReport(MTTransactionDetailLookupRequest lookupRequest, List<string> strPosIdList)
        {
            lookupRequest.ThrowIfNull(nameof(lookupRequest));

            var receiveReport = new List<ReceiveReportDetail>();

            // DLS call for DailyTransaction
            var lookupResponse = dlsIntegration.MTTransactionDetailLookup(lookupRequest, strPosIdList);

            if (lookupResponse?.GetMTTransactionDetailLookupResultList == null)
            {
                return receiveReport;
            }

            foreach (var lookupResult in lookupResponse.GetMTTransactionDetailLookupResultList)
            {
                if (!lookupResult.IsReceiveTransaction()) continue;
                var receiveRow = ProcessReceiveTransaction(lookupResult);
                receiveReport.Add(receiveRow);
            }

            return receiveReport;
        }

        private static ReceiveReportDetail ProcessReceiveTransaction(TransactionDetailLookupResult lookupResult)
        {
            //Amount
            var amount = double.Parse(lookupResult.FaceAmount);
            if (lookupResult.EventCode == DLSActivityTypeCode.RSN.ToString())
            {
                amount += double.Parse(lookupResult.FeeAmount);
            }

            return new ReceiveReportDetail
            {
                LocalTime = lookupResult.GetLocalTime(),
                ReferenceNumber = lookupResult.ReferenceId,
                UserId = lookupResult.GetUserId(),
                PosNumber = lookupResult.FormattedPosId(),
                TotalAmount = amount,
                Currency = lookupResult.IsoCurrencyCode ?? string.Empty,
                AuthCode = lookupResult.AuthCode ?? string.Empty,
                ReceiverLastName = lookupResult.ReceiverName.Split(',')[0],
                TransactionType = lookupResult.GetTransactionTypeName()
            };
        }

        private static SendReportDetail ProcessSendTransaction(TransactionDetailLookupResult lookupResult)
        {
            return new SendReportDetail
            {
                LocalTime = lookupResult.GetLocalTime(),
                ReferenceNumber = lookupResult.ReferenceId,
                UserId = lookupResult.GetUserId(),
                PosNumber = lookupResult.FormattedPosId(),
                FaceAmount = double.Parse(lookupResult.FaceAmount),
                FeeAmount = double.Parse(lookupResult.FeeAmount),
                TotalAmount = double.Parse(lookupResult.FaceAmount) + double.Parse(lookupResult.FeeAmount),
                Currency = lookupResult.IsoCurrencyCode ?? string.Empty
            };
        }

        private static BillPayReportDetail ProcessBillPayTransaction(TransactionDetailLookupResult lookupResult)
        {
            var productId = string.Empty;

            if (string.IsNullOrEmpty(lookupResult.ProductId))
            {
                productId = ReportConstants.Common.UTILITY_BILL_PAYMENT_PRODUCT_TYPE;
            }
            else
            {
                productId = lookupResult.ProductId == ReportConstants.Common.EXPRESS_PAYMENT_PRODUCT_ID ? ReportConstants.Common.EXPRESS_PAYMENT_PRODUCT_TYPE : ReportConstants.Common.UTILITY_BILL_PAYMENT_PRODUCT_TYPE;
            }

            return new BillPayReportDetail
            {
                Currency = lookupResult.IsoCurrencyCode,
                LocalTime = lookupResult.GetLocalTime(),
                ReferenceNumber = lookupResult.ReferenceId,
                UserId = lookupResult.GetUserId(),
                PosNumber = lookupResult.FormattedPosId(),
                FaceAmount = double.Parse(lookupResult.FaceAmount),
                FeeAmount = double.Parse(lookupResult.FeeAmount),
                TotalAmount = double.Parse(lookupResult.FaceAmount) + double.Parse(lookupResult.FeeAmount),
                ProductType = productId
            };
        }
    }
}