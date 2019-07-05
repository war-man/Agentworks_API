using System;
using System.Collections.Generic;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.Amend;
using MoneyGram.AgentConnect.IntegrationTest.Data.Requests;
using MoneyGram.AgentConnect.IntegrationTest.Data.Send;
using MoneyGram.AgentConnect.IntegrationTest.Data.Setup;
using MoneyGram.AgentConnect.IntegrationTest.Operations.Amend;
using MoneyGram.AgentConnect.IntegrationTest.Operations.Send;
using TransactionRunner.Interfaces;
using System.Text;
using MoneyGram.AgentConnect.IntegrationTest.Data;
using TransactionRunner.ViewModels.Static;
using MoneyGram.AgentConnect.IntegrationTest.Data.Extensions;

namespace TransactionRunner.Transactions.Handlers
{
    /// <summary>
    ///     Handler for Amend transactions.
    /// </summary>
    public class AmendHandler : BaseHandler
    {
        private readonly IAgentSelectorViewModel _agentSelector;
        private readonly IMessageBus _messageBus;
        private readonly ISendParametersViewModel _sendParameters;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="messageBus">used to publish results</param>
        /// <param name="sendParameters"></param>
        /// <param name="agentSelector"></param>
        /// <param name="amendOperations"></param>
        /// <param name="sendOperations"></param>
        public AmendHandler(IMessageBus messageBus, ISendParametersViewModel sendParameters,
            IAgentSelectorViewModel agentSelector, AmendOperations amendOperations, SendOperations sendOperations)
        {
            AmendOperations = amendOperations;
            SendOperations = sendOperations;
            _messageBus = messageBus;
            _agentSelector = agentSelector;
            _sendParameters = sendParameters;
        }

        /// <summary>
        ///     Display name for this transaction
        /// </summary>
        public override string Display => "Amend";

        public override string TransactionName => StaticTransactionNames.Amend;

        /// <summary>
        ///     This transaction's type
        /// </summary>
        public override SessionType Type { get; } = SessionType.AMD;

        private AmendOperations AmendOperations { get; }

        private SendOperations SendOperations { get; }

		/// <summary>
		///     Implement this to instantiate parameters.  To be called when Transaction is added to queue
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



				var amount = 0.0M; // TODO: Generate random amount value
				var country = _sendParameters?.SelectedCountry.CountryCode;
				var state = _sendParameters?.SelectedCountrySubdivision.CountrySubdivisionCode;
				var sendCurr = _sendParameters?.SelectedCurrency.CurrencyCode;
				var serviceOpt = _sendParameters?.SelectedServiceOption.Key;

				var thirdPartyType = TestThirdPartyType.None; // TODO: Generate random value

				return new SendParameters
				{
					Environment = environment,
					AgentCountryIsoCode = agentCountry,
					AgentId = agentId,
					AgentPos = agentPos,
					AgentState = agentState,
					CustomAmount = amount,
					AmtRange = _sendParameters?.SelectedAmountRange.Code,
					Country = country,
					State = state,
					FeeType = _sendParameters?.SelectedItemChoice?.Code,
					SendCurr = sendCurr,
					ServiceOption = serviceOpt,
					ThirdPartyType = thirdPartyType
				};
			}
		}

		/// <summary>
		///     Run this transaction
		/// </summary>
		/// <param name="parameters"></param>
		/// <returns></returns>
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
            sendData = SendOperations.SendCompleteForExistingCustomer(sendData);

            var referenceNumber = sendData?.CompleteSessionResp?.Payload?.ReferenceNumber;

            var amendRequest = new AmendOperationRequest
            {
                AgentState = sendParameters.AgentState,
                ReferenceNumber = referenceNumber
            };
            amendRequest.PopulateAgentData(sendParameters.AgentState);
            var amendData = new AmendData(amendRequest);
            amendData.Set(sendData);
            AmendOperations.AmendComplete(amendData);

            return new TransactionResult { Result = amendData };
        }

        /// <summary>
        ///     Implement this to handle the results of the transaction
        /// </summary>
        /// <param name="transactionResult"></param>
        /// <param name="mode"></param>
        /// <param name="guid"></param>
        public override void ResultHandler(ITransactionResult transactionResult, TransactionQueueMode mode, Guid guid)
        {
            var stringBuilder = new StringBuilder();
            var amendData = (AmendData)transactionResult.Result;
            var referenceNumber = amendData?.CompleteSessionResp?.Payload?.ReferenceNumber;
            var formattedStringList = amendData.ToFormattedString(new List<string>());
            var timeSpan = (amendData.BeginTimeStamp.HasValue && amendData.EndTimeStamp.HasValue) ? amendData.EndTimeStamp.Value - amendData.BeginTimeStamp.Value : new TimeSpan();

            stringBuilder.AppendLine($"==========BEGIN {Display.ToUpper()}==========");
            stringBuilder.AppendLine($"{referenceNumber}");
            stringBuilder.AppendLine($"{DateTime.Now}");
            stringBuilder.AppendLine((amendData.Errors.Count == 0) ? "COMPLETED SUCCESSFULLY" : "COMPLETED WITH ERRORS");
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