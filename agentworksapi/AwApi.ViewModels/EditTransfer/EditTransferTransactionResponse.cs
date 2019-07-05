using System;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace AwApi.ViewModels.EditTransfer
{
    [Serializable]
    public class EditTransferTransactionResponse: Response
    {
        public EditTransferTransactionResponsePayload EditTransferTransactionResponsePayload { get; set; }
    }
}