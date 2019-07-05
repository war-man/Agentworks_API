using System.Runtime.Serialization;

namespace MoneyGram.AgentConnect
{
    [DataContract(Namespace = "http://www.moneygram.com/AgentConnect1705", Name = "requestSecurityHeader")]
    public class RequestSecurityHeader
    {
        [DataMember]
        public UsernameToken UsernameToken { get; set; }
    }
}