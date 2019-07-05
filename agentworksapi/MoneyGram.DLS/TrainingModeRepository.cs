using System;
using System.Collections.Generic;
using System.Linq;
using MoneyGram.Common.TrainingMode;
using MoneyGram.Common.TrainingMode.Enums;
using MoneyGram.DLS.DomainModel;
using MoneyGram.DLS.DomainModel.Response;

namespace MoneyGram.DLS
{
    public class TrainingModeRepository : ITrainingModeRepository
    {
        public MTTransactionDetailLookupResponse MockMTTransactionDetailLookup(IList<string> strPosIdList)
        {
            Func<TrainingModeResponses, string> getFileName = null;
            var response = TrainingModeConfiguration.GetResponse<MTTransactionDetailLookupResponse>(SessionType.REP, null,
                getFileName = r => r.ReportDailyTransactionActivityMtTransaction);

            //adjust POS numbers to requested
            if (strPosIdList.Any())
            {
                foreach (var transaction in response.GetMTTransactionDetailLookupResultList)
                {
                    transaction.PosId = strPosIdList.First();
                }
            }
            else //no POS selected - returning empty list
            {
                response.GetMTTransactionDetailLookupResultList = new List<TransactionDetailLookupResult>();
            }

            return response;
        }

        public BPTransactionDetailLookupResponse MockBPTransactionDetailLookup(IList<string> strPosIdList)
        {
            Func<TrainingModeResponses, string> getFileName = null;
            var response = TrainingModeConfiguration.GetResponse<BPTransactionDetailLookupResponse>(SessionType.REP, null,
                getFileName = r => r.ReportDailyTransactionActivityBillPayTransaction);

            //adjust POS numbers to requested
            if (strPosIdList.Any())
            {
                foreach (var transaction in response.GetDailyTransactionDetailLookupResultList)
                {
                    transaction.PosId = strPosIdList.First();
                }
            }
            else //no POS selected - returning empty list
            {
                response.GetDailyTransactionDetailLookupResultList = new List<TransactionDetailLookupResult>();
            }

            return response;
        }
    }
}