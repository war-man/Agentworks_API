namespace AwApi.ViewModels.Reports
{
    public class SendTotalReportDetail
    {
        public string Currency { get; set; }
        public int Count { get; set; }
        public double FaceAmount { get; set; }
        public double FeeAmount { get; set; }
        public double TotalAmount { get; set; }
    }
}