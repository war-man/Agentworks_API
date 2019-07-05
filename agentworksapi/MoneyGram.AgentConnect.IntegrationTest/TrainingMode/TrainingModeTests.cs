using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.BillPay;
using MoneyGram.AgentConnect.IntegrationTest.Data.Extensions;
using MoneyGram.AgentConnect.IntegrationTest.Data.Requests;
using MoneyGram.AgentConnect.IntegrationTest.Data.Send;
using MoneyGram.AgentConnect.IntegrationTest.Data.Setup;
using MoneyGram.AgentConnect.IntegrationTest.Operations;
using MoneyGram.AgentConnect.IntegrationTest.Operations.BillPay;
using MoneyGram.AgentConnect.IntegrationTest.Operations.Send;
using MoneyGram.AgentConnect;

namespace MoneyGram.AgentConnect.IntegrationTest.TrainingMode
{
    [TestClass]
    public class TrainingModeTests
    {
        private SendOperations _sendOperations;
        private BillPayOperations _billPayOperations;
        private TrainingModeRepository _repo;

        private const string GraphicMimeType = "image/jpeg";
        private const string ThermalMimeType = "application/vnd.thermal";

        private readonly IList<string> _receiveReferenceNumbers = new List<string>
        {
            "11111111",
            "22222222",
            "33333333"
        };

        [TestInitialize]
        public void TestSetup()
        {
            _repo = new TrainingModeRepository();
            _sendOperations = new SendOperations(new TestRunner(true));
            _billPayOperations = new BillPayOperations(new TestRunner(true));
        }

        #region Send
        [TestMethod]
        [Ignore] //test dependent on AC and currently failing
        public void TrainingMode_Send_CompleteRequest()
        {
            var sendRequest = new SendRequest
            {
                AgentState = AgentLocation.NY,
                Country = Country.Usa,
                State = State.Ny,
                SendCurr = Currency.Usd,
                AmtRange = AmountRange.NoIdsNoThirdParty,
                FeeType = ItemChoiceType1.amountExcludingFee,
                ServiceOption = ServiceOptionType.WillCall
            };
            sendRequest.PopulateAgentData(sendRequest.AgentState, true);

            var sendData = new SendData(sendRequest);
            sendData = _sendOperations.SendCompleteForExistingCustomer(sendData);

            Assert.IsFalse(sendData.Errors.Any(),
                $" {Environment.NewLine}Errors: {Environment.NewLine}{sendData.Errors?.Log()}");
            Assert.IsTrue(sendData.CompleteSessionResp.Payload != null &&
                          !string.IsNullOrWhiteSpace(sendData.CompleteSessionResp.Payload.ReferenceNumber));
            Assert.AreEqual(sendData.CompleteSessionResp.Payload.ReferenceNumber, "12345678");
        }

        [TestMethod]
        public void TrainingMode_Send_CheckContentOfMockFiles()
        {
            //jpeg receipts
            CompleteSessionResponse result = _repo.MockCompleteSessionResponse(SessionType.SEND, null, false);
            Assert.IsNotNull(result);
            Assert.AreEqual(GraphicMimeType, result.Payload.Receipts.ReceiptMimeType);

            //thermal receipts
            CompleteSessionResponse thermalResult = _repo.MockCompleteSessionResponse(SessionType.SEND, null, true);
            Assert.IsNotNull(thermalResult);
            Assert.AreEqual(ThermalMimeType, thermalResult.Payload.Receipts.ReceiptMimeType);
        }
        #endregion

        #region Bill Pay
        [TestMethod]
        [Ignore] //test dependent on AC and currently failing
        public void TrainingMode_BillPay_CompleteRequest()
        {
            var request = new BillPayOperationRequest
            {
                AgentState = AgentLocation.MN,
                Biller = Billers.HubbardAttorney,
                AmtRange = AmountRange.NoIdsNoThirdParty,
                ThirdPartyType = TestThirdPartyType.None
            };
            request.PopulateAgentData(request.AgentState, true);
            var billPayData = new BillPayData(request);
            _billPayOperations.CompleteSession(billPayData);

            var completeSessionResponse = billPayData.CompleteSessionResponse;

            Assert.IsFalse(string.IsNullOrEmpty(completeSessionResponse.Payload.ReferenceNumber));
        }

        [TestMethod]
        public void TrainingMode_BillPay_CheckContentOfMockFiles()
        {
            CompleteSessionResponse result = _repo.MockCompleteSessionResponse(SessionType.BP, null, false);
            Assert.IsNotNull(result);
            Assert.AreEqual(GraphicMimeType, result.Payload.Receipts.ReceiptMimeType);

            CompleteSessionResponse thermalResult = _repo.MockCompleteSessionResponse(SessionType.BP, null, true);
            Assert.IsNotNull(thermalResult);
            Assert.AreEqual(ThermalMimeType, thermalResult.Payload.Receipts.ReceiptMimeType);
        }
        #endregion

        #region Receive
        [TestMethod]
        public void TrainingMode_Receive_CheckLookupFilesContent()
        {
            foreach (var refNumber in _receiveReferenceNumbers)
            {
                var mockedData = _repo.MockTransactionLookupResponse(refNumber, PurposeOfLookup.Receive);
                Assert.IsNotNull(mockedData);
                Assert.IsInstanceOfType(mockedData, typeof(TransactionLookupResponse));
                Assert.AreEqual(refNumber, mockedData.Payload.MgiSessionID);
                Assert.AreEqual(refNumber, mockedData.Payload.CurrentValues
                    .First(x => x.InfoKey == "referenceNumber")
                    .Value);
                switch (refNumber)
                {
                    case "11111111":
                        Assert.AreEqual(100, mockedData.Payload.ReceiveAmounts.ReceiveAmount);
                        Assert.AreEqual(100, mockedData.Payload.SendAmounts.SendAmount);
                        Assert.AreEqual("USD", mockedData.Payload.SendAmounts.SendCurrency);
                        Assert.AreEqual("JOHN",
                            mockedData.Payload.CurrentValues.First(x => x.InfoKey == "sender_FirstName")
                                .Value);
                        Assert.AreEqual("DOE",
                            mockedData.Payload.CurrentValues.First(x => x.InfoKey == "sender_LastName")
                                .Value);
                        Assert.AreEqual("USA",
                            mockedData.Payload.CurrentValues.First(x => x.InfoKey == "originatingCountry")
                                .Value);
                        Assert.AreEqual("JANE",
                            mockedData.Payload.CurrentValues.First(x => x.InfoKey == "receiver_FirstName")
                                .Value);
                        Assert.AreEqual("SMITH",
                            mockedData.Payload.CurrentValues.First(x => x.InfoKey == "receiver_LastName")
                                .Value);

                        break;
                    case "22222222":
                        Assert.AreEqual(3100, mockedData.Payload.ReceiveAmounts.ReceiveAmount);
                        Assert.AreEqual(3100, mockedData.Payload.SendAmounts.SendAmount);
                        Assert.AreEqual("USD", mockedData.Payload.SendAmounts.SendCurrency);
                        Assert.AreEqual("MARK",
                            mockedData.Payload.CurrentValues.First(x => x.InfoKey == "sender_FirstName")
                                .Value);
                        Assert.AreEqual("SMITH",
                            mockedData.Payload.CurrentValues.First(x => x.InfoKey == "sender_LastName")
                                .Value);
                        Assert.AreEqual("USA",
                            mockedData.Payload.CurrentValues.First(x => x.InfoKey == "originatingCountry")
                                .Value);
                        Assert.AreEqual("PAUL",
                            mockedData.Payload.CurrentValues.First(x => x.InfoKey == "receiver_FirstName")
                                .Value);
                        Assert.AreEqual("JONES",
                            mockedData.Payload.CurrentValues.First(x => x.InfoKey == "receiver_LastName")
                                .Value);
                        break;
                    case "33333333":
                        Assert.AreEqual(500, mockedData.Payload.ReceiveAmounts.ReceiveAmount);
                        Assert.AreEqual(500, mockedData.Payload.SendAmounts.SendAmount);
                        Assert.AreEqual("GBP", mockedData.Payload.SendAmounts.SendCurrency);
                        Assert.AreEqual("MARY",
                            mockedData.Payload.CurrentValues.First(x => x.InfoKey == "sender_FirstName")
                                .Value);
                        Assert.AreEqual("SMYTHE",
                            mockedData.Payload.CurrentValues.First(x => x.InfoKey == "sender_LastName")
                                .Value);
                        Assert.AreEqual("GBR",
                            mockedData.Payload.CurrentValues.First(x => x.InfoKey == "originatingCountry")
                                .Value);
                        Assert.AreEqual("BILL",
                            mockedData.Payload.CurrentValues.First(x => x.InfoKey == "receiver_FirstName")
                                .Value);
                        Assert.AreEqual("MORRIS",
                            mockedData.Payload.CurrentValues.First(x => x.InfoKey == "receiver_LastName")
                                .Value);
                        break;
                    case "44444444":
                        Assert.AreEqual(200, mockedData.Payload.ReceiveAmounts.ReceiveAmount);
                        Assert.AreEqual(200, mockedData.Payload.SendAmounts.SendAmount);
                        Assert.AreEqual("EUR", mockedData.Payload.SendAmounts.SendCurrency);
                        Assert.AreEqual("TIM",
                            mockedData.Payload.CurrentValues.First(x => x.InfoKey == "sender_FirstName")
                                .Value);
                        Assert.AreEqual("MARTIN",
                            mockedData.Payload.CurrentValues.First(x => x.InfoKey == "sender_LastName")
                                .Value);
                        Assert.AreEqual("ESP",
                            mockedData.Payload.CurrentValues.First(x => x.InfoKey == "originatingCountry")
                                .Value);
                        Assert.AreEqual("JOE",
                            mockedData.Payload.CurrentValues.First(x => x.InfoKey == "receiver_FirstName")
                                .Value);
                        Assert.AreEqual("GORDON",
                            mockedData.Payload.CurrentValues.First(x => x.InfoKey == "receiver_LastName")
                                .Value);
                        break;
                    default:
                        throw new NotImplementedException(
                            $"Not implemented test criteria for reference number {refNumber}.");
                }
            }
        }

        [TestMethod]
        public void TrainingMode_Receive_CheckCompleteSessionFilesContent()
        {
            foreach (string mgiSessionId in _receiveReferenceNumbers)
            {
                CompleteSessionResponse result = _repo.MockCompleteSessionResponse(SessionType.RCV, mgiSessionId, false);
                Assert.IsNotNull(result);
                Assert.AreEqual(GraphicMimeType, result.Payload.Receipts.ReceiptMimeType);

                CompleteSessionResponse thermalResult = _repo.MockCompleteSessionResponse(SessionType.RCV, mgiSessionId, true);
                Assert.IsNotNull(thermalResult);
                Assert.AreEqual(ThermalMimeType, thermalResult.Payload.Receipts.ReceiptMimeType);
            }
        }

        [TestMethod]
        public void TrainingMode_Receive_CheckValidationFilesContent()
        {
            foreach (string mgiSessionId in _receiveReferenceNumbers)
            {
                var validation1Result = _repo.MockReceiveValidationResponse(mgiSessionId, new List<KeyValuePairType>(), false); //empty FieldValues
                Assert.IsNotNull(validation1Result);
                Assert.IsFalse(validation1Result.Payload.ReadyForCommit);
                Assert.IsTrue(validation1Result.Payload.FieldsToCollect.Any());
                Assert.IsInstanceOfType(validation1Result.Payload.FieldsToCollect.First(), typeof(CategoryInfo)); //check if type is deserialized correctly

                var validation2Result = _repo.MockReceiveValidationResponse(mgiSessionId, new List<KeyValuePairType> { new KeyValuePairType() }, false); //not empty FieldValues
                Assert.IsNotNull(validation2Result);
                Assert.IsTrue(validation2Result.Payload.ReadyForCommit);
                Assert.IsFalse(validation2Result.Payload.FieldsToCollect.Any());
            }
        }
        #endregion

        #region Staged Send
        private const string StagedSendReferenceNumber = "11111111";

        [TestMethod]
        public void TrainingMode_StagedSend_CheckSearchTransactionsFileContent()
        {
            var result = _repo.MockSearchStagedTransactionsResponse(SessionType.SEND);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Payload.StagedTransactionInfos.Count > 0);
            Assert.IsTrue(result.Payload.StagedTransactionInfos.Any(x => x.ConfirmationNumber == StagedSendReferenceNumber));
        }

        [TestMethod]
        public void TrainingMode_StagedSend_CheckTransactionLookupFileContent()
        {
            var result = _repo.MockTransactionLookupResponse(StagedSendReferenceNumber, PurposeOfLookup.SendCompletion);
            Assert.IsNotNull(result);
            Assert.AreEqual(StagedSendReferenceNumber, result.Payload.MgiSessionID);
        }

        [TestMethod]
        public void TrainingMode_StagedSend_CheckValidationFilesContent()
        {
            var validation1Result = _repo.MockSendValidationResponse(StagedSendReferenceNumber, new List<KeyValuePairType>(), false); //empty FieldValues
            Assert.IsNotNull(validation1Result);
            Assert.IsFalse(validation1Result.Payload.ReadyForCommit);
            Assert.IsTrue(validation1Result.Payload.FieldsToCollect.Any());
            Assert.IsInstanceOfType(validation1Result.Payload.FieldsToCollect.First(), typeof(CategoryInfo)); //check if type is deserialized correctly

            var validation2Result = _repo.MockSendValidationResponse(StagedSendReferenceNumber, new List<KeyValuePairType> { new KeyValuePairType() }, false); //not empty FieldValues
            Assert.IsNotNull(validation2Result);
            Assert.AreEqual(GraphicMimeType, validation2Result.Payload.Receipts.ReceiptMimeType);
            Assert.IsTrue(validation2Result.Payload.ReadyForCommit);
            Assert.IsFalse(validation2Result.Payload.FieldsToCollect.Any());

            var validation2ResultThermal = _repo.MockSendValidationResponse(StagedSendReferenceNumber, new List<KeyValuePairType> { new KeyValuePairType() }, true); //not empty FieldValues
            Assert.IsNotNull(validation2ResultThermal);
            Assert.AreEqual(ThermalMimeType, validation2ResultThermal.Payload.Receipts.ReceiptMimeType);
            Assert.IsTrue(validation2ResultThermal.Payload.ReadyForCommit);
            Assert.IsFalse(validation2ResultThermal.Payload.FieldsToCollect.Any());
        }

        [TestMethod]
        public void TrainingMode_StagedSend_CheckCompleteSessionFilesContent()
        {
            CompleteSessionResponse result = _repo.MockCompleteSessionResponse(SessionType.BP, StagedSendReferenceNumber, false);
            Assert.IsNotNull(result);
            Assert.AreEqual(GraphicMimeType, result.Payload.Receipts.ReceiptMimeType);

            CompleteSessionResponse thermalResult = _repo.MockCompleteSessionResponse(SessionType.BP, StagedSendReferenceNumber, true);
            Assert.IsNotNull(thermalResult);
            Assert.AreEqual(ThermalMimeType, thermalResult.Payload.Receipts.ReceiptMimeType);
        }
        #endregion

        #region Staged Bill Pay
        private const string StagedBillPayReferenceNumber = "11111111";

        [TestMethod]
        public void TrainingMode_StagedBillPay_CheckSearchTransactionsFileContent()
        {
            var result = _repo.MockSearchStagedTransactionsResponse(SessionType.BP);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Payload.StagedTransactionInfos.Count > 0);
            Assert.IsTrue(result.Payload.StagedTransactionInfos.Any(x => x.ConfirmationNumber == StagedBillPayReferenceNumber));
        }

        [TestMethod]
        public void TrainingMode_StagedBillPay_CheckTransactionLookupFileContent()
        {
            var result = _repo.MockTransactionLookupResponse(StagedBillPayReferenceNumber, PurposeOfLookup.BillPayCompletion);
            Assert.IsNotNull(result);
            Assert.AreEqual(StagedBillPayReferenceNumber, result.Payload.MgiSessionID);
        }

        [TestMethod]
        public void TrainingMode_StagedBillPay_CheckValidationFilesContent()
        {
            var validation1Result = _repo.MockBillPayValidationResponse(StagedBillPayReferenceNumber, new List<KeyValuePairType>(), false); //empty FieldValues
            Assert.IsNotNull(validation1Result);
            Assert.IsFalse(validation1Result.Payload.ReadyForCommit);
            Assert.IsTrue(validation1Result.Payload.FieldsToCollect.Any());
            Assert.IsInstanceOfType(validation1Result.Payload.FieldsToCollect.First(), typeof(CategoryInfo)); //check if type is deserialized correctly

            var validation2Result = _repo.MockBillPayValidationResponse(StagedBillPayReferenceNumber, new List<KeyValuePairType> { new KeyValuePairType() }, false); //not empty FieldValues
            Assert.IsNotNull(validation2Result);
            Assert.AreEqual(GraphicMimeType, validation2Result.Payload.Receipts.ReceiptMimeType);
            Assert.IsTrue(validation2Result.Payload.ReadyForCommit);
            Assert.IsFalse(validation2Result.Payload.FieldsToCollect.Any());

            var validation2ResultThermal = _repo.MockBillPayValidationResponse(StagedBillPayReferenceNumber, new List<KeyValuePairType> { new KeyValuePairType() }, true); //not empty FieldValues
            Assert.IsNotNull(validation2ResultThermal);
            Assert.AreEqual(ThermalMimeType, validation2ResultThermal.Payload.Receipts.ReceiptMimeType);
            Assert.IsTrue(validation2ResultThermal.Payload.ReadyForCommit);
            Assert.IsFalse(validation2ResultThermal.Payload.FieldsToCollect.Any());
        }

        [TestMethod]
        public void TrainingMode_StagedBillPay_CheckCompleteSessionFilesContent()
        {
            CompleteSessionResponse result = _repo.MockCompleteSessionResponse(SessionType.BP, StagedBillPayReferenceNumber, false);
            Assert.IsNotNull(result);
            Assert.AreEqual(GraphicMimeType, result.Payload.Receipts.ReceiptMimeType);

            CompleteSessionResponse thermalResult = _repo.MockCompleteSessionResponse(SessionType.BP, StagedBillPayReferenceNumber, true);
            Assert.IsNotNull(thermalResult);
            Assert.AreEqual(ThermalMimeType, thermalResult.Payload.Receipts.ReceiptMimeType);
        }
        #endregion

        #region Staged Receive
        private const string StagedReceiveReferenceNumber = "44444444";

        [TestMethod]
        public void TrainingMode_StagedReceive_CheckTransactionLookupFileContent()
        {
            var result = _repo.MockTransactionLookupResponse(StagedReceiveReferenceNumber, PurposeOfLookup.ReceiveCompletion);
            Assert.IsNotNull(result);
            Assert.AreEqual(StagedReceiveReferenceNumber, result.Payload.MgiSessionID);
        }

        [TestMethod]
        public void TrainingMode_StagedReceive_CheckCompleteSessionFilesContent()
        {
            CompleteSessionResponse result = _repo.MockCompleteSessionResponse(SessionType.RCV, StagedReceiveReferenceNumber, false);
            Assert.IsNotNull(result);
            Assert.AreEqual(GraphicMimeType, result.Payload.Receipts.ReceiptMimeType);

            CompleteSessionResponse thermalResult = _repo.MockCompleteSessionResponse(SessionType.RCV, StagedReceiveReferenceNumber, true);
            Assert.IsNotNull(thermalResult);
            Assert.AreEqual(ThermalMimeType, thermalResult.Payload.Receipts.ReceiptMimeType);
        }

        [TestMethod]
        public void TrainingMode_StagedReceive_CheckValidationFilesContent()
        {
            var validation1Result = _repo.MockReceiveValidationResponse(StagedReceiveReferenceNumber, new List<KeyValuePairType>(), false); //empty FieldValues
            Assert.IsNotNull(validation1Result);
            Assert.IsTrue(validation1Result.Payload.ReadyForCommit);
            Assert.IsFalse(validation1Result.Payload.FieldsToCollect.Any());
        }

        [TestMethod]
        public void TrainingMode_StagedReceiveDt4_CheckValidationFilesContent()
        {
            var validation1Result = _repo.MockReceiveValidationResponse(StagedReceiveReferenceNumber, new List<KeyValuePairType>(), true); //empty FieldValues
            Assert.IsNotNull(validation1Result);
            Assert.IsFalse(validation1Result.Payload.ReadyForCommit);
            Assert.IsTrue(validation1Result.Payload.FieldsToCollect.Any());
            Assert.IsInstanceOfType(validation1Result.Payload.FieldsToCollect.First(), typeof(CategoryInfo)); //check if type is deserialized correctly

            var validation2Result = _repo.MockReceiveValidationResponse(StagedReceiveReferenceNumber, new List<KeyValuePairType> { new KeyValuePairType() }, true); //not empty FieldValues
            Assert.IsNotNull(validation2Result);
            Assert.IsTrue(validation2Result.Payload.ReadyForCommit);
            Assert.IsFalse(validation2Result.Payload.FieldsToCollect.Any());
        }
        #endregion

        #region Amend
        private const string AmendReferenceNumber = "77777777";
        
        [TestMethod]
        public void TrainingMode_Amend_CheckTransactionLookupFileContent()
        {
            var result = _repo.MockTransactionLookupResponse(AmendReferenceNumber, PurposeOfLookup.Amend);
            Assert.IsNotNull(result);
            Assert.AreEqual(AmendReferenceNumber, result.Payload.MgiSessionID);

            var resultStatus = _repo.MockTransactionLookupResponse(AmendReferenceNumber, PurposeOfLookup.Status);
            Assert.IsNotNull(resultStatus);
            Assert.AreEqual(AmendReferenceNumber, resultStatus.Payload.MgiSessionID);
        }

        [TestMethod]
        public void TrainingMode_Amend_CheckCompleteSessionFilesContent()
        {
            CompleteSessionResponse result = _repo.MockCompleteSessionResponse(SessionType.AMD, AmendReferenceNumber, false);
            Assert.IsNotNull(result);
            Assert.AreEqual(GraphicMimeType, result.Payload.Receipts.ReceiptMimeType);
        }

        [TestMethod]
        public void TrainingMode_Amend_CheckValidationFilesContent()
        {
            var validation1Result = _repo.MockAmendValidationResponse(AmendReferenceNumber, new List<KeyValuePairType>()); //empty FieldValues
            Assert.IsNotNull(validation1Result);
            Assert.IsFalse(validation1Result.Payload.ReadyForCommit);
            Assert.IsTrue(validation1Result.Payload.FieldsToCollect.Any());
            Assert.IsInstanceOfType(validation1Result.Payload.FieldsToCollect.First(), typeof(CategoryInfo)); //check if type is deserialized correctly

            var validation2Result = _repo.MockAmendValidationResponse(AmendReferenceNumber, new List<KeyValuePairType> { new KeyValuePairType() }); //not empty FieldValues
            Assert.IsNotNull(validation2Result);
            Assert.IsTrue(validation2Result.Payload.ReadyForCommit);
        }
        #endregion

        #region Send Reversal
        private const string SendReversalFeeAutoReferenceNumber = "55555555";
        private const string SendReversalFeeChoiceReferenceNumber = "66666666";
        private const string FeeElementKey = "feeMustBeRefundedFlag";

        [TestMethod]
        public void TrainingMode_SendReversal_CheckTransactionLookupFileContent()
        {
            var result1 = _repo.MockTransactionLookupResponse(SendReversalFeeAutoReferenceNumber, PurposeOfLookup.SendReversal);
            Assert.IsNotNull(result1);
            Assert.AreEqual(SendReversalFeeAutoReferenceNumber, result1.Payload.MgiSessionID);
            var feeElement1 = result1.Payload.CurrentValues.FirstOrDefault(x => x.InfoKey == FeeElementKey);
            Assert.IsNotNull(feeElement1);
            Assert.AreEqual("true", feeElement1.Value);

            var result1Status = _repo.MockTransactionLookupResponse(SendReversalFeeAutoReferenceNumber, PurposeOfLookup.Status);
            Assert.IsNotNull(result1Status);
            Assert.AreEqual(SendReversalFeeAutoReferenceNumber, result1Status.Payload.MgiSessionID);


            var result2 = _repo.MockTransactionLookupResponse(SendReversalFeeChoiceReferenceNumber, PurposeOfLookup.SendReversal);
            Assert.IsNotNull(result2);
            Assert.AreEqual(SendReversalFeeChoiceReferenceNumber, result2.Payload.MgiSessionID);
            var feeElement2 = result2.Payload.CurrentValues.FirstOrDefault(x => x.InfoKey == FeeElementKey);
            Assert.IsNotNull(feeElement2);
            Assert.AreEqual("false", feeElement2.Value);

            var result2Status = _repo.MockTransactionLookupResponse(SendReversalFeeChoiceReferenceNumber, PurposeOfLookup.Status);
            Assert.IsNotNull(result2Status);
            Assert.AreEqual(SendReversalFeeChoiceReferenceNumber, result2Status.Payload.MgiSessionID);
        }

        [TestMethod]
        public void TrainingMode_SendReversal_CheckCompleteSessionFilesContent()
        {
            CompleteSessionResponse result1 = _repo.MockCompleteSessionResponse(SessionType.SREV, SendReversalFeeAutoReferenceNumber, false);
            Assert.IsNotNull(result1);
            Assert.AreEqual(GraphicMimeType, result1.Payload.Receipts.ReceiptMimeType);

            CompleteSessionResponse result2 = _repo.MockCompleteSessionResponse(SessionType.SREV, SendReversalFeeChoiceReferenceNumber, false);
            Assert.IsNotNull(result2);
            Assert.AreEqual(GraphicMimeType, result2.Payload.Receipts.ReceiptMimeType);
        }

        [TestMethod]
        public void TrainingMode_SendReversal_CheckValidationFilesContent()
        {
            var validation1Result1 = _repo.MockSendReversalValidationResponse(SendReversalFeeAutoReferenceNumber, new List<KeyValuePairType>()); //empty FieldValues
            Assert.IsNotNull(validation1Result1);
            Assert.IsFalse(validation1Result1.Payload.ReadyForCommit);
            Assert.IsTrue(validation1Result1.Payload.FieldsToCollect.Any());
            Assert.IsInstanceOfType(validation1Result1.Payload.FieldsToCollect.First(), typeof(CategoryInfo)); //check if type is deserialized correctly

            var validation2Result1 = _repo.MockSendReversalValidationResponse(SendReversalFeeAutoReferenceNumber, new List<KeyValuePairType> { new KeyValuePairType() }); //not empty FieldValues
            Assert.IsNotNull(validation2Result1);
            Assert.IsTrue(validation2Result1.Payload.ReadyForCommit);


            var validation1Result2 = _repo.MockSendReversalValidationResponse(SendReversalFeeChoiceReferenceNumber, new List<KeyValuePairType>()); //empty FieldValues
            Assert.IsNotNull(validation1Result2);
            Assert.IsFalse(validation1Result2.Payload.ReadyForCommit);
            Assert.IsTrue(validation1Result2.Payload.FieldsToCollect.Any());
            Assert.IsInstanceOfType(validation1Result2.Payload.FieldsToCollect.First(), typeof(CategoryInfo)); //check if type is deserialized correctly

            var validation2Result2 = _repo.MockSendReversalValidationResponse(SendReversalFeeChoiceReferenceNumber, new List<KeyValuePairType> { new KeyValuePairType() }); //not empty FieldValues
            Assert.IsNotNull(validation2Result2);
            Assert.IsTrue(validation2Result2.Payload.ReadyForCommit);
        }

        #endregion
    }
}