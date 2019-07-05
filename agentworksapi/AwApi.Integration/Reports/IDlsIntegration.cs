using System.Collections.Generic;
using MoneyGram.Common.Models;
using MoneyGram.DLS.DomainModel.Request;
using MoneyGram.DLS.DomainModel.Response;

namespace AwApi.Integration
{
    public interface IDlsIntegration
    {
        HealthCheckResponse HealthCheck();

        DailyTransactionDetailLookupResponse DailyTransactionDetailLookup(DailyTransactionDetailLookupRequest dailyTransactionDetailLookupRequest);

        BPTransactionDetailLookupResponse BPTransactionDetailLookup(BPTransactionDetailLookupRequest bpTransactionDetailLookupRequest, IList<string> strPosIdList);

        MTTransactionDetailLookupResponse MTTransactionDetailLookup(MTTransactionDetailLookupRequest mtTransactionDetailLookupRequest, IList<string> strPosIdList);
    }
}