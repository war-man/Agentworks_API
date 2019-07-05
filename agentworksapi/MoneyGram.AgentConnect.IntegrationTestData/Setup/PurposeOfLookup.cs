namespace MoneyGram.AgentConnect.IntegrationTest.Data.Setup
{
    public static class PurposeOfLookup
    {
        // Purposes may include: Status, ReceiveValidation,
        // ReceiveCompletion(FormFree or Resume), Refund, Cancel, Amend,
        // SendCompletion(FormFree or Resume), BPCompletion(FormFree or
        // Resume), Note: Status should be used as "default" if not yet known
        // true purpose
        public static string Status => "Status";
        public static string ReceiveValidation => "ReceiveValidation"; //didn't work
        public static string ReceiveCompletion => "ReceiveCompletion";
        public static string Receive => "RECEIVE";
        public static string Refund => "Refund";  //didn't work
        public static string Cancel => "Cancel";  //didn't work
        public static string Amend => "Amend";
        public static string SendCompletion => "SendCompletion";  //didn't work
        public static string BillPayCompletion => "BPCompletion";  //didn't work
        public static string SendReversal => "SendRev";
        public static string ReceiveReversal => "RcvRev";
    }
}