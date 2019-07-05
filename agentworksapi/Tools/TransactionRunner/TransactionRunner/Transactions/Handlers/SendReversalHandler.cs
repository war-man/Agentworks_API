using System;
using System.Collections.Generic;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.Requests;
using MoneyGram.AgentConnect.IntegrationTest.Data.SendReversal;
using MoneyGram.AgentConnect.IntegrationTest.Operations.SendReversal;
using TransactionRunner.Interfaces;
using System.Text;
using MoneyGram.AgentConnect.IntegrationTest.Data;
using TransactionRunner.ViewModels.Static;
using MoneyGram.AgentConnect.IntegrationTest.Operations.Send;
using MoneyGram.AgentConnect.IntegrationTest.Data.Setup;
using SendData = MoneyGram.AgentConnect.IntegrationTest.Data.Send.SendData;
namespace TransactionRunner.Transactions.Handlers
{
    /// <summary>
    ///     Handler for Send Reversal transactions.
    /// </summary>
    public class SendReversalHandler : BaseHandler
    {
        private readonly IAgentSelectorViewModel _agentSelector;
        private readonly IMessageBus _messageBus;
        private readonly ISendReversalParametersViewModel _sendReversalParameters;

	    /// <summary>
	    ///     Constructor.
	    /// </summary>
	    /// <param name="messageBus"></param>
	    /// <param name="sendParameters"></param>
	    /// <param name="sendReversalParameters"></param>
	    /// <param name="agentSelector"></param>
	    /// <param name="sendReversalOperations"></param>
	    /// <param name="sendOperations"></param>
	    public SendReversalHandler(IMessageBus messageBus, ISendReversalParametersViewModel sendReversalParameters,
            IAgentSelectorViewModel agentSelector, SendReversalOperations sendReversalOperations, SendOperations sendOperations)
        {
            SendOperations = sendOperations;
            SendReversalOperations = sendReversalOperations;
            _messageBus = messageBus;
            _agentSelector = agentSelector;
            _sendReversalParameters = sendReversalParameters;
        }

        /// <summary>
        ///     Display name for this transaction.
        /// </summary>
        public override string Display => "Send Reversal";

        /// <summary>
        ///     Unique identifier for the type of transaction we're doing.
        /// </summary>
        public override string TransactionName => StaticTransactionNames.SendReversal;

        /// <summary>
        ///     This session's type.
        /// </summary>
        public override SessionType Type { get; } = SessionType.SREV;

        private SendReversalOperations SendReversalOperations { get; }
        private SendOperations SendOperations { get; }

        /// <summary>
        ///     Implement this to instantiate parameters.  To be called when Transaction is added to queue.
        /// </summary>
        /// <returns>Parameters for this transaction.</returns>
        public override BaseParams BuildParams
		{
			get
			{
				var environment = _agentSelector?.SelectedEnvironment;
				var agentCountry = _agentSelector?.SelectedAgentLocation.AgentCountryIsoCode;
				var agentId = _agentSelector?.SelectedAgent.AgentId;
				var agentPos = _agentSelector?.SelectedAgentPos.AgentSequence;
				var agentState = _agentSelector?.SelectedAgentLocation.AgentStateCode;

				var refundReason = _sendReversalParameters?.SelectedRefundReason;
				var refundFee = _sendReversalParameters?.RefundFee ?? false;

                return new SendReversalParameters
				{
					Environment = environment,
					AgentCountryIsoCode = agentCountry,
					AgentId = agentId,
					AgentPos = agentPos,
					AgentState = agentState,
					RefundReason = refundReason?.Identifier,
					RefundFee = refundFee,
				    CustomAmount = (_sendReversalParameters?.IsCustomAmountRangeSelected).GetValueOrDefault() ? (_sendReversalParameters?.CustomAmount).GetValueOrDefault() : 0.0M,
				    AmtRange = _sendReversalParameters?.SelectedAmountRange?.Code,
                    Country = _sendReversalParameters?.SelectedCountry.CountryCode,
                    State = _sendReversalParameters?.SelectedCountrySubdivision.CountrySubdivisionCode,
                    FeeType = _sendReversalParameters?.SelectedItemChoice?.Code,
                    SendCurr = _sendReversalParameters?.SelectedCurrency.CurrencyCode,
                    ServiceOption = _sendReversalParameters?.SelectedServiceOption.Key,
                    ThirdPartyType = _sendReversalParameters?.SelectedThirdPartyType
                };
			}
		}

		/// <summary>
		///     Run this transaction.
		/// </summary>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public override ITransactionResult Transaction(object parameters)
        {
            var sendReversalParameters = (SendReversalParameters)parameters;

            TestConfig.TestSettings = new IntegrationTestSettingsModel { AcEnvironment = sendReversalParameters.Environment };

            var amtRangeParsed = Enum.TryParse(sendReversalParameters.AmtRange, true, out AmountRange amtRange);
            var itemChoiceTypeParsed = Enum.TryParse(sendReversalParameters.FeeType, true, out ItemChoiceType1 itemChoiceType);
            var sendRequest = new SendRequest
            {
                Environment = sendReversalParameters.Environment,
                AgentCountryIsoCode = sendReversalParameters.AgentCountryIsoCode,
                AgentId = sendReversalParameters.AgentId,
                AgentPos = sendReversalParameters.AgentPos,
                AgentState = sendReversalParameters.GetAgentState(),
                Amount = (double)sendReversalParameters.CustomAmount,
                AmtRange = amtRangeParsed ? amtRange : AmountRange.CustomAmount,
                Country = sendReversalParameters.Country,
                State = sendReversalParameters.State,
                FeeType = itemChoiceTypeParsed ? itemChoiceType : ItemChoiceType1.amountExcludingFee,
                SendCurr = sendReversalParameters.SendCurr,
                ServiceOption = sendReversalParameters.ServiceOption,
                ThirdPartyType = sendReversalParameters.ThirdPartyType
            };

            var sendData = new SendData(sendRequest);
            sendData = SendOperations.SendCompleteForNewCustomer(sendData);

            var sendReversalRequest = new SendReversalRequest
            {
                AgentId = sendReversalParameters.AgentId,
                AgentPos = sendReversalParameters.AgentPos,
                AgentCountryIsoCode = sendReversalParameters.AgentCountryIsoCode,
                AgentState = sendReversalParameters.AgentState,
                ReferenceNumber = sendData.CompleteSessionResp.Payload.ReferenceNumber,
                RefundReason = new EnumeratedIdentifierInfo { Identifier = sendReversalParameters.RefundReason },
                RefundFee = sendReversalParameters.RefundFee
            };

            var sendReversalData = new SendReversalData(sendReversalRequest);
            sendReversalData = SendReversalOperations.SendReversalComplete(sendReversalData);

            return new TransactionResult { Result = sendReversalData };
        }

        /// <summary>
        ///     Implement this to handle the results of the transaction.
        /// </summary>
        /// <param name="transactionResult"></param>
        /// <param name="mode"></param>
        /// <param name="guid"></param>
        public override void ResultHandler(ITransactionResult transactionResult, TransactionQueueMode mode, Guid guid)
        {
            var stringBuilder = new StringBuilder();
            var sendReversalData = (SendReversalData)transactionResult.Result;
            var referenceNumber = sendReversalData?.CompleteSessionResp?.Payload?.ReferenceNumber;
            var formattedStringList = sendReversalData.ToFormattedString(new List<string>());
            var timeSpan = (sendReversalData.BeginTimeStamp.HasValue && sendReversalData.EndTimeStamp.HasValue) ? sendReversalData.EndTimeStamp.Value - sendReversalData.BeginTimeStamp.Value : new TimeSpan();

            stringBuilder.AppendLine($"==========BEGIN {Display.ToUpper()}==========");
            stringBuilder.AppendLine($"{referenceNumber}");
            stringBuilder.AppendLine($"{DateTime.Now}");
            stringBuilder.AppendLine((sendReversalData.Errors.Count == 0) ? "COMPLETED SUCCESSFULLY" : "COMPLETED WITH ERRORS");
            stringBuilder.AppendLine($"FINISHED IN {timeSpan.Minutes}MIN {timeSpan.Seconds}SEC {timeSpan.Milliseconds}MS");
            stringBuilder.AppendLine("==========\n");

            foreach (var formattedString in formattedStringList)
            {
                stringBuilder.Append(formattedString);
            }

            stringBuilder.AppendLine($"==========END {Display.ToUpper()}==========");

            _messageBus.Publish(new TransactionResultsReceivedMessage(mode, guid)
            {
                Result = transactionResult,
                DetailsString = stringBuilder.ToString(),
                ReferenceNumber = referenceNumber
            });
        }
    }
}