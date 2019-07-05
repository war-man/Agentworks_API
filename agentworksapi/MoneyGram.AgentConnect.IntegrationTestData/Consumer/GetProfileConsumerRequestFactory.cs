using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.Consumer
{
    public static class GetProfileConsumerRequestFactory
    {
        public static GetProfileConsumerRequest NewGetProfileConsumerRequest()
        {
            return new GetProfileConsumerRequest();
        }
    }
}
