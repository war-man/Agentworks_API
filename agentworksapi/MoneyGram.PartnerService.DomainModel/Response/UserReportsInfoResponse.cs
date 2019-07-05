using System;
using System.Collections.Generic;

namespace MoneyGram.PartnerService.DomainModel
{
    [Serializable]
    public class UserReportsInfoResponseList : BaseServiceMessage
    {
        public List<UserReportsInfoResponse> UserReportsInfoList { get; set; }    
    }
    [Serializable]
    public class UserReportsInfoResponse
    {
        public decimal AgentId { get; set; }

        public string AgentName { get; set; }

        public string EdirGuid { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string LdapUserId { get; set; }

        public string ActiveUserFlag { get; set; }

        public string DeviceName { get; set; }

        public string ActivityType { get; set; }

        public int PosNumber { get; set; }

        public bool PosNumberFieldSpecified { get; set; }

        public System.DateTime LastLogonLclDate { get; set; }

        public bool LastLogonLclDateSpecified { get; set; }
    }

}
