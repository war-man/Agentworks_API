using System.Xml.Serialization;

namespace TransactionRunner.ImportExport
{
    /// <summary>
    /// Attribute to mark properties that can be exported and imported to file
    /// </summary>
    public class ExportableAttribute : XmlElementAttribute
    {
        /// <summary>
        /// Header for column in Import View
        /// </summary>
        public string ColumnHeader { get; set; }

        /// <summary>
        /// Name of mapping dictionary to translate codes into human readable values
        /// </summary>
        public string ExportValueMap { get; set; }

        /// <summary>
        /// If true, field value is required when importing transactions from file
        /// </summary>
        public bool IsRequired { get; set; } = true;
    }
}