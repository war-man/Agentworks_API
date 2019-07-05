using System.Xml.Serialization;
using TransactionRunner.ImportExport;

namespace TransactionRunner.Transactions.Handlers
{
    /// <summary>
    /// Class to represent criteria for Bill Pay transaction
    /// </summary>
    [XmlType("Bill_Pay")]
    public class BillPayParameters : BaseParams
    {
        /// <summary>
        /// Returns transaction name
        /// </summary>
        public override string TransactionName => "Bill Pay";

        /// <summary>
        ///     Gets or sets the Biller Code.
        /// </summary>
        [Exportable(ElementName = "Biller_Code", ColumnHeader = "Biller Code", Order = 15)]
        public string BillerCode { get; set; }

        /// <summary>
        ///     Gets or sets the Biller Account Number.
        /// </summary>
        [Exportable(ElementName = "Biller_Account_Number", ColumnHeader = "Biller Account Number", Order = 16)]
        public string BillerAccountNumber { get; set; }
    }
}