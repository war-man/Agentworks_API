using System.Collections.Generic;
using System.Xml.Serialization;
using TransactionRunner.Transactions;

namespace TransactionRunner.ImportExport
{
    /// <summary>
    /// Helper class to provide node for multiple transaction criterias
    /// </summary>
    [XmlType(TypeName = "Transactions")]
    public class ParamsList : List<BaseParams>
    {
        /// <summary>
        /// Creates new empty instance
        /// </summary>
        public ParamsList()
        {
        }

        /// <summary>
        /// Creates new instance for passed elements
        /// </summary>
        /// <param name="collection"></param>
        public ParamsList(IEnumerable<BaseParams> collection)
            : base(collection)
        {
        }
    }
}