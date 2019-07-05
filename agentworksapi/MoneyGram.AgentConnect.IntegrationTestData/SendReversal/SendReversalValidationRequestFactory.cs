using System.Collections.Generic;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.SendReversal
{
    public static class SendReversalValidationRequestFactory
    {
        public static SendReversalValidationRequest NewRequestWithBaseData()
        {
            return new SendReversalValidationRequest
            {
                FieldValues = new List<KeyValuePairType>(),
                VerifiedFields = new List<string>()
                //TODO: Set 'FeeRefund'
            };
        }

        public static SendReversalValidationRequest FromTransactionLookup(this SendReversalValidationRequest sendReversalValidationReq, TransactionLookupResponsePayload tranLookupResponsePayload)
        {
            sendReversalValidationReq.MgiSessionID = tranLookupResponsePayload.MgiSessionID;
            sendReversalValidationReq.SendAmount = tranLookupResponsePayload.SendAmounts.SendAmount.GetValueOrDefault();
            sendReversalValidationReq.SendCurrency = tranLookupResponsePayload.SendAmounts.SendCurrency;

            return sendReversalValidationReq;
        }
    }
}