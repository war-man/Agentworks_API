using System.Collections.Generic;
using System.Linq;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.Setup
{
    public static class InfoKeyGroups
    {
        public static string FirstName => @"FirstName";
        public static string MiddleName => @"MiddleName";
        public static string LastName => @"LastName\d?";
        public static string Address => @"Address";
        public static string Address2 => @"Address2";
        public static string Address3 => @"Address3";
        public static string Country => @"Country";
        public static string City => @"City";
        public static string SubdivisionCode => @"SubdivisionCode";
        public static string SubdivisionText => @"SubdivisionText";
        public static string PostalCode => @"PostalCode";
        public static string PhoneCountryCode => @"PhoneCountryCode";
        public static string Phone => @"Phone";
        public static string PhoneSMSEnabled => @"PhoneSMSEnabled";
        public static string Email => @"Email";
        public static string DOB => @"DOB";
        public static string CustomerNumber => @"CustomerNumber";
        public static string AgentTransactionId => @"AgentTransactionId";
        public static string ConsumerId => @"ConsumerId";
        public static string PersonalIdNumber => @"PersonalId\d_Number";
        public static string UseSendData => @"UseSendData";
        public static string Nationality => @"Nationality";
        public static string NationalityAtBirth => @"NationalityAtBirth";
        public static string Other => @"Other";
        public static string Issue_Year => @"Issue_Year";
        public static string Expiration_Year => @"Expiration_Year";
        public static string Month => @"Month";
        public static string Day => @"Day";
        public static string ServiceOption => @"ServiceOption";
        public static string WithLookup => @"WithLookup";
        public static string AccountType => @"AccountType";
        public static string BankCode => @"BankCode";
        public static string Text => @"Text";
        public static string BenefIdNumber => @"BenefIdNumber";
        public static string BranchCode => @"BranchCode";
        public static string BankIdentifier => @"BankIdentifier";
        public static string Flag => @"Flag";
        public static string Direction => @"direction\d";
        public static string MessageField => @"MessageField\d";
        public static string Question => @"Question";
        public static string Answer => @"Answer";
        public static string TimeToLive => @"TimeToLive";
        public static string OptIn => @"OptIn";
        public static string Amount => @"Amount";
        public static string TransactionPin => @"TransactionPin";
        public static string AccountNumber => @"AccountNumber";
        public static string OperatorName => @"OperatorName";
        public static string PcTerminalNumber => @"PcTerminalNumber";
        public static string Organization => @"Organization";
        public static string UseReceiveData => @"UseReceiveData";
        public static string OtherPayoutType => @"OtherPayoutType";
        public static string MgiRewardsNumber => @"MgiRewardsNumber";
        public static string CustomerReceiveNumber => @"CustomerReceiveNumber";



        public static string PropertyName(string val)
        {
            var properties = typeof(InfoKeyGroups).GetProperties().ToList();
            var propName = properties.FirstOrDefault(x => x.GetValue(null).ToString() == val).Name;
            return propName;
        }

        public static List<string> IdNumberGroups = new List<string>
        {
            PersonalIdNumber
        };

        public static List<string> AllGroups = new List<string>
        {
            FirstName,
            MiddleName,
            LastName,
            Address3,
            Address2,
            Address,
            City,
            SubdivisionCode,
            Country,
            PostalCode,
            PhoneCountryCode,
            PhoneSMSEnabled,
            Phone,
            Email,
            DOB,
            CustomerNumber,
            AgentTransactionId,
            ConsumerId,
            PersonalIdNumber,
            UseSendData,
            Nationality,
            NationalityAtBirth,
            Other,
            Issue_Year,
            Expiration_Year,
            Month,
            Day,
            ServiceOption,
            WithLookup,
            AccountType,
            BankCode,
            Text,
            BenefIdNumber,
            BranchCode,
            BankIdentifier,
            Flag,
            Direction,
            MessageField,
            Question,
            Answer,
            TimeToLive,
            OptIn,
            Amount,
            TransactionPin,
            AccountNumber,
            OperatorName,
            PcTerminalNumber,
            Organization,
            UseReceiveData,
            OtherPayoutType,
            MgiRewardsNumber,
            CustomerReceiveNumber
        };
    }
}