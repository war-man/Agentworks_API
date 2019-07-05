using System.Collections.Generic;
using AwApi.ViewModels.Reports;
using MoneyGram.DLS.DomainModel.Request;

namespace AwApi.Business.Reports.Generators
{
    public interface IDailyTranDetailReportGenerator
    {
        List<BillPayReportDetail> BillPayReport(BPTransactionDetailLookupRequest lookupRequest, List<string> strPosIdList);
        List<SendReportDetail> SendReport(MTTransactionDetailLookupRequest lookupRequest, List<string> strPosIdList);
        List<ReceiveReportDetail> ReceiveReport(MTTransactionDetailLookupRequest lookupRequest, List<string> strPosIdList);
    }
}