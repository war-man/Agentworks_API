namespace AwApi.Integration.Models
{ 
    public class ProductAuthorizations
    {
        #region Send Product Suite
        public bool CanSend { get; set; }
        public bool CanSendCompletion { get; set; }
        public bool CanAmend { get; set; }
        public bool CanCancel { get; set; }
        public bool CanSendReversal { get; set; }
        public bool CanFeeRefund { get; set; }
        #endregion

        #region Receive Product Suite
        public bool CanReceive { get; set; }
        public bool CanReceiveCompletion { get; set; }
        public bool CanReceiveReversal { get; set; }
        #endregion

        #region Bill Pay Suite
        public bool CanBillPay { get; set; }
        public bool CanBillPayCompletion { get; set; }
        public bool CanLoadPrepaidCard { get; set; }
        public bool CanExpressPayment { get; set; }
        public bool CanUtilityBillPay { get; set; }
        #endregion

        #region Money Order Suite
        public bool CanMoneyOrder { get; set; }
        public bool CanVendorPay { get; set; }
        #endregion        
    }
}