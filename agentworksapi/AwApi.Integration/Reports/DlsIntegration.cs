using System.Collections.Generic;
using MoneyGram.Common.Models;
using MoneyGram.DLS;
using MoneyGram.DLS.DomainModel.Request;
using MoneyGram.DLS.DomainModel.Response;

namespace AwApi.Integration
{
    public class DlsIntegration : IDlsIntegration
    {
        IDLSRepository _repo;

        public DlsIntegration(IDLSRepository repo)
        {
            _repo = repo;
        }

        public DailyTransactionDetailLookupResponse DailyTransactionDetailLookup(
            DailyTransactionDetailLookupRequest dailyTransactionDetailLookupRequest)
        {
            var agent = AuthIntegration.GetAgent();
            return _repo.DailyTransactionDetailLookup(agent.IsInTrainingMode, dailyTransactionDetailLookupRequest);
        }

        public BPTransactionDetailLookupResponse BPTransactionDetailLookup(
            BPTransactionDetailLookupRequest bpTransactionDetailLookupRequest, IList<string> strPosIdList)
        {
            var agent = AuthIntegration.GetAgent();
            return _repo.BPTransactionDetailLookup(agent.IsInTrainingMode, bpTransactionDetailLookupRequest, strPosIdList);
        }

        public MTTransactionDetailLookupResponse MTTransactionDetailLookup(
            MTTransactionDetailLookupRequest mtTransactionDetailLookupRequest, IList<string> strPosIdList)
        {
            var agent = AuthIntegration.GetAgent();
            return _repo.MTTransactionDetailLookup(agent.IsInTrainingMode, mtTransactionDetailLookupRequest, strPosIdList);
        }

        public HealthCheckResponse HealthCheck()
        {
            return new HealthCheckResponse
            {
                ServiceName = ServiceNames.DLS,
                Message = "Not Implemented",
                StatusCode = StatusCode.NotImplemented
            };
        }
    }
}