using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.BillPay;
using MoneyGram.AgentConnect.IntegrationTest.Data.Extensions;
using MoneyGram.AgentConnect.IntegrationTest.Data.Requests;
using MoneyGram.AgentConnect.IntegrationTest.Data.Setup;
using MoneyGram.AgentConnect.IntegrationTest.Operations.BillPay;
using System;
using System.Collections.Generic;
using System.Text;
using MoneyGram.AgentConnect.IntegrationTest.Data;
using TransactionRunner.Interfaces;
using TransactionRunner.ViewModels.Static;

namespace TransactionRunner.Transactions.Handlers
{
	/// <summary>
	/// Creates a BillPayHandler object
	/// </summary>
	public class StagedBillPayHandler : BaseHandler
	{
		private IMessageBus _messageBus;
        private readonly IAgentSelectorViewModel _agentSelector;
        private BillPayOperations _billPayOperations;
        private readonly IStagedBillPayViewModel _stagedBillPay;

        public StagedBillPayHandler(IMessageBus messageBus, IStagedBillPayViewModel stagedBillPayViewModel, IAgentSelectorViewModel agentSelector, BillPayOperations billPayOperations)
		{
			_messageBus = messageBus;
			_billPayOperations = billPayOperations;
            _stagedBillPay = stagedBillPayViewModel;
            _agentSelector = agentSelector;

        }
		public override string Display { get; } = "Staged Bill Pay";

        /// <summary>
        ///     Unique identifier for the type of transaction we're doing.
        /// </summary>
        public override string TransactionName => StaticTransactionNames.StagedBillPay;
        public override SessionType Type { get; } = SessionType.BP;

	    /// <summary>
	    ///     Creates staged billpay parameters based on the viewmodel
	    /// </summary>
	    /// <returns>Transaction parameters</returns>
	    public override BaseParams BuildParams
	    {
	        get
	        {
	            TestBiller biller = _stagedBillPay.SelectedBiller.Name != "Manual Entry"
	                ? _stagedBillPay.SelectedBiller
	                : new TestBiller
	                {
	                    Code = _stagedBillPay.ManualBillerCode,
	                    ValidAccountNumber = _stagedBillPay.ManualBillerAccountNumber
	                };

                return new StagedBillPayParameters
                {
	                Environment = _agentSelector?.SelectedEnvironment,
	                AgentCountryIsoCode = _agentSelector?.SelectedAgentLocation.AgentCountryIsoCode,
	                AgentId = _agentSelector?.SelectedAgent.AgentId,
	                AgentPos = _agentSelector?.SelectedAgentPos.AgentSequence,
	                AgentState = _agentSelector?.SelectedAgentLocation.AgentStateCode,
	                CustomAmount = (_stagedBillPay?.IsCustomAmountRangeSelected).GetValueOrDefault() ? (_stagedBillPay?.CustomAmount).GetValueOrDefault() : 0.0M,
	                AmtRange = _stagedBillPay?.SelectedAmountRange?.Code,
	                BillerCode = biller.Code,
                    BillerAccountNumber = biller.ValidAccountNumber,
	                ThirdPartyType = _stagedBillPay.SelectedThirdPartyType
	            };
	        }
	    }

	    public override ITransactionResult Transaction(object parameters)
        {
            var billPayParams = (BillPayParameters)parameters;

            TestConfig.TestSettings = new IntegrationTestSettingsModel { AcEnvironment = billPayParams.Environment };

            var amtRangeParsed = Enum.TryParse(billPayParams.AmtRange, true, out AmountRange amtRange);
            var billPayRequest = new BillPayOperationRequest
            {
                Biller = new TestBiller { Code = billPayParams?.BillerCode, ValidAccountNumber = billPayParams?.BillerAccountNumber },
                AmtRange = amtRangeParsed ? amtRange : AmountRange.CustomAmount,
                Amount = (double)billPayParams.CustomAmount,
                AgentState = billPayParams.GetAgentState(),
                ThirdPartyType = billPayParams.ThirdPartyType,
                AgentId = billPayParams.AgentId,
                AgentPos = billPayParams.AgentPos
            };
            billPayRequest.PopulateAgentData(billPayParams.AgentState);
            var billPayData = new BillPayData(billPayRequest);
            _billPayOperations.CompleteStagedSession(billPayData);

            return new TransactionResult { Result = billPayData };
        }

        public override void ResultHandler(ITransactionResult transactionResult, TransactionQueueMode mode, Guid guid)
        {
            var stringBuilder = new StringBuilder();
            var billPayData = (BillPayData)transactionResult.Result;
            var confirmationNumber = billPayData?.CompleteSessionResponse?.Payload?.ConfirmationNumber;
            var formattedStringList = billPayData?.ToFormattedString(new List<string>());
            var timeSpan = billPayData?.BeginTimeStamp != null && billPayData.EndTimeStamp.HasValue ? billPayData.EndTimeStamp.Value - billPayData.BeginTimeStamp.Value : new TimeSpan();

            stringBuilder.AppendLine($"==========BEGIN {Display.ToUpper()}==========");
            stringBuilder.AppendLine($"{confirmationNumber}");
            stringBuilder.AppendLine($"{DateTime.Now}");
            stringBuilder.AppendLine(billPayData != null && (billPayData.Errors.Count == 0) ? "COMPLETED SUCCESSFULLY" : "COMPLETED WITH ERRORS");
            stringBuilder.AppendLine($"FINISHED IN {timeSpan.Minutes}MIN {timeSpan.Seconds}SEC {timeSpan.Milliseconds}MS");
            stringBuilder.AppendLine("==========\n");

            if (formattedStringList != null)
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