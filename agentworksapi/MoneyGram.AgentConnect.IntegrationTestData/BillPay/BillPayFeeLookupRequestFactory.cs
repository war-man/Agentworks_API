using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.Setup;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.BillPay
{
    public static class BillPayFeeLookupRequestFactory
    {
        public static FeeLookupRequest FeeLookupForEp(string receiveCode, string receiveAgentId, AmountRange amountRange, double amount)
        {
            return new FeeLookupRequest
            {
                MgiSessionType = SessionType.BP,
                ProductVariant = ProductVariantType.EP,
                Item = amountRange == AmountRange.CustomAmount ? (decimal)amount : GenerateAmount(amountRange),
                ItemElementName = ItemChoiceType1.amountIncludingFee,
                DefaultMaxFee = false,
                AllOptions = true,
                DestinationCountry = Country.Usa,
                ReceiveCode = receiveCode,
                ReceiveAgentID = receiveAgentId
            };
        }

        private static decimal GenerateAmount(AmountRange amtRange)
        {
            switch (amtRange)
            {
                case AmountRange.UnderOneHundred:
                    return DataGenerator.Amount(25.00m, 99.99m);
                case AmountRange.NoIdsNoThirdParty:
                    return DataGenerator.Amount(25.00m, 500.00m);
                case AmountRange.SingleId:
                    return DataGenerator.Amount(501.00m, 999.00m);
                case AmountRange.TwoIds:
                    return DataGenerator.Amount(1001.00m, 1499.00m);
                case AmountRange.ThirdParty:
                    return DataGenerator.Amount(1501.00m, 2999.00m);
                default:
                    return DataGenerator.Amount(25.00m, 150.00m);
            }
        }
    }
}