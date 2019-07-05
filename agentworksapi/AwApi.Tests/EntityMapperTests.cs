using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyGram.DLS.DomainModel;
using MoneyGram.DLS.DomainModel.Request;
using MoneyGram.DLS;
using MoneyGram.DLS.EntityMapper;

namespace AwApi.Tests
{
    [TestClass]
    public class EntityMapperTests
    {

        [TestMethod]
        public void ValidateAutoMapperConfigurationDls()
        {
            DLSMapper.Configure();
        }

        [TestMethod]
        public void ValidateDailyTransactionDetailLookup()
        {
            DLSMapper.Configure();
            var request = new DailyTransactionDetailLookupRequest()
            {
                AgentId = "40698809",
                PosId = "",
                StartDate = "09/27/2017"
            };

            var header = new Header();
            var processingInstruction = new ProcessingInstruction
            {
                Action = "DailyTransactionDetailLookup",
                RollbackTransaction = false
            };


            header.ProcessingInstruction = processingInstruction;
            request.header = header;

            request.header = header;

            var config = new DLSConfig();
            var proxy = new DLSProxyFactory(config);
            var repo = new DLSRepository(proxy);
            var x = repo.DailyTransactionDetailLookup(false, request);
        }
    }
}