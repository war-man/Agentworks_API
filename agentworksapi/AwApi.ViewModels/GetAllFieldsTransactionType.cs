namespace AwApi.ViewModels
{
    public static class GetAllFieldsTransactionType
    {
        public static string Send => "SEND";
        public static string Receive => "RECEIVE";
        public static string BillPayment => "BILL_PAYMENT";
        public static string TransactionLookup => "TRANSACTION_LOOKUP";
        public static string Amend => "AMEND";
        public static string SendReversal => "SEND_REVERSAL";

        public static string ReceiveReversal => "RECEIVE_REVERSAL";
        public static string BankDetails => "BANK_DETAILS";
        public static string ConsumerLookup => "CONSUMER_LOOKUP";
    }
}