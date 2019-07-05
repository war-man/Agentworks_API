namespace AwApi.ViewModels.Reports
{
    public class BillPayReportDetail : TransactionDetail
    {
        public string ProductType { get; set; }
        public double FaceAmount { get; set; }
        public double FeeAmount { get; set; }
    }
}