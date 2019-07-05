namespace TransactionRunner.ViewModels.Settings
{
    /// <summary>
    /// User settings for Bill Pay transaction
    /// </summary>
    public class BillPaySettings
    {
        /// <summary>
        /// Selected country
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// Selected amount range code
        /// </summary>
        public string AmountRange { get; set; }

        /// <summary>
        /// Value for custom amount option
        /// </summary>
        public decimal CustomAmount { get; set; }

        /// <summary>
        /// Selected third party type
        /// </summary>
        public string ThirdPartyType { get; set; }

        /// <summary>
        /// Selected biller code
        /// </summary>
        public string Biller { get; set; }

        /// <summary>
        /// Biller code for manual entry option
        /// </summary>
        public string ManualBillerCode { get; set; }

        /// <summary>
        /// Biller account for manual entry option
        /// </summary>
        public string ManualBillerAccountNumber { get; set; }
    }
}
