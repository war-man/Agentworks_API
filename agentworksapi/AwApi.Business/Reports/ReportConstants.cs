namespace AwApi.Business.Reports
{
    public static class ReportConstants
    {
        public static class Common
        {
            public const string AMT_FORMAT = "{0:###,###,###,##0.00}";
            public const string AMT_FORMAT_NEG = "{0:-###,###,###,##0.00}";
            public const string DATE_FORMAT = "{0:MM/dd/yyyy HH:mm:ss}";

            public const string MT_PRODUCT_ID = "1";
            public const string FAST_CASH_PRODUCT_ID = "2";
            public const string EXPRESS_PAYMENT_PRODUCT_ID = "3";
            public const string EXPRESS_PAYMENT_PRODUCT_TYPE = "EP";
            public const string UTILITY_BILL_PAYMENT_PRODUCT_ID = "10";
            public const string UTILITY_BILL_PAYMENT_PRODUCT_TYPE = "UBP";
            public const string DETAIL_REPORT_NAME = "STRKEY_DETAIL_REPORT_NAME";
            public const string SUMMARY_REPORT_NAME = "STRKEY_SUMMARY_REPORT_NAME";
            public const string SALES_REPORT_NAME = "STRKEY_SALES_REPORT_NAME";
            public const string USER_REPORT_NAME = "User Report";
            public const string TRANEXCEED_REPORT_NAME = "Transactions that exceed Operators maximum limits";

            public const string TRAN_TYPE_UP = "Utility Payment";
            public const string TRAN_TYPE_EP = "Express Payment";
            public const string TRAN_TYPE_SEND = "MoneyGram Send";
            public const string TRAN_TYPE_RECEIVE = "Receive";
            public const string TRAN_TYPE_CANCEL = "MoneyGram Cancel";
            public const string TRAN_TYPE_REVERSAL = "Receive Reversal";
            public const string TRAN_TYPE_REFUND = "Refund";
            public const string TRAN_TYPE_UNKNOWN = "Unknown";

            public const string SUPER_AGENT = "superAgent.posZeroText";
            public const string SR_NAME = "STRKEY_SR_NAME";
            public const string STR_NAME = "STRKEY_SEND_TOTAL";
            public const string PICT_NAME = "STRKEY_BILL_PAYMENTS";
            public const string PIC_NAME = "STRKEY_BILL_PMNT_TOTAL";
            public const string SSR_NAME = "STRKEY_SEND";
            public const string RTR_NAME = "STRKEY_RECV_TOTAL";
            public const string RSR_NAME = "STRKEY_RECV";

            public const string FACE = "STRKEY_FACE";
            public const string FEE = "STRKEY_FEE";
            public const string TOTAL = "STRKEY_TOTAL";
            public const string AMOUNT = "STRKEY_AMOUNT";
            public const string REF_NUMBER = "STRKEY_REFNUM";
            public const string POS_ID = "STRKEY_POS_ID";
            public const string USER_ID = "STRKEY_USER_ID";
            public const string AGENT_ID = "agentId";
            public const string TIME = "STRKEY_TIME_LOCAL";
            public const string COMPUTER_NAME = "STRKEY_COMPUTER_NAME";
            public const string AGENT_NAME = "agentName";
            public const string FIRST_NAME = "firstName";
            public const string LAST_NAME = "lastName";
            public const string SEND_CURRENCY = "STRKEY_SEND_CURRENCY";
            public const string COUNT = "STRKEY_COUNT";
            public const string RECEIVE_CURRENCY = "STRKEY_RECV_CURRENCY";
            public const string REPORT_NAME = "Report Name";
            public const string ACTIVITY_DATE = "Activity Date";
            public const string REPORT_DATE = "Report Date";
        }

        public static class DailyTranSales
        {
            public const string ISO_CURRENCY_CODE = "STRKEY_CURRENCY_CODE";
            public const string AGENT_TIME = "STRKEY_TIME";
            public const string EMPLOYEE_NUMBER = "STRKEY_EMPLOYEE_NUMBER";
            public const string RECEIVER_NAME = "STRKEY_RECEIVER_NAME";
            public const string REFERENCE_ID = "STRKEY_REF_NUM";
            public const string SERIAL_NUMBER = "STRKEY_SERIAL_NUMBER";
            public const string TRANSACTION_TYPE = "STRKEY_TRANSACTION_TYPE";
            public const string EXCHANGE_RATE = "STRKEY_FX";
            public const string SENDER = "STRKEY_SENDER";
            public const string USERNAME = "STRKEY_USER_NAME";
            public const string AGENT_ID = "STRKEY_AGENT_ID";
            public const string TOTAL_AMOUNT = "STRKEY_TOTAL_AMOUNT";
        }
        public static class DailyActivitySummary
        {
            public const string PAYMENT_CURRENCY = "STRKEY_PMNT_CURRENCY";
            public const string PRODUCT_TYPE = "STRKEY_PRODUCT_TYPE";
        }

        public static class BPDetailLookup
        {
            public const string PRODUCT_TYPE = "STRKEY_PRODUCT_TYPE";
            public const string INFORMATIONAL_FEE_INDICATOR = "informationalFeeIndicator";
        }

        public static class MTDetailLookup
        {
            public const string RECEIVER_LAST_NAME = "STRKEY_RECV_LAST_NAME";
            public const string AUTH_CODE = "STRKEY_AUTH_CODE";

        }

        public static class StagedContainer
        {
            public const string ACTIVITY_DATE = "activityDate";
            public const string MO_AGENT_ID = "moAgentId";
            public const string REPORT_SOURCE = "reportSource";
            public const string REPORT_SOURCE_DEST = "Local";
            public const string MAIN_OFFICE_ID = "mainOfficeId";
            public const string MAIN_OFFICE_NAME = "mainOfficeName";
            public const string STORE_NAME = "storeName";
        }

        public static class TransactionExceedDetails
        {
            public const string CITY = "City";
            public const string LDAP_USER_ID = "LdapUserId";
            public const string EVENT_TRAN_CODE = "EventTranCode";
            public const string EVENT_TRAN_NAME = "EventTranName";
            public const string TRAN_REF_ID = "TranRefId";
            public const string TRAN_LIM_CODE = "TranLimCode";
            public const string TRAN_LIM_BSNS_DESC = "TranLimBsnsDesc";
            public const string EVENT_FACE_TRAN_AMT = "EventFaceTranAmt";
            public const string TRAN_LIM_USD_AMT = "TranLimUsdAmt";
            public const string MGR_FIRST_NAME = "MgrFirstName";
            public const string MGR_LAST_NAME = "MgrLastName";
            public const string MGR_LDAP_USER_ID = "MgrLdapUserId";
            public const string EVENT_TRAN_EVENT_DATE = "EventTranEvntDate";
            public const string EVENT_TRAN_EVENT_LCL_DATE_FIELD = "EventTranEvntLclDateField";
        }

        public static class UserDetails
        {
            public const string ACTIVE_USER_FLAG = "activeUserFlag";
            public const string ACTIVITY_TYPE = "activityType";
            public const string DEVICE_NAME = "deviceName";
            public const string EDIR_GUID = "eDirGuid";
            public const string LAST_LOGON_LCL_DATE = "lastLogonLclDate";
            public const string LDAP_USER_ID = "ldapUserId";
            public const string POS_NUMBER = "posNumber";
        }
    }
}