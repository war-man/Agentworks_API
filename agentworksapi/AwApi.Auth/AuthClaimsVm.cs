using System;
using System.Collections.Generic;

namespace AwApi.Auth
{
    [Serializable]
    public class AuthClaimsVm
    {
        public AuthClaimsVm()
        {
            MgiAppAwRoles = new List<string>();
        }

        public string Token { get; set; }
        public string Sub { get; set; }
        public string MgiAppAwMaxSendAmount { get; set; }
        public string MgiAppAwMaxBillPayAmount { get; set; }
        public string MgiAppAwMaxReceiveAmount { get; set; }
        public string MgiAppAwMaxSendCancellationAmount { get; set; }
        public string MgiAppAwMaxReceiveCancellationAmount { get; set; }
        public string MgiAppAwMaxAmendAmount { get; set; }
        public string MgiAppAwMaxMoneyOrderAmount { get; set; }
        public string MgiAppAwMaxVendorPaymentAmount { get; set; }
        public string MgiAgentLocationId { get; set; } // AGENT_ID "40698809"
        public string MgiUserLastLoginTS { get; set; }
        public string Given_Name { get; set; }
        public string Country { get; set; } // "USA"
        public string MgiDevicePosNumber { get; set; } // AGENT_POS_SEQ "50"
        public string MgiDeviceAgentId { get; set; }
        public string Updated_At { get; set; }
        public string MgiMainOfficeId { get; set; } // "68187237"
        public string Email { get; set; }
        public string Name { get; set; }
        public string Family_Name { get; set; }
        public string Language { get; set; } // "en-US"
        public string MgiPosUnitProfileId { get; set; }
        public string AgentPassword { get; set; } // "TEST"
        public string AgentName { get; set; } // AGENT_NAME "TestAgentName"
        public string StoreName { get; set; } // GENERIC_DOCUMENT_STORE "CHECK CENTER - BERKELEY"
        public string AgentTimeZone { get; set; } // AGENT_TIME_ZONE "GMT-05:00"/>
        public string AgentAddress1 { get; set; } // AGENT_ADDRESS_1 "187 Havemeyer Street"/>
        public string AgentAddress2 { get; set; } // AGENT_ADDRESS_2 "BROOKLYN, NEW YORK 112115612"/>
        public string AgentTelNo { get; set; } // AGENT_PHONE "7183990488"/>
        public string PrimaryReceiptLanguage { get; set; } // "ENG"/>
        public string SecondaryReceiptLanguage { get; set; } // "SPA"/>
        public string SendThirdPartyThreshold { get; set; } // key: FRAUD_LIMIT_THIRD_PARTY_ID, productID: 5 "3000"
        public string RecvThirdPartyThreshold { get; set; } // key: FRAUD_LIMIT_THIRD_PARTY_ID, productID: 6 "3000"
        public string BillPayThirdPartyThreshold { get; set; } // key: FRAUD_LIMIT_THIRD_PARTY_ID, productID: 7 "3000"
        public string MgiAppAwDualControlAllowed { get; set; }
        public List<string> MgiAppAwRoles { get; set; }
        public string DSL { get; set; }
        public string HostVersionEOLDate { get; set; }
        public string PlatformVersionEOLDate { get; set; }
    }
}