using System.Collections.Generic;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.Consumer
{
    public static class CreateOrUpdateProfileConsumerRequestFactory
    {
        public static CreateOrUpdateProfileConsumerRequest NewRequestWithBaseData()
        {
            return PopulateBaseData();
        }

        private static CreateOrUpdateProfileConsumerRequest PopulateBaseData()
        {
            return new CreateOrUpdateProfileConsumerRequest()
            {
                GAFVersionNumber = string.Empty,                
                FieldValues = new List<KeyValuePairType>(),
                VerifiedFields = new List<string>(),                
            };
        }
    }
}