using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.Common;
using System.Collections.Generic;
using MoneyGram.AgentConnect.IntegrationTest.Data.Requests;
using MoneyGram.AgentConnect.IntegrationTest.Data.Send;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.Recv
{
	public class ReceiveData : TransactionData
    {
        public ReceiveData()
        {
	        Init();
        }

	    public ReceiveData(ReceiveRequest receiveRequest)
	    {
		    Set(receiveRequest);
		    Init();
	    }

	    public ReceiveRequest ReceiveRequest { get; private set; }
	    public SendData SendData { get; private set; }
	    public TransactionLookupRequest TransactionLookupRequest { get; private set; }
	    public TransactionLookupResponse TransactionLookupResponse { get; private set; }

	    //TODO: Do we need these staged request/Response?
	    //public TransactionLookupRequest StagedTransactionLookupRequest { get; private set; }
	    //public TransactionLookupResponse StagedTransactionLookupResponse { get; private set; }

	    public List<ReceiveValidationRequest> ReceiveValidationRequests { get; private set; }
	    public List<ReceiveValidationResponse> ReceiveValidationResponses { get; private set; }
	    public CompleteSessionRequest CompleteSessionRequest { get; set; }
	    public CompleteSessionResponse CompleteSessionResponse { get; set; }
	    public bool Staging { get; private set; }

		private void Init()
	    {
			ReceiveValidationResponses = new List<ReceiveValidationResponse>();
		    ReceiveValidationRequests = new List<ReceiveValidationRequest>();
		    TransactionLookupRequest = new TransactionLookupRequest();
		    TransactionLookupResponse = new TransactionLookupResponse();
		    CompleteSessionRequest = new CompleteSessionRequest();
		    CompleteSessionResponse = new CompleteSessionResponse();
	    }

	    private void Set(ReceiveRequest receiveRequest)
	    {
		    ReceiveRequest = receiveRequest;
			SetExecutionOrder(nameof(ReceiveRequest));
	    }

	    public void Set(SendData sendData)
	    {
		    SendData = sendData;
		    SetExecutionOrder(nameof(SendData));
	    }

		public void Set(CompleteSessionRequest completeSessionRequest)
	    {
		    CompleteSessionRequest = completeSessionRequest;
		    SetExecutionOrder(nameof(CompleteSessionRequest));
	    }
	    public void Set(CompleteSessionResponse completeSessionResponse)
	    {
		    CompleteSessionResponse = completeSessionResponse;
		    SetExecutionOrder(nameof(CompleteSessionResponse));
	    }

	    public void Set(TransactionLookupRequest transactionLookupRequest)
	    {
		    TransactionLookupRequest = transactionLookupRequest;
			SetExecutionOrder(nameof(TransactionLookupRequest));
	    }
	    public void Set(TransactionLookupResponse transactionLookupResponse)
	    {
		    TransactionLookupResponse = transactionLookupResponse;
		    SetExecutionOrder(nameof(TransactionLookupResponse));
	    }

	    public void Set(bool staging)
	    {
		    Staging = staging;
			SetExecutionOrder(nameof(Staging));
	    }

    }
}