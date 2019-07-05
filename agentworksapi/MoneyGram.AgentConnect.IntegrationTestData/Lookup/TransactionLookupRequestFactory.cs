using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.Setup;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.Lookup
{
    public static class TransactionLookupRequestFactory
    {
        private static TransactionLookupRequest GenerateBaseTransactionLookupRequest(string refNum)
        {
            return new TransactionLookupRequest
            {
                ReferenceNumber = refNum
            };
        }

        public static TransactionLookupRequest TransanctionLookupRequestForStatus(string refNum)
        {
            var request = GenerateBaseTransactionLookupRequest(refNum);
            request.PurposeOfLookup = PurposeOfLookup.Status;

            return request;
        }

        public static TransactionLookupRequest TransanctionLookupRequestForReceiveValidation(string refNum)
        {
            var request = GenerateBaseTransactionLookupRequest(refNum);
            request.PurposeOfLookup = PurposeOfLookup.ReceiveValidation;

            return request;
        }

        public static TransactionLookupRequest TransanctionLookupRequestForReceiveCompletion(string refNum)
        {
            var request = GenerateBaseTransactionLookupRequest(refNum);
            request.PurposeOfLookup = PurposeOfLookup.ReceiveCompletion;

            return request;
        }
        public static TransactionLookupRequest TransanctionLookupRequestForReceive(string refNum)
        {
            var request = GenerateBaseTransactionLookupRequest(refNum);
            request.PurposeOfLookup = PurposeOfLookup.Receive;

            return request;
        }
        public static TransactionLookupRequest TransanctionLookupRequestForSendReversal(string refNum)
        {
            var request = GenerateBaseTransactionLookupRequest(refNum);
            request.PurposeOfLookup = PurposeOfLookup.SendReversal;

            return request;
        }

        public static TransactionLookupRequest TransactionLookupRequestForReceiveReversal(string refNum)
        {
            var request = GenerateBaseTransactionLookupRequest(refNum);
            request.PurposeOfLookup = PurposeOfLookup.ReceiveReversal;

            return request;
        }

        public static TransactionLookupRequest TransanctionLookupRequestForCancel(string refNum)
        {
            var request = GenerateBaseTransactionLookupRequest(refNum);
            request.PurposeOfLookup = PurposeOfLookup.Cancel;

            return request;
        }

        public static TransactionLookupRequest TransanctionLookupRequestForAmend(string refNum)
        {
            var request = GenerateBaseTransactionLookupRequest(refNum);
            request.PurposeOfLookup = PurposeOfLookup.Amend;

            return request;
        }

        public static TransactionLookupRequest TransanctionLookupRequestForSendCompletion(string refNum)
        {
            var request = GenerateBaseTransactionLookupRequest(refNum);
            request.PurposeOfLookup = PurposeOfLookup.SendCompletion;

            return request;
        }

        public static TransactionLookupRequest TransanctionLookupRequestForBPCompletion(string refNum)
        {
            var request = GenerateBaseTransactionLookupRequest(refNum);
            request.PurposeOfLookup = PurposeOfLookup.BillPayCompletion;

            return request;
        }
    }
}