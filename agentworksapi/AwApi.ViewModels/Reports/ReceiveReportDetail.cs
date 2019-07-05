namespace AwApi.ViewModels.Reports
{
    public class ReceiveReportDetail : TransactionDetail
    {
        public string ReceiverLastName { get; set; }
        public string AuthCode { get; set; }
        public string TransactionType { get; set; }
    }
}