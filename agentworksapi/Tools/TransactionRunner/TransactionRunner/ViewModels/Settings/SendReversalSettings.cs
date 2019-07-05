namespace TransactionRunner.ViewModels.Settings
{
	public class SendReversalSettings : SendSettings
	{
		public string RefundReasonCode { get; set; }

		public bool RefundFee { get; set; }
    }
}