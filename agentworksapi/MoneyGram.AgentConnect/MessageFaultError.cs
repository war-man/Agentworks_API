using System.Runtime.Serialization;

namespace MoneyGram.AgentConnect
{
    [DataContract(Namespace = "http://www.moneygram.com/AgentConnect1705", Name = "messageFaultError")]
    public class MessageFaultError
    {
        [DataMember(Name = "errorString", Order =1)]
        public string ErrorString { get; set; }
        [DataMember(Name = "offendingField", Order = 2)]
        public string OffendingField { get; set; }
    }
}