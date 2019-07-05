using MoneyGram.AgentConnect.DomainModel.Exceptions;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.Common;
using MoneyGram.AgentConnect.IntegrationTest.Data.Extensions;
using MoneyGram.AgentConnect.IntegrationTest.Data.Lookup;
using MoneyGram.AgentConnect.IntegrationTest.Data.Recv;
using MoneyGram.AgentConnect.IntegrationTest.Data.Setup;
using MoneyGram.AgentConnect.IntegrationTest.Operations.Common;
using MoneyGram.AgentConnect.IntegrationTest.Operations.Helpers;
using MoneyGram.AgentConnect.IntegrationTest.Operations.Lookup;
using System.Linq;

namespace MoneyGram.AgentConnect.IntegrationTest.Operations.Recv
{
	public class ReceiveOperations
	{
		private TestAgentConnectIntegration AcIntegration { get; }
		private readonly GetAllFieldsOperations _gafOperations;
		private readonly ValidationOperations _validationOperations;
		public ReceiveOperations(TestRunner testRunner)
		{
			AcIntegration = new TestAgentConnectIntegration(testRunner);
			_gafOperations = new GetAllFieldsOperations(testRunner);
			_validationOperations = new ValidationOperations(testRunner);
		}
		public ReceiveData SingleValidateForReceive(ReceiveData receiveData)
		{
			try
			{
                receiveData.ReceiveRequest.PopulateAgentData(receiveData.ReceiveRequest.AgentState);
                // Get the all fields grouping and types for generation
                receiveData.Set(_gafOperations.GetInfoKeysWithGroupTypes(receiveData.ReceiveRequest.AgentId, receiveData.ReceiveRequest.AgentPos, receiveData.GafInfoKeysWithGroups, GetAllFieldsTransactionType.Receive));
				// Transaction Lookup Request
				CreateTransactionLookupRequest(receiveData);
				// Transaction Lookup Response
				TransactionLookup(receiveData);

				// Validate Receive
				_validationOperations.RecvValidate(receiveData, receiveData.ReceiveRequest.ThirdPartyType);
				if (DataErrorHandler.CheckForNestedErrors(receiveData).Any())
				{
					throw new AgentConnectException();
				}

			}
			catch (AgentConnectException acExp)
			{
				receiveData.Set(new BusinessError
				{
					ErrorCategory = "AC Exception",
					ErrorCode = acExp.ErrorCode.ToString(),
					Message = acExp.Message
				});
			}
			return receiveData;
		}
		public ReceiveData ValidateUntilReadyForCommitForReceive(ReceiveData receiveData)
		{
			try
			{
                // Get the all fields grouping and types for generation
                receiveData.Set(_gafOperations.GetInfoKeysWithGroupTypes(receiveData.ReceiveRequest.AgentId, receiveData.ReceiveRequest.AgentPos, receiveData.GafInfoKeysWithGroups, GetAllFieldsTransactionType.Receive));
				// Transaction Lookup Request
				CreateTransactionLookupRequest(receiveData);
				// Transaction Lookup Response
				TransactionLookup(receiveData);

				// Initial Validate Receive
				_validationOperations.RecvValidate(receiveData, TestThirdPartyType.None);
				if (DataErrorHandler.CheckForNestedErrors(receiveData).Any())
				{
					throw new AgentConnectException();
				}
				_validationOperations.RecvValidateUntilReadyForCommit(receiveData);
				if (DataErrorHandler.CheckForNestedErrors(receiveData).Any())
				{
					throw new AgentConnectException();
				}

			}
			catch (AgentConnectException acExp)
			{
				receiveData.Set(new BusinessError
				{
					ErrorCategory = "AC Exception",
					ErrorCode = acExp.ErrorCode.ToString(),
					Message = acExp.Message
				});
			}
			return receiveData;
		}
		public ReceiveData ReceiveComplete(ReceiveData receiveData)
		{
			try
			{
                receiveData.ReceiveRequest.PopulateAgentData(receiveData.ReceiveRequest.AgentState);
                // Get the all fields grouping and types for generation
                receiveData.Set(_gafOperations.GetInfoKeysWithGroupTypes(receiveData.ReceiveRequest.AgentId, receiveData.ReceiveRequest.AgentPos, receiveData.GafInfoKeysWithGroups, GetAllFieldsTransactionType.Receive));
				// Transaction Lookup Request
				CreateTransactionLookupRequest(receiveData);
				// Transaction Lookup Response
				TransactionLookup(receiveData);
				// Initial Validate Receive
				_validationOperations.RecvValidate(receiveData, receiveData.ReceiveRequest.ThirdPartyType);
				if (DataErrorHandler.CheckForNestedErrors(receiveData).Any())
				{
					throw new AgentConnectException();
				}
				_validationOperations.RecvValidateUntilReadyForCommit(receiveData);
				if (DataErrorHandler.CheckForNestedErrors(receiveData).Any())
				{
					throw new AgentConnectException();
				}
				CompleteSession(receiveData);
			}
			catch (AgentConnectException acExp)
			{
				receiveData.Set(new BusinessError
				{
					ErrorCategory = "AC Exception",
					ErrorCode = acExp.ErrorCode.ToString(),
					Message = acExp.Message
				});
			}
			return receiveData;
		}
		private void TransactionLookup(ReceiveData receiveData)
		{
            receiveData.ReceiveRequest.PopulateAgentData(receiveData.ReceiveRequest.AgentState);
            if (receiveData.TransactionLookupRequest == null)
			{
				CreateTransactionLookupRequest(receiveData);
			}
            receiveData.TransactionLookupRequest.AgentID = receiveData.ReceiveRequest.AgentId;
            receiveData.TransactionLookupRequest.AgentSequence = receiveData.ReceiveRequest.AgentPos;

            receiveData.Set(AcIntegration.TransactionLookup(receiveData.TransactionLookupRequest));
		}
		private static void CreateTransactionLookupRequest(ReceiveData receiveData)
		{
			receiveData.TransactionLookupRequest.ReferenceNumber = receiveData.SendData.CompleteSessionResp.Payload.ReferenceNumber;
			receiveData.TransactionLookupRequest.PurposeOfLookup = PurposeOfLookup.Receive;
		}

		private void CompleteSession(ReceiveData receiveData)
		{
            receiveData.ReceiveRequest.PopulateAgentData(receiveData.ReceiveRequest.AgentState);
            var mgiSessionId = receiveData.ReceiveValidationResponses.Last().Payload.MgiSessionID;
			receiveData.Set(CompleteSessionRequestFactory.CompleteRecvRequest(mgiSessionId));
			if (receiveData.Staging)
			{
				receiveData.CompleteSessionRequest.Commit = false;
			}
            receiveData.CompleteSessionRequest.AgentID = receiveData.ReceiveRequest.AgentId;
            receiveData.CompleteSessionRequest.AgentSequence = receiveData.ReceiveRequest.AgentPos;

            receiveData.Set(AcIntegration.CompleteSession(receiveData.CompleteSessionRequest));
		}

		public ReceiveData TransactionLookupForReceiveCompletion(ReceiveData receiveData)
		{
            receiveData.ReceiveRequest.PopulateAgentData(receiveData.ReceiveRequest.AgentState);
            var transactionLookupRequest =
				TransactionLookupRequestFactory.TransanctionLookupRequestForReceiveCompletion(receiveData.SendData.CompleteSessionResp.Payload.ReferenceNumber);
            transactionLookupRequest.AgentID = receiveData.ReceiveRequest.AgentId;
            transactionLookupRequest.AgentSequence = receiveData.ReceiveRequest.AgentPos;

            receiveData.Set(transactionLookupRequest);
			var transactionLookupResponse =
				AcIntegration.TransactionLookup(receiveData.TransactionLookupRequest);
			receiveData.Set(transactionLookupResponse);//TODO: This was StagedTransactionLookupRequest
			return receiveData;
		}

		public ReceiveData ReceiveCompleteStaged(ReceiveData receiveData)
		{
			try
			{
                receiveData.ReceiveRequest.PopulateAgentData(receiveData.ReceiveRequest.AgentState);
                // Get the all fields grouping and types for generation
                receiveData.Set(_gafOperations.GetInfoKeysWithGroupTypes(receiveData.ReceiveRequest.AgentId, receiveData.ReceiveRequest.AgentPos, receiveData.GafInfoKeysWithGroups, GetAllFieldsTransactionType.Receive));
				// Transaction Lookup Request
				CreateTransactionLookupRequest(receiveData);
				// Transaction Lookup Response
				TransactionLookup(receiveData);

				// Initial Validate
				receiveData.Set(true);
				_validationOperations.RecvValidate(receiveData, receiveData.ReceiveRequest.ThirdPartyType, receiveData.Staging);
				if (DataErrorHandler.CheckForNestedErrors(receiveData).Any())
				{
					throw new AgentConnectException();
				}
				// Final Validate
				_validationOperations.RecvStagedValidateUntilReadyForCommit(receiveData);
				if (DataErrorHandler.CheckForNestedErrors(receiveData).Any())
				{
					throw new AgentConnectException();
				}

				// Complete
				CompleteSession(receiveData);
			}
			catch (AgentConnectException acExp)
			{
				receiveData.Set(new BusinessError
				{
					ErrorCategory = "AC Exception",
					ErrorCode = acExp.ErrorCode.ToString(),
					Message = acExp.Message
				});
			}
			return receiveData;
		}
	}
}