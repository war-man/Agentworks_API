using System;
using System.Collections.Generic;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.Requests;
using MoneyGram.AgentConnect.IntegrationTest.Data.Send;
using MoneyGram.AgentConnect.IntegrationTest.Data.Recv;
using MoneyGram.AgentConnect.IntegrationTest.Data.Setup;
using MoneyGram.AgentConnect.IntegrationTest.Operations.Send;
using MoneyGram.AgentConnect.IntegrationTest.Operations.Recv;
using TransactionRunner.Interfaces;
using System.Text;
using MoneyGram.AgentConnect.IntegrationTest.Data;
using TransactionRunner.ViewModels.Static;

namespace TransactionRunner.Transactions.Handlers
{
    public class ReceiveHandler : BaseHandler
    {
        private readonly IAgentSelectorViewModel _agentSelector;
        private readonly IMessageBus _messageBus;
        private readonly IReceiveViewModel _receiveViewModel;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="messageBus">Message bus used to publish results</param>
        /// <param name="receiveViewModel">Send parameter view model needed to create transaction</param>
        /// <param name="agentSelector">agent parameter view model needed to create transaction</param>
        /// <param name="sendOperations">Send operations parameter</param>
        public ReceiveHandler(IMessageBus messageBus, IReceiveViewModel receiveViewModel,
            IAgentSelectorViewModel agentSelector, SendOperations sendOperations, ReceiveOperations receiveOperations)
        {
            SendOperations = sendOperations;
            ReceiveOperations = receiveOperations;
            _messageBus = messageBus;
            _receiveViewModel = receiveViewModel;
            _agentSelector = agentSelector;
        }

        /// <summary>
        ///     Display name for this transaction
        /// </summary>
        public override string Display { get; } = "Receive";

        /// <summary>
        ///     Unique identifier for the type of transaction we're doing.
        /// </summary>
        public override string TransactionName => StaticTransactionNames.Receive;
        /// <summary>
        ///     This session's type
        /// </summary>
        public override SessionType Type { get; } = SessionType.RCV;

        private SendOperations SendOperations { get; }

        private ReceiveOperations ReceiveOperations { get; }

        /// <summary>
        ///     Creates send parameters based on the viewmodel
        /// </summary>
        /// <returns>Transaction parameters</returns>
        public override BaseParams BuildParams => new ReceiveParameters
        {
            Environment = _agentSelector?.SelectedEnvironment,
            AgentCountryIsoCode = _agentSelector?.SelectedAgentLocation.AgentCountryIsoCode,
            AgentId = _agentSelector?.SelectedAgent.AgentId,
            AgentPos = _agentSelector?.SelectedAgentPos.AgentSequence,
            AgentState = _agentSelector?.SelectedAgentLocation.AgentStateCode,
            CustomAmount = (_receiveViewModel?.IsCustomAmountRangeSelected).GetValueOrDefault() ? (_receiveViewModel?.CustomAmount).GetValueOrDefault() : 0.0M,
            AmtRange = _receiveViewModel?.SelectedAmountRange?.Code,
            Country = _receiveViewModel?.SelectedCountry.CountryCode,
            State = _receiveViewModel?.SelectedCountrySubdivision.CountrySubdivisionCode,
            FeeType = _receiveViewModel?.SelectedItemChoice?.Code,
            SendCurr = _receiveViewModel?.SelectedCurrency.CurrencyCode,
            ServiceOption = _receiveViewModel?.SelectedServiceOption.Key,
            ThirdPartyType = _receiveViewModel?.SelectedThirdPartyType
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
            var receiveAgentState = sendParameters.State.Split('-')[1];
            var receiveRequest = new ReceiveRequest
            {
                AgentState = receiveAgentState,
                ThirdPartyType = sendParameters.ThirdPartyType
            };

            var sendData = new SendData(sendRequest);
            sendData = SendOperations.SendCompleteForNewCustomer(sendData);

            var receiveData = new ReceiveData(receiveRequest);
            receiveData.Set(sendData);
            receiveData = ReceiveOperations.ReceiveComplete(receiveData);

            return new TransactionResult { Result = receiveData };
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
            var receiveData = (ReceiveData)transactionResult.Result;
            var confirmationNumber = receiveData?.CompleteSessionResponse?.Payload?.ReferenceNumber;
            var formattedStringList = receiveData.ToFormattedString(new List<string>());
            var timeSpan = (receiveData.BeginTimeStamp.HasValue && receiveData.EndTimeStamp.HasValue) ? receiveData.EndTimeStamp.Value - receiveData.BeginTimeStamp.Value : new TimeSpan();

            stringBuilder.AppendLine($"==========BEGIN {Display.ToUpper()}==========");
            stringBuilder.AppendLine($"{confirmationNumber}");
            stringBuilder.AppendLine($"{DateTime.Now}");
            stringBuilder.AppendLine((receiveData.Errors.Count == 0) ? "COMPLETED SUCCESSFULLY" : "COMPLETED WITH ERRORS");
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
                ReferenceNumber = confirmationNumber
            });
        }
    }
}