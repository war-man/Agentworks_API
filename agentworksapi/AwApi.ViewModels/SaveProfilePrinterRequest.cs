using System;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace AwApi.ViewModels
{
    [Serializable]
    public class SaveProfilePrinterRequest : Request
    {
        public string PrinterName { get; set; }
    }
}
