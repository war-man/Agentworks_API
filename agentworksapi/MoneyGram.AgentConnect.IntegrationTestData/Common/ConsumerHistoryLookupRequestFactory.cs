using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.Common
{
    public static class ConsumerHistoryLookupRequestFactory
    {
        private static readonly string NumOfCustomers = "10";

        public static ConsumerHistoryLookupRequest ConsumerHistoryLookupRequestExisting => new ConsumerHistoryLookupRequest
        {
            ConsumerPhone = "9525913694",
            MaxSendersToReturn = NumOfCustomers,
            MaxReceiversToReturn = NumOfCustomers
        };

        public static ConsumerHistoryLookupRequest ConsumerHistoryLookupRequestNonExisting => new ConsumerHistoryLookupRequest
        {
            ConsumerPhone = "8005559696",
            MaxSendersToReturn = NumOfCustomers,
            MaxReceiversToReturn = NumOfCustomers
        };
    }
}