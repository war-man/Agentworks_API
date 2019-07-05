namespace TransactionRunner.ViewModels.Settings
{
	public class SendSettings
	{
		public string CountryCode { get; set; }

		public string CountrySubdivisionCode { get; set; }

		public string CurrencyCode { get; set; }

		public string AmountRange { get; set; }

        public decimal CustomAmount { get; set; }

		public string ItemChoice { get; set; }

		public string ServiceOption { get; set; }

        public string ThirdPartyType { get; set; }
    }
}