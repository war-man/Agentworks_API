using System.Collections.Generic;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.Consumer
{
    public static class SearchConsumerProfilesRequestFactory
    {
        public static SearchConsumerProfilesRequest NewSearchConsumerProfilesRequest()
        {         
            return new SearchConsumerProfilesRequest()
            {
                FieldValues = new List<KeyValuePairType>()
            };
        }
    }
}
