using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.Requests;
using MoneyGram.AgentConnect.IntegrationTest.Data.Send;
using MoneyGram.AgentConnect.IntegrationTest.Data.Setup;
using MoneyGram.AgentConnect.IntegrationTest.Operations;
using MoneyGram.AgentConnect.IntegrationTest.Operations.Lookup;
using MoneyGram.AgentConnect.IntegrationTest.Operations.Recv;
using MoneyGram.AgentConnect.IntegrationTest.Operations.Send;
using System;
using System.Linq;
using MoneyGram.AgentConnect.IntegrationTest.Data.Recv;
using MoneyGram.AgentConnect.IntegrationTest.Data.Extensions;

namespace MoneyGram.AgentConnect.IntegrationTest.Recv
{
	/// <summary>
	/// 
	/// </summary>
	[TestClass]
	public class RecvTests
	{
		private ReceiveOperations ReceiveOperations { get; set; }
		private SendOperations SendOperations { get; set; }
		private TransactionLookupOperations TranLookupOperations { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[TestInitialize]
		public void TestSetup()
		{
			var testRunner = new TestRunner();
			ReceiveOperations = new ReceiveOperations(testRunner);
			SendOperations = new SendOperations(testRunner);
			TranLookupOperations = new TransactionLookupOperations(testRunner);
		}

		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Recv_ValidateShouldReturnFieldsToCollect()
		{
			var sendRequest = new SendRequest
			{
				AgentState = AgentLocation.MN,
				Country = Country.Usa,
				State = State.Ny,
				SendCurr = Currency.Usd,
				AmtRange = AmountRange.NoIdsNoThirdParty,
				FeeType = ItemChoiceType1.amountExcludingFee
			};
            sendRequest.PopulateAgentData(sendRequest.AgentState);
            var sendData = new SendData(sendRequest);
			var receiveRequest = new ReceiveRequest
			{
				AgentCountryIsoCode = Country.Usa,
				AgentState = AgentLocation.NY,
                ThirdPartyType = TestThirdPartyType.None
            };
            receiveRequest.PopulateAgentData(receiveRequest.AgentState);

            var receiveData = new ReceiveData(receiveRequest);
			sendData = SendOperations.SendCompleteForNewCustomer(sendData);
			receiveData.Set(sendData);
			ReceiveOperations.SingleValidateForReceive(receiveData);

			var receiveValidationResponse = receiveData.ReceiveValidationResponses?.FirstOrDefault();

			// ASSERT ALL THE THINGS
			Assert.IsFalse(receiveData.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{receiveData.Errors?.Log()}");
			Assert.IsTrue(receiveValidationResponse?.Payload.FieldsToCollect.Any() ?? false);
		}

		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Recv_ValidationUntilReadyForCommit()
		{
			var sendRequest = new SendRequest
			{
				AgentState = AgentLocation.MN,
				Country = Country.Usa,
				State = State.Ny,
				SendCurr = Currency.Usd,
				AmtRange = AmountRange.NoIdsNoThirdParty,
				FeeType = ItemChoiceType1.amountExcludingFee
			};
            sendRequest.PopulateAgentData(sendRequest.AgentState);
            var sendData = new SendData(sendRequest);
			var receiveRequest = new ReceiveRequest
			{
				AgentState = AgentLocation.NY,
                ThirdPartyType = TestThirdPartyType.None
            };
            receiveRequest.PopulateAgentData(receiveRequest.AgentState);

            var receiveData = new ReceiveData(receiveRequest);

            SendOperations.SendCompleteForNewCustomer(sendData);
            receiveData.Set(sendData);
			ReceiveOperations.ValidateUntilReadyForCommitForReceive(receiveData);

			var receiveValidationResponse = receiveData.ReceiveValidationResponses?.FirstOrDefault();

			// ASSERT ALL THE THINGS
			Assert.IsFalse(sendData.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{sendData.Errors?.Log()}");
			Assert.IsTrue(receiveValidationResponse != null && receiveValidationResponse.Payload.ReadyForCommit);
		}

		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Receive_Complete_LowAmount()
		{
			var sendRequest = new SendRequest
			{
				AgentState = AgentLocation.MN,
				Country = Country.Usa,
				State = State.Ny,
				SendCurr = Currency.Usd,
				AmtRange = AmountRange.UnderOneHundred,
				FeeType = ItemChoiceType1.amountExcludingFee
			};
            sendRequest.PopulateAgentData(sendRequest.AgentState);
            var sendData = new SendData(sendRequest);
			var receiveRequest = new ReceiveRequest
			{
				AgentState = AgentLocation.NY,
				ThirdPartyType = TestThirdPartyType.None
			};
            receiveRequest.PopulateAgentData(receiveRequest.AgentState);

            var receiveData = new ReceiveData(receiveRequest);
			sendData = SendOperations.SendCompleteForNewCustomer(sendData);
			receiveData.Set(sendData);
			receiveData = ReceiveOperations.ReceiveComplete(receiveData);
			var transLookupResponse = TranLookupOperations.TransactionLookupForStatus(receiveRequest.AgentId, receiveRequest.AgentPos, receiveData.CompleteSessionResponse.Payload.ReferenceNumber);

			// ASSERT ALL THE THINGS
			Assert.IsFalse(sendData.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{sendData.Errors?.Log()}");
			Assert.IsFalse(receiveData.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{sendData.Errors?.Log()}");
			Assert.IsTrue(transLookupResponse.TransactionLookupResp.Payload.TransactionStatus == TransactionStatusType.RECVD);
		}

		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Recv_Complete()
		{
			var sendRequest = new SendRequest
			{
				AgentState = AgentLocation.MN,
				Country = Country.Usa,
				State = State.Ny,
				SendCurr = Currency.Usd,
				AmtRange = AmountRange.NoIdsNoThirdParty,
				FeeType = ItemChoiceType1.amountExcludingFee
			};
            sendRequest.PopulateAgentData(sendRequest.AgentState);
            var sendData = new SendData(sendRequest);
			var receiveRequest = new ReceiveRequest
			{
				AgentState = AgentLocation.NY,
				ThirdPartyType = TestThirdPartyType.None
			};
            receiveRequest.PopulateAgentData(receiveRequest.AgentState);

            var receiveData = new ReceiveData(receiveRequest);
            sendData = SendOperations.SendCompleteForNewCustomer(sendData);
			receiveData.Set(sendData);
			receiveData = ReceiveOperations.ReceiveComplete(receiveData);
			var transLookupResponse = TranLookupOperations.TransactionLookupForStatus(receiveRequest.AgentId, receiveRequest.AgentPos, receiveData.CompleteSessionResponse.Payload.ReferenceNumber);

			// ASSERT ALL THE THINGS
			Assert.IsFalse(sendData.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{sendData.Errors?.Log()}");
			Assert.IsFalse(receiveData.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{sendData.Errors?.Log()}");
			Assert.IsTrue(transLookupResponse.TransactionLookupResp.Payload.TransactionStatus == TransactionStatusType.RECVD);
		}

		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Recv_ThirdParty_Person_Complete()
		{
			var sendRequest = new SendRequest
			{
				AgentState = AgentLocation.MN,
				Country = Country.Usa,
				State = State.Ny,
				SendCurr = Currency.Usd,
				AmtRange = AmountRange.ThirdParty,
				FeeType = ItemChoiceType1.amountExcludingFee,
				ServiceOption = ServiceOptionType.WillCall,
				ThirdPartyType = TestThirdPartyType.Person
			};
            sendRequest.PopulateAgentData(sendRequest.AgentState);
            var sendData = new SendData(sendRequest);
			sendData = SendOperations.SendCompleteForThirdParty(sendData);
			var receiveRequest = new ReceiveRequest
			{
				AgentState = AgentLocation.NY,
				ThirdPartyType = TestThirdPartyType.Person
			};
            receiveRequest.PopulateAgentData(receiveRequest.AgentState);

            var receiveData = new ReceiveData(receiveRequest);
            receiveData.Set(sendData);

            receiveData = ReceiveOperations.ReceiveComplete(receiveData);
			var transLookupResponse = TranLookupOperations.TransactionLookupForStatus(receiveRequest.AgentId, receiveRequest.AgentPos, receiveData.SendData.CompleteSessionResp.Payload.ReferenceNumber);

			// ASSERT ALL THE THINGS
			Assert.IsFalse(sendData.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{sendData.Errors?.Log()}");
			Assert.IsFalse(receiveData.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{sendData.Errors?.Log()}");
			Assert.IsTrue(transLookupResponse.TransactionLookupResp.Payload.TransactionStatus == TransactionStatusType.RECVD);
		}
		[TestMethod]
		public void Recv_ThirdParty_Org_Complete()
		{
			var sendRequest = new SendRequest
			{
				AgentState = AgentLocation.MN,
				Country = Country.Usa,
				State = State.Ny,
				SendCurr = Currency.Usd,
				AmtRange = AmountRange.ThirdParty,
				FeeType = ItemChoiceType1.amountExcludingFee,
				ServiceOption = ServiceOptionType.WillCall,
				ThirdPartyType = TestThirdPartyType.Org
			};
            sendRequest.PopulateAgentData(sendRequest.AgentState);

            var sendData = new SendData(sendRequest);
			sendData = SendOperations.SendCompleteForThirdParty(sendData);
			var receiveRequest = new ReceiveRequest
			{
				AgentState = AgentLocation.NY,
				ThirdPartyType = TestThirdPartyType.Org
			};
            receiveRequest.PopulateAgentData(receiveRequest.AgentState);

            var receiveData = new ReceiveData(receiveRequest);
            receiveData.Set(sendData);

            receiveData = ReceiveOperations.ReceiveComplete(receiveData);
			var transLookupResponse = TranLookupOperations.TransactionLookupForStatus(receiveRequest.AgentId, receiveRequest.AgentPos, receiveData.SendData.CompleteSessionResp.Payload.ReferenceNumber);

			// ASSERT ALL THE THINGS
			Assert.IsFalse(sendData.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{sendData.Errors?.Log()}");
			Assert.IsFalse(receiveData.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{sendData.Errors?.Log()}");
			Assert.IsTrue(transLookupResponse.TransactionLookupResp.Payload.TransactionStatus == TransactionStatusType.RECVD);
		}

		[TestMethod]
		public void Recv_StageReceive()
		{
			var sendRequest = new SendRequest
			{
				AgentState = AgentLocation.MN,
				Country = Country.Usa,
				State = State.Ny,
				SendCurr = Currency.Usd,
				AmtRange = AmountRange.NoIdsNoThirdParty,
				FeeType = ItemChoiceType1.amountExcludingFee
			};
            sendRequest.PopulateAgentData(sendRequest.AgentState);
            var sendData = new SendData(sendRequest);
			sendData = SendOperations.SendCompleteForExistingCustomer(sendData);
			var receiveRequest = new ReceiveRequest
			{
				AgentState = AgentLocation.NY,
				ThirdPartyType = TestThirdPartyType.None
			};
            receiveRequest.PopulateAgentData(receiveRequest.AgentState);
            var receiveData = new ReceiveData(receiveRequest);
            receiveData.Set(sendData);

			receiveData = ReceiveOperations.ReceiveCompleteStaged(receiveData);
			//Transaction Lookup for receive completion
			ReceiveOperations.TransactionLookupForReceiveCompletion(receiveData);
			var recvCompletionTransactionLookupResponse = receiveData.TransactionLookupResponse;

			// ASSERT ALL THE THINGS
			Assert.IsFalse(sendData.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{sendData.Errors?.Log()}");
			Assert.IsFalse(receiveData.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{sendData.Errors?.Log()}");
			Assert.IsFalse(recvCompletionTransactionLookupResponse.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{recvCompletionTransactionLookupResponse.Errors?.Log()}");
			Assert.IsTrue(recvCompletionTransactionLookupResponse.Payload.TransactionStatus == TransactionStatusType.AVAIL);
		}
		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Recv_StageReceive_SmallAmount()
		{
			var sendRequest = new SendRequest
			{
				AgentState = AgentLocation.MN,
				Country = Country.Usa,
				State = State.Ny,
				SendCurr = Currency.Usd,
				AmtRange = AmountRange.UnderOneHundred,
				FeeType = ItemChoiceType1.amountExcludingFee
			};
            sendRequest.PopulateAgentData(sendRequest.AgentState);
            var sendData = new SendData(sendRequest);
			sendData = SendOperations.SendCompleteForExistingCustomer(sendData);
			var receiveRequest = new ReceiveRequest
			{
				AgentState = AgentLocation.NY,
				ThirdPartyType = TestThirdPartyType.None
			};
            receiveRequest.PopulateAgentData(receiveRequest.AgentState);

            var receiveData = new ReceiveData(receiveRequest);
            receiveData.Set(sendData);

            receiveData = ReceiveOperations.ReceiveCompleteStaged(receiveData);
			ReceiveOperations.TransactionLookupForReceiveCompletion(receiveData);

			// ASSERT ALL THE THINGS
			Assert.IsFalse(sendData.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{sendData.Errors?.Log()}");
			Assert.IsFalse(receiveData.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{sendData.Errors?.Log()}");
			Assert.IsFalse(receiveData.TransactionLookupResponse.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{receiveData.TransactionLookupResponse.Errors?.Log()}");
			Assert.IsTrue(receiveData.TransactionLookupResponse.Payload.TransactionStatus == TransactionStatusType.AVAIL);
		}

		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void Recv_StageReceive_SmallAmount_NewCustomer()
		{
			var sendRequest = new SendRequest
			{
				AgentState = AgentLocation.MN,
				Country = Country.Usa,
				State = State.Ny,
				SendCurr = Currency.Usd,
				AmtRange = AmountRange.UnderOneHundred,
				FeeType = ItemChoiceType1.amountExcludingFee,
				ThirdPartyType = TestThirdPartyType.None
			};
            sendRequest.PopulateAgentData(sendRequest.AgentState);

            var sendData = new SendData(sendRequest);
			sendData = SendOperations.SendCompleteForNewCustomer(sendData);
			var receiveRequest = new ReceiveRequest()
			{
				AgentState = AgentLocation.NY,
				ThirdPartyType = TestThirdPartyType.None
			};
            receiveRequest.PopulateAgentData(receiveRequest.AgentState);
            var receiveData = new ReceiveData(receiveRequest);
            receiveData.Set(sendData);

            receiveData = ReceiveOperations.ReceiveCompleteStaged(receiveData);
			receiveData = ReceiveOperations.TransactionLookupForReceiveCompletion(receiveData);

			// ASSERT ALL THE THINGS
			Assert.IsFalse(sendData.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{sendData.Errors?.Log()}");
			Assert.IsFalse(receiveData.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{sendData.Errors?.Log()}");
			Assert.IsFalse(receiveData.TransactionLookupResponse.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{receiveData.TransactionLookupResponse.Errors?.Log()}");
			Assert.IsTrue(receiveData.TransactionLookupResponse.Payload.TransactionStatus == TransactionStatusType.AVAIL);
		}
	}
}