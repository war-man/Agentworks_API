using System;
using System.Collections.Generic;

namespace MoneyGram.PartnerService.DomainModel
{
    [Serializable]
    public class TransactionExceedInfoResponse : BaseServiceMessage 
    {
        public List<TransactionExceedReportsInfo> transactionExceedReportsInfoList { get; set; }
    }

    [Serializable]
    public class TransactionExceedReportsInfo 
    {
        public decimal AgentId { get; set; }
        
        public string AgentName { get; set; }
        
        public string City { get; set; }
        
        public string PosId { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public string LdapUserId { get; set; }
        
        public string EventTranCode { get; set; }
        
        public string EventTranName { get; set; }
        
        public string TranRefId { get; set; }
        
        public int TranLimCode { get; set; }
        
        public string TranLimBsnsDesc { get; set; }
        
        public decimal EventFaceTranAmt { get; set; }
        
        public decimal TranLimUsdAmt { get; set; }
        
        public string MgrFirstName { get; set; }
        
        public string MgrLastName { get; set; }
        
        public string MgrLdapUserId { get; set; }
        
        public System.DateTime EventTranEvntDate { get; set; }
        
        public bool EventTranEvntDateFieldSpecified { get; set; }
        
        public System.DateTime EventTranEvntLclDateField { get; set; }
        
        public bool EventTranEvntLclDateSpecified { get; set; }
    }
}
