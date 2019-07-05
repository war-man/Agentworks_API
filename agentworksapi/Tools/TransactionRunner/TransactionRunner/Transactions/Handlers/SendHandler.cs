using System;
using System.Collections.Generic;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.Requests;
using MoneyGram.AgentConnect.IntegrationTest.Data.Send;
using MoneyGram.AgentConnect.IntegrationTest.Data.Setup;
using MoneyGram.AgentConnect.IntegrationTest.Operations.Send;
using TransactionRunner.Interfaces;
using System.Text;
using MoneyGram.AgentConnect.IntegrationTest.Data;
using TransactionRunner.ViewModels.Static;

namespace TransactionRunner.Transactions.Handlers
{
    /// <summary>
    ///     Handler for SendCompleteForExistingCustomer
    /// </summary>
    public class SendHandler : BaseHandler
    {
        private readonly IAgentSelectorViewModel _agentSelector;
        private readonly IMessageBus _messageBus;
        private readonly ISendParametersViewModel _sendParameters;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="messageBus">Message bus used to publish results</param>
        /// <param name="sendParameters">Send parameter view model needed to create transaction</param>
        /// <param name="agentSelector">agent parameter view model needed to create transaction</param>
        /// <param name="sendOperations">Send operations parameter</param>
        public SendHandler(IMessageBus messageBus, ISendParametersViewModel sendParameters,
            IAgentSelectorViewModel agentSelector, SendOperations sendOperations)
        {
            SendOperations = sendOperations;
            _messageBus = messageBus;
            _sendParameters = sendParameters;
            _agentSelector = agentSelector;
        }

        /// <summary>
        ///     Display name for this transaction
        /// </summary>
        public override string Display { get; } = "Send";

        /// <summary>
        ///     Unique identifier for the type of transaction we're doing.
        /// </summary>
        public override string TransactionName => StaticTransactionNames.Send;

        /// <summary>
        ///     This sessions's type
        /// </summary>
        public override SessionType Type { get; } = SessionType.SEND;

        private SendOperations SendOperations { get; }

		/// <summary>
		///     Creates send parameters based on the viewmodel
		/// </summary>
		/// <returns>Transaction parameters</returns>
		public override BaseParams BuildParams => new SendParameters
		{
		    Environment = _agentSelector?.SelectedEnvironment,
		    AgentCountryIsoCode = _agentSelector?.SelectedAgentLocation.AgentCountryIsoCode,
		    AgentId = _agentSelector?.SelectedAgent.AgentId,
		    AgentPos = _agentSelector?.SelectedAgentPos.AgentSequence,
		    AgentState = _agentSelector?.SelectedAgentLocation?.AgentStateCode,
		    CustomAmount = (_sendParameters?.IsCustomAmountRangeSelected).GetValueOrDefault() ? (_sendParameters?.CustomAmount).GetValueOrDefault() : 0.0M,
		    AmtRange = _sendParameters?.SelectedAmountRange.Code,
		    Country = _sendParameters?.SelectedCountry.CountryCode,
		    State = _sendParameters?.SelectedCountrySubdivision.CountrySubdivisionCode,
		    FeeType = _sendParameters?.SelectedItemChoice.Code,
		    SendCurr = _sendParameters?.SelectedCurrency.CurrencyCode,
		    ServiceOption = _sendParameters?.SelectedServiceOption.Key,
		    ThirdPartyType = _sendParameters?.SelectedThirdPartyType
		};

        /// <summary>
		///     Implements transaction for Send
		/// </summary>
		/// <param name="parameters"></param>
		/// <returns>Transaction result</returns>
		public override ITransactionResult Transaction(object parameters)
        {
            var sendParameters = (SendParameters)parameters;

            TestConfig.TestSettings = new IntegrationTestSettingsModel { AcEnvironment = sendParameters.Environment };

            var amtRangeParsed = Enum.TryParse(sendParameters?.AmtRange, true, out AmountRange amtRange);
            var itemChoiceTypeParsed = Enum.TryParse(sendParameters?.FeeType, true, out ItemChoiceType1 itemChoiceType);
            var sendRequest = new SendRequest
            {
                Environment = sendParameters.Environment,
                AgentCountryIsoCode = sendParameters.AgentCountryIsoCode,
                AgentId = sendParameters.AgentId,
                AgentPos = sendParameters.AgentPos,
                AgentState = sendParameters.GetAgentState(),
                Amount = (double)sendParameters.CustomAmount,
                AmtRange = amtRangeParsed ? amtRange : AmountRange.CustomAmount,
                Country = sendParameters.Country,
                State = sendParameters.State,
                FeeType = itemChoiceTypeParsed ? itemChoiceType : ItemChoiceType1.amountExcludingFee,
                SendCurr = sendParameters.SendCurr,
                ServiceOption = sendParameters.ServiceOption,
                ThirdPartyType = sendParameters.ThirdPartyType
            };
            var sendData = new SendData(sendRequest);
            sendData = SendOperations.SendCompleteForNewCustomer(sendData);

            return new TransactionResult { Result = sendData };
        }

        /// <summary>
        ///     Publishes results to listeners
        /// </summary>
        /// <param name="transactionResult"></param>
        /// <param name="mode"></param>
        /// <param name="guid"></param>
        public override void ResultHandler(ITransactionResult transactionResult, TransactionQueueMode mode, Guid guid)
        {
            var stringBuilder = new StringBuilder();
            var sendData = (SendData)transactionResult.Result;
            var referenceNumber = sendData?.CompleteSessionResp?.Payload?.ReferenceNumber;
            var formattedStringList = sendData.ToFormattedString(new List<string>());
            var timeSpan = (sendData.BeginTimeStamp.HasValue && sendData.EndTimeStamp.HasValue) ? sendData.EndTimeStamp.Value - sendData.BeginTimeStamp.Value : new TimeSpan();

            stringBuilder.AppendLine($"==========BEGIN {Display.ToUpper()}==========");
            stringBuilder.AppendLine($"{referenceNumber}");
            stringBuilder.AppendLine($"{DateTime.Now}");
            stringBuilder.AppendLine((sendData.Errors.Count == 0) ? "COMPLETED SUCCESSFULLY" : "COMPLETED WITH ERRORS");
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