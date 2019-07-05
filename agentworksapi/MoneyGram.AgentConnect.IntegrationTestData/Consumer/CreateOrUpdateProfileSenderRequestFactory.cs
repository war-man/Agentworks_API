using System.Collections.Generic;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.Consumer
{
    public static class CreateOrUpdateProfileSenderRequestFactory
    {
        public static CreateOrUpdateProfileSenderRequest NewRequestWithBaseData()
        {
            return PopulateBaseData();
        }

        private static CreateOrUpdateProfileSenderRequest PopulateBaseData()
        {
            return new CreateOrUpdateProfileSenderRequest
            {
                GAFVersionNumber = string.Empty,
                FieldValues = new List<KeyValuePairType>(),
                VerifiedFields = new List<string>(),
            };
        }
    }
}