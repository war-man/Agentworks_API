using System;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace AwApi.ViewModels.EditTransfer
{
    [Serializable]
    public class EditTransferTransactionResponsePayload : Payload
    {
        public ApiResponse<TransactionLookupResponse, ApiData> AmendTransactionLookupResponse { get; set; }
        public ApiResponse<TransactionLookupResponse, ApiData> SendReversalTransactionLookupResponse { get; set; }
        public ApiResponse<TransactionLookupResponse, ApiData> ReceiveReversalTransactionLookupResponse { get; set; }
    }
}