using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.Send;
using MoneyGram.AgentConnect.IntegrationTest.Data.Setup;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.Common
{
    public static class FeeLookupRequestFactory
    {
        private static FeeLookupRequest GenerateBaseFeeLookupRequest(string toCountry, string toState, string sendCurr, AmountRange amtRange, double amount, ItemChoiceType1 itemChoiceType)
        {
            return new FeeLookupRequest
            {
                DefaultMaxFee = false,
                AllOptions = true,
                DestinationCountry = toCountry,
                DestinationCountrySubdivisionCode = toState,
                SendCurrency = sendCurr,
                Item = amtRange == AmountRange.CustomAmount ? (decimal)amount : GenerateAmount(amtRange),
                ItemElementName = itemChoiceType,
            };
        }

        public static FeeLookupRequest FeeLookupRequestForSend(SendData sendData)
        {
            var feeLookupRequest = GenerateBaseFeeLookupRequest(sendData.SendRequest.Country, sendData.SendRequest.State, sendData.SendRequest.SendCurr, sendData.SendRequest.AmtRange, sendData.SendRequest.Amount, sendData.SendRequest.FeeType);
            feeLookupRequest.MgiSessionType = SessionType.SEND;
            feeLookupRequest.ServiceOption = sendData.SendRequest?.ServiceOption;

            return feeLookupRequest;
        }

        public static FeeLookupRequest FeeLookupForBpEp(string toCountry, string toState, string sendCurr, AmountRange amtRange, ItemChoiceType1 itemChoiceType, string receiveCode, string receiveAgentId, string agentId, string agentPos)
        {
            var feeLookupRequest = GenerateBaseFeeLookupRequest(toCountry, toState, sendCurr, amtRange, 0.0 /*custom amount currently unsupported for Bill Pay*/, itemChoiceType);
            feeLookupRequest.MgiSessionType = SessionType.BP;
            feeLookupRequest.ProductVariant = ProductVariantType.EP;
            feeLookupRequest.ReceiveCode = receiveCode;
            feeLookupRequest.ReceiveAgentID = receiveAgentId;
            feeLookupRequest.AgentID = agentId;
            feeLookupRequest.AgentSequence = agentPos;

            return feeLookupRequest;
        }

        private static decimal GenerateAmount(AmountRange amtRange)
        {
            switch (amtRange)
            {
                case AmountRange.UnderOneHundred:
                    return DataGenerator.Amount(25.00m, 99.99m);
                case AmountRange.NoIdsNoThirdParty:
                    return DataGenerator.Amount(25.00m, 499.99m);
                case AmountRange.SingleId:
                    return DataGenerator.Amount(500.00m, 999.99m);
                case AmountRange.TwoIds:
                    return DataGenerator.Amount(1000.00m, 2999.99m);
                case AmountRange.ThirdParty:
                    return DataGenerator.Amount(3000.00m, 5999.00m);
                default:
                    return DataGenerator.Amount(25.00m, 150.00m);
            }
        }
    }
}