using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace AwApi.Business.BusinessRules
{
    public static class SearchStagedTransactionsRequestBusinessRules
    {
        public static SearchStagedTransactionsRequest ApplyBusinessRules(this SearchStagedTransactionsRequest req)
        {
            const string defMaxRecordToReturn = "99";

            if (string.IsNullOrEmpty(req.MaxRowsToReturn))
            {
                req.MaxRowsToReturn = defMaxRecordToReturn;
            }

            return req;
        }
    }
}