using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace AwApi.Business.BusinessRules
{
    public static class ConsumerHistoryLookup
    {
        public static ConsumerHistoryLookupRequest ApplyBusinessRules(this ConsumerHistoryLookupRequest req)
        {
            const string defMaxConsumersToReturn = "10";
            if (string.IsNullOrEmpty(req.MaxSendersToReturn))
            {
                req.MaxSendersToReturn = defMaxConsumersToReturn;
            }
            if (string.IsNullOrEmpty(req.MaxReceiversToReturn))
            {
                req.MaxReceiversToReturn = defMaxConsumersToReturn;
            }
            return req;
        }
    }
}