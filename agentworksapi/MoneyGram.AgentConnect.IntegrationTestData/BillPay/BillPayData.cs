using System.Collections.Generic;
using System.Linq;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.Common;
using MoneyGram.AgentConnect.IntegrationTest.Data.Requests;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.BillPay
{
    public class BillPayData : TransactionData
    {
	    public BillPayData()
	    {
		    Init();
	    }
        public BillPayData(BillPayOperationRequest billPayRequest)
        {
            Set(billPayRequest);
            Init();
        }
	    private void Init()
	    {
			FeeLookup = new FeeLookupData();
		    BillPayValidationRequestList = new List<BPValidationRequest>();
		    BillPayValidationResponseList = new List<BPValidationResponse>();
	    }

		public BillPayOperationRequest BillPayRequest { get; private set; }

		public BillerSearchRequest BillerSearchRequest { get; private set; }
        public BillerSearchResponse BillerSearchResponse { get; private set; }

		public ConsumerHistoryLookupData ConsumerHistoryLookup { get; private set; }
        public ConsumerHistoryLookupRequest ConsumerHistoryLookupRequest { get; private set; }
        public ConsumerHistoryLookupResponse ConsumerHistoryLookupResponse { get; private set; }
		public FeeLookupData FeeLookup { get; private set; }
        public List<BPValidationRequest> BillPayValidationRequestList { get; private set; }
		public List<BPValidationResponse> BillPayValidationResponseList { get; private set; }

        public CompleteSessionRequest CompleteSessionRequest { get; private set; }
        public CompleteSessionResponse CompleteSessionResponse { get; private set; }

	    public void Set(BillPayOperationRequest billPayRequest)
	    {
		    BillPayRequest = billPayRequest;
			SetExecutionOrder(nameof(BillPayRequest));
	    }

		public void Set(BPValidationRequest billPayValidationRequest)
		{
			var isFirstSet = !BillPayValidationRequestList.Any();

			BillPayValidationRequestList.Add(billPayValidationRequest);
			if (isFirstSet) SetExecutionOrder(nameof(BillPayValidationRequestList));
		}

		public void Set(List<BPValidationRequest> billPayValidationRequestList)
		{
			var isFirstSet = billPayValidationRequestList.Any() && !BillPayValidationRequestList.Any();

			BillPayValidationRequestList.AddRange(billPayValidationRequestList);
			if (isFirstSet) SetExecutionOrder(nameof(BillPayValidationRequestList));
		}

		public void Set(BPValidationResponse billPayValidationResponse)
		{
			var isFirstSet = !BillPayValidationResponseList.Any();

			BillPayValidationResponseList.Add(billPayValidationResponse);
			if (isFirstSet) SetExecutionOrder(nameof(BillPayValidationResponseList));
		}

		public void Set(List<BPValidationResponse> billPayValidationResponseList)
		{
			var isFirstSet = billPayValidationResponseList.Any() && !BillPayValidationResponseList.Any();

			BillPayValidationResponseList.AddRange(billPayValidationResponseList);
			if (isFirstSet) SetExecutionOrder(nameof(BillPayValidationResponseList));
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
        public void Set(ConsumerHistoryLookupRequest consumerHistoryLookupRequest)
        {
            ConsumerHistoryLookupRequest = consumerHistoryLookupRequest;
            SetExecutionOrder(nameof(ConsumerHistoryLookupRequest));
        }
        public void Set(ConsumerHistoryLookupResponse consumerHistoryLookupResponse)
        {
            ConsumerHistoryLookupResponse = consumerHistoryLookupResponse;
            SetExecutionOrder(nameof(ConsumerHistoryLookupResponse));
        }
        public void Set(ConsumerHistoryLookupData consumerHistoryLookupData)
		{
		    ConsumerHistoryLookup = consumerHistoryLookupData;
			SetExecutionOrder(nameof(ConsumerHistoryLookup));
		}
		public void Set(FeeLookupData feeLookupData)
		{
			FeeLookup = feeLookupData;
			SetExecutionOrder(nameof(FeeLookup));
		}
	    public void Set(BillerSearchRequest billerSearchRequest)
	    {
		    BillerSearchRequest = billerSearchRequest;
		    SetExecutionOrder(nameof(BillerSearchRequest));
	    }
	    public void Set(BillerSearchResponse billerSearchResponse)
	    {
		    BillerSearchResponse = billerSearchResponse;
		    SetExecutionOrder(nameof(BillerSearchResponse));
	    }
	}
}