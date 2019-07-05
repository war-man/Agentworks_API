namespace AwApi.ViewModels
{
    public class TransactionalLimits
    {
        public decimal? MaxSendAmount { get; set; }
        public decimal? MaxAmendAmount { get; set; }
        public decimal? MaxBillPayAmount { get; set; }
        public decimal? MaxReceiveAmount { get; set; }
        public decimal? MaxSendCancellationAmount { get; set; }
        public decimal? MaxReceiveCancellationAmount { get; set; }
        public decimal? MaxMoneyOrderAmount { get; set; }
        public decimal? MaxVendorPaymentAmount { get; set; }
    }
}