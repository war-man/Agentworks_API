using System.Xml.Serialization;
using TransactionRunner.ImportExport;

namespace TransactionRunner.Transactions.Handlers
{
    /// <summary>
    ///     Class that encapsulates parameters needed to execute send transactions.
    /// </summary>
    [XmlType("Send")]
    public class SendParameters : BaseParams
    {
        /// <summary>
        /// Returns transaction name
        /// </summary>
        public override string TransactionName => "Send";

        /// <summary>
        ///     Gets or sets the Country parameter.
        /// </summary>
        [Exportable(ElementName = "Country", ColumnHeader = "Country", Order = 10)]
        public string Country { get; set; }

        /// <summary>
        ///     Gets or sets the State parameter.
        /// </summary>
        [Exportable(ElementName = "State", ColumnHeader = "State", Order = 11, IsRequired = false)]
        public string State { get; set; }

        /// <summary>
        ///     Gets or sets the Fee Type parameter.
        /// </summary>
        [Exportable(ElementName = "Fee_Type", ColumnHeader = "Fee Type", Order = 12)]
        public string FeeType { get; set; }

        /// <summary>
        ///     Gets or sets the Send Curr parameter.
        /// </summary>
        [Exportable(ElementName = "Send_Currency", ColumnHeader = "Send Currency", Order = 13)]
        public string SendCurr { get; set; }

        /// <summary>
        ///     Gets or sets the Service Option parameter.
        /// </summary>
        [Exportable(ElementName = "Service_Option", ColumnHeader = "Service Option", Order = 14)]
        public string ServiceOption { get; set; }
    }
}