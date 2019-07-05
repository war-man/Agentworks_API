using System.Xml.Serialization;

namespace TransactionRunner.Transactions.Handlers
{
    /// <summary>
    ///     Class that encapsulates parameters needed to execute staged bill pay transactions.
    /// </summary>
    [XmlType("Staged_Bill_Pay")]
    public class StagedBillPayParameters : BillPayParameters
    {
        /// <summary>
        /// Returns transaction name
        /// </summary>
        public override string TransactionName => "Staged Bill Pay";
    }
}