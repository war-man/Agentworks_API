using System.Collections.Generic;
using System.Linq;
using MoneyGram.DLS.EntityMapper.DomainModelExtensions;
using MoneyGram.DLS.EntityMapper.ServiceModelExtensions;
using DOMAIN = MoneyGram.DLS.DomainModel;
using MoneyGram.Common.Extensions;
using System.Threading.Tasks;

namespace MoneyGram.DLS
{
    public class DLSRepository : IDLSRepository
    {
        private readonly IDLSProxyFactory _proxyFactory = null;

        public DLSRepository(IDLSProxyFactory proxyFactory)
        {
            _proxyFactory = proxyFactory;

        }

        /// <summary>
        /// Returns DailyTransactionDetailLookupResponse for the DailyTransactionDetailLookup Request...
        /// </summary>
        /// <param name="isInTrainingMode">If is true Training mode is enabled</param>
        /// <param name="dailyTransactionDetailLookupRequest">DailyTransactionDetailLookupRequest Request</param>
        /// <returns>DailyTransactionDetailLookupResponse</returns>
        public DOMAIN.Response.DailyTransactionDetailLookupResponse DailyTransactionDetailLookup(bool isInTrainingMode, DOMAIN.Request.DailyTransactionDetailLookupRequest dailyTransactionDetailLookupRequest)
        {
            Task<DOMAIN.Response.DailyTransactionDetailLookupResponse> callTask = Task.Run(() => this.DailyTransactionDetailLookupAsync(isInTrainingMode, dailyTransactionDetailLookupRequest));

            callTask.Wait();

            return callTask.Result;
        }

        /// <summary>
        /// Returns DailyTransactionDetailLookupResponse for the DailyTransactionDetailLookup Request...
        /// </summary>
        /// <param name="isInTrainingMode">If is true Training mode is enabled</param>
        /// <param name="dailyTransactionDetailLookupRequest">DailyTransactionDetailLookupRequest Request</param>
        /// <returns>Task<DailyTransactionDetailLookupResponse></returns>
        public async Task<DOMAIN.Response.DailyTransactionDetailLookupResponse> DailyTransactionDetailLookupAsync(bool isInTrainingMode, DOMAIN.Request.DailyTransactionDetailLookupRequest dailyTransactionDetailLookupRequest)
        {
            dailyTransactionDetailLookupRequest.ThrowIfNull(nameof(dailyTransactionDetailLookupRequest));

            var dailyTransactionDetailLookupResponse = await _proxyFactory.UseService(service => service.DailyTransactionDetailLookupAsync(dailyTransactionDetailLookupRequest.ToService()));

            return dailyTransactionDetailLookupResponse.dailyTransactionDetailLookupResponse.ToDomain();
        }

        /// <summary>
        /// Returns BPTransactionDetailLookupResponse for the BPTransactionDetailLookupRequest Request...
        /// </summary>
        /// <param name="isInTrainingMode">If is true Training mode is enabled</param>
        /// <param name="bpTransactionDetailLookupRequest">BPTransactionDetailLookupRequest Request</param>
        /// <param name="strPosIdList">Filtered list of POS Ids</param>
        /// <returns>BPTransactionDetailLookupResponse</returns>
        public DOMAIN.Response.BPTransactionDetailLookupResponse BPTransactionDetailLookup(bool isInTrainingMode, DOMAIN.Request.BPTransactionDetailLookupRequest bpTransactionDetailLookupRequest, IList<string> strPosIdList)
        {
            Task<DOMAIN.Response.BPTransactionDetailLookupResponse> callTask = Task.Run(() => this.BPTransactionDetailLookupAsync(isInTrainingMode, bpTransactionDetailLookupRequest, strPosIdList));

            callTask.Wait();

            return callTask.Result;
        }

        /// <summary>
        /// Returns BPTransactionDetailLookupResponse for the BPTransactionDetailLookupRequest Request...
        /// </summary>
        /// <param name="isInTrainingMode">If is true Training mode is enabled</param>
        /// <param name="bpTransactionDetailLookupRequest">BPTransactionDetailLookupRequest Request</param>
        /// <param name="strPosIdList">Filtered list of POS Ids</param>
        /// <returns>Task<BPTransactionDetailLookupResponse></returns>
        public async Task<DOMAIN.Response.BPTransactionDetailLookupResponse> BPTransactionDetailLookupAsync(bool isInTrainingMode, DOMAIN.Request.BPTransactionDetailLookupRequest bpTransactionDetailLookupRequest, IList<string> strPosIdList)
        {
            bpTransactionDetailLookupRequest.ThrowIfNull(nameof(bpTransactionDetailLookupRequest));

            var bpTransactionDetailLookupResponse = await _proxyFactory.UseService(service => service.BPTransactionDetailLookupAsync(bpTransactionDetailLookupRequest.ToService()));

            var transactions = bpTransactionDetailLookupResponse.bpTransactionDetailLookupResponse.ToDomain();
            transactions.GetDailyTransactionDetailLookupResultList = transactions.GetDailyTransactionDetailLookupResultList
                .Where(x => strPosIdList.Contains(x.PosId)).ToList();

            return bpTransactionDetailLookupResponse.bpTransactionDetailLookupResponse.ToDomain();
        }

        /// <summary>
        /// Returns MTTransactionDetailLookupResponse for the DailyTransactionDetailLookup Request...
        /// </summary>
        /// <param name="isInTrainingMode">If is true Training mode is enabled</param>
        /// <param name="mtTransactionDetailLookupRequest">MTTransactionDetailLookupRequest Request</param>
        /// <param name="strPosIdList">Filtered list of POS Ids</param>
        /// <returns>MTTransactionDetailLookupResponse</returns>
        public DOMAIN.Response.MTTransactionDetailLookupResponse MTTransactionDetailLookup(bool isInTrainingMode, DOMAIN.Request.MTTransactionDetailLookupRequest mtTransactionDetailLookupRequest, IList<string> strPosIdList)
        {
            Task<DOMAIN.Response.MTTransactionDetailLookupResponse> callTask = Task.Run(() => this.MTTransactionDetailLookupAsync(isInTrainingMode, mtTransactionDetailLookupRequest, strPosIdList));

            callTask.Wait();

            return callTask.Result;
        }

        /// <summary>
        /// Returns MTTransactionDetailLookupResponse for the DailyTransactionDetailLookup Request...
        /// </summary>
        /// <param name="isInTrainingMode">If is true Training mode is enabled</param>
        /// <param name="mtTransactionDetailLookupRequest">MTTransactionDetailLookupRequest Request</param>
        /// <param name="strPosIdList">Filtered list of POS Ids</param>
        /// <returns>MTTransactionDetailLookupResponse</returns>
        public async Task<DOMAIN.Response.MTTransactionDetailLookupResponse> MTTransactionDetailLookupAsync(bool isInTrainingMode, DOMAIN.Request.MTTransactionDetailLookupRequest mtTransactionDetailLookupRequest, IList<string> strPosIdList)
        {
            mtTransactionDetailLookupRequest.ThrowIfNull(nameof(mtTransactionDetailLookupRequest));

            var mtTransactionDetailLookupResponse = await _proxyFactory.UseService(service => service.MTTransactionDetailLookupAsync(mtTransactionDetailLookupRequest.ToService()));

            var transactions = mtTransactionDetailLookupResponse.mtTransactionDetailLookupResponse.ToDomain();
            transactions.GetMTTransactionDetailLookupResultList = transactions.GetMTTransactionDetailLookupResultList
                .Where(x => strPosIdList.Contains(x.PosId)).ToList();

            return transactions;
        }
    }
}