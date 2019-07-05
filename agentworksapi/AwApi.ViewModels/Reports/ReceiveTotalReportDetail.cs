namespace AwApi.ViewModels.Reports
{
    public class ReceiveTotalReportDetail
    {
        public string TransactionType { get; set; } 
        public string Currency { get; set; }
        public int Count { get; set; }
        public double TotalAmount { get; set; }
    }
}