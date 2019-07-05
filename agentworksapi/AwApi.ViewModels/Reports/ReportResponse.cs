using System;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace AwApi.ViewModels.Reports
{
    [Serializable]
    public class ReportResponse<T> : Response
    {
        public string Name { get; set; }
        public DateTime ReportDateTime { get; set; }
        public ReportMetadata Metadata { get; set; }
        public T Payload { get; set; }
    }
}