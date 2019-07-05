using System.Runtime.Serialization;

namespace MoneyGram.AgentConnect
{
    [DataContract(Namespace = "http://www.moneygram.com/AgentConnect1705", Name = "usernameToken")]
    public class UsernameToken
    {
        [DataMember(Order = 1)]
        public string Username { get; set; }
        [DataMember(Order = 2)]
        public string Password { get; set; }
    }
}