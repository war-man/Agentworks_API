using System.Collections.Generic;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.Consumer
{
    public static class CreateOrUpdateProfileReceiverRequestFactory
    {
        public static CreateOrUpdateProfileReceiverRequest NewRequestWithBaseData()
        {
            return PopulateBaseData();
        }

        private static CreateOrUpdateProfileReceiverRequest PopulateBaseData()
        {
            return new CreateOrUpdateProfileReceiverRequest
            {
                GAFVersionNumber = string.Empty,
                FieldValues = new List<KeyValuePairType>(),
                VerifiedFields = new List<string>(),
            };
        }
    }
}