using Moq;
using SERVICE = MoneyGram.AgentConnect.Service;

namespace MoneyGram.AgentConnect.IntegrationTest
{
    /// <summary>
    /// Implements a mock for calls to the AgentConnect service.
    /// </summary>
    public class AgentConnectProxyFactoryMock : IAgentConnectProxyFactory
    {
        /// <summary>
        /// Defines the mocked behavior of AgentConnect
        /// </summary>
        /// <returns>The WCF proxy factory that will returned a Moq IAgentConnect implementation</returns>
        public SERVICE.AgentConnect CreateProxy()
        {
            var acService = new Mock<SERVICE.AgentConnect>(); // AgentConnect IS an interface - yes the codegen name doesn't start with "I"

//            acService.Setup(service => service.moneyGramConsumerLookup(
//                It.IsAny<SERVICE.moneyGramConsumerLookupRequest1>()))  // WHAT IS THIS ?????
//                .Returns<SERVICE.moneyGramConsumerLookupRequest1>
//                    ((consumerlkprequest) =>
//                    {
//                        logger.Debug("AC CALL IS MOCKED!!!");
//
//                        var response = new SERVICE.moneyGramConsumerLookupResponse1();
//                        response.moneyGramConsumerLookupResponse = new SERVICE.MoneyGramConsumerLookupResponse();
//                        response.moneyGramConsumerLookupResponse.senderInfo = new SERVICE.SenderLookupInfo[] { };
//
//                        return response;
//                    });

            return acService.Object;
        }
    }
}