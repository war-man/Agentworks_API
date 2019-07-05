using System.Collections.Generic;
using MoneyGram.AgentConnect.IntegrationTest.Operations;
using MoneyGram.AgentConnect.IntegrationTest.Operations.Amend;
using MoneyGram.AgentConnect.IntegrationTest.Operations.BillPay;
using MoneyGram.AgentConnect.IntegrationTest.Operations.Recv;
using MoneyGram.AgentConnect.IntegrationTest.Operations.Send;
using MoneyGram.AgentConnect.IntegrationTest.Operations.SendReversal;
using TransactionRunner.Interfaces;
using TransactionRunner.Messaging;
using TransactionRunner.Transactions.Handlers;
using TransactionRunner.ViewModels.Static;

namespace TransactionRunner.ConfigProviders
{
    /// <summary>
    ///     Provides available transactions
    /// </summary>
    public class TransactionProvider
    {
        private static List<ITransactionHandler> _transactionHandlers;

        /// <summary>
        ///     Get available handlers
        /// </summary>
        /// <returns>List of available handlers.  New handlers should be added to this list.</returns>
        public static IEnumerable<ITransactionHandler> TransactionHandlers =>
            _transactionHandlers =
                new List<ITransactionHandler>
                {
                    //new AmendHandler(StaticMessageBus.MessageBus,
                    //    StaticSendParametersVm.SendParametersViewModel,
                    //    StaticAgentSelectorVm.AgentSelectorViewModel,
                    //    new AmendOperations(new TestRunner()),
                    //    new SendOperations(new TestRunner())),
                    CreateSendHandler(),
                    CreateSendReversalHandler(),
                    CreateStagedSendHandler(),
                    CreateBillPayHandler(),
                    CreateStagedBillPayHandler(),
                    CreateStagedReceiveHandler(),
                    CreateReceiveHandler()
     //               new ReceiveHandler(
		   //             StaticMessageBus.MessageBus,
		   //             StaticAgentSelectorVm.ReceiveAgentSelectorViewModel,
		   //             new ReceiveOperations(new TestRunner())),
					//new SendReversalHandler(
     //                   StaticMessageBus.MessageBus,
     //                   StaticSendParametersVm.SendParametersViewModel,
     //                   StaticSendReversalParametersVm.SendReversalParametersViewModel,
     //                   StaticAgentSelectorVm.AgentSelectorViewModel,
     //                   new SendReversalOperations(new TestRunner()),
     //                   new SendOperations(new TestRunner()))
                };
        private static ITransactionHandler CreateSendHandler()
        {
            return new SendHandler(
                StaticMessageBus.MessageBus,
                StaticSendParametersVm.SendParametersViewModel,
                StaticAgentSelectorVm.AgentSelectorViewModel,
                new SendOperations(new TestRunner()));
        }
        private static ITransactionHandler CreateStagedSendHandler()
        {
            return new StagedSendHandler(
                StaticMessageBus.MessageBus,
                StaticStagedSendParametersVm.StagedSendParametersViewModel,
                StaticAgentSelectorVm.AgentSelectorViewModel,
                new SendOperations(new TestRunner()));
        }
        private static ITransactionHandler CreateStagedBillPayHandler()
        {
            return new StagedBillPayHandler(StaticMessageBus.MessageBus,
                StaticStagedBillPayVm.StagedBillPayViewModel,
                StaticAgentSelectorVm.AgentSelectorViewModel,
                new BillPayOperations(new TestRunner()));
        }
        private static ITransactionHandler CreateBillPayHandler()
        {
            return new BillPayHandler(StaticMessageBus.MessageBus,
                StaticBillPayVm.BillPayViewModel,
                StaticAgentSelectorVm.AgentSelectorViewModel,
                new BillPayOperations(new TestRunner()));
        }
        private static ITransactionHandler CreateStagedReceiveHandler()
        {
            return new StagedReceiveHandler(StaticMessageBus.MessageBus,
                StaticStagedReceiveVm.StagedReceiveViewModel,
                StaticAgentSelectorVm.AgentSelectorViewModel,
                new SendOperations(new TestRunner()),
                new ReceiveOperations(new TestRunner()));
        }
        private static ITransactionHandler CreateReceiveHandler()
        {
            return new ReceiveHandler(StaticMessageBus.MessageBus,
                StaticReceiveVm.ReceiveViewModel,
                StaticAgentSelectorVm.AgentSelectorViewModel,
                new SendOperations(new TestRunner()),
                new ReceiveOperations(new TestRunner()));
        }
        private static ITransactionHandler CreateSendReversalHandler()
        {
            return new SendReversalHandler(StaticMessageBus.MessageBus,
                StaticSendReversalParametersVm.SendReversalParametersViewModel,
                StaticAgentSelectorVm.AgentSelectorViewModel,
                new SendReversalOperations(new TestRunner()),
                new SendOperations(new TestRunner()));
        }
    }
}