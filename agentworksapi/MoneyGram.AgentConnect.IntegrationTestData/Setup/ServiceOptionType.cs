using System.Collections.Generic;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.Setup
{
    public static class ServiceOptionType
    {
        public static string WillCall => "WILL_CALL";
        public static string HdsUsd => "HDS_USD";
        public static string HdsLocal => "HDS_LOCAL";
        public static string HomeDelivery => "HOME_DELIVERY";
        public static string CardDeposit => "CARD_DEPOSIT";
        public static string BankDeposit => "BANK_DEPOSIT";
        public static string ReceiveAt => "RECEIVE_AT";

        public static List<string> PreferredOrder => new List<string> { WillCall, BankDeposit, HomeDelivery };
    }
}