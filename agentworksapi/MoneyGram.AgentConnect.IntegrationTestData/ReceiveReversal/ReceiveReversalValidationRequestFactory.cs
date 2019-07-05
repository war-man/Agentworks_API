using System.Collections.Generic;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.ReceiveReversal
{
    public static class ReceiveReversalValidationRequestFactory
    {
        public static ReceiveReversalValidationRequest NewRequestWithBaseData()
        {
            return new ReceiveReversalValidationRequest
            {
                FieldValues = new List<KeyValuePairType>(),
                VerifiedFields = new List<string>()
            };
        }

        public static ReceiveReversalValidationRequest FromTransactionLookup(this ReceiveReversalValidationRequest receiveReversalValidationReq, TransactionLookupResponsePayload tranLookupResponsePayload)
        {
            receiveReversalValidationReq.MgiSessionID = tranLookupResponsePayload.MgiSessionID;
            receiveReversalValidationReq.ReceiveAmount = tranLookupResponsePayload.ReceiveAmounts.ReceiveAmount.GetValueOrDefault();
            receiveReversalValidationReq.ReceiveCurrency = tranLookupResponsePayload.ReceiveAmounts.ReceiveCurrency;

            return receiveReversalValidationReq;
        }
    }
}