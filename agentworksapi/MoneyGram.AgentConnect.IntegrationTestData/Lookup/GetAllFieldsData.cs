using System.Collections.Generic;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.Lookup
{
    public class GetAllFieldsData
    {
        public GetAllFieldsRequest GetAllFieldsReq { get; set; }
        public GetAllFieldsResponse GetAllFieldsResp { get; set; }
        public List<string> InfoKeys { get; set; }
    }
}