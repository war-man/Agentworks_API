using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using TransactionRunner.Transactions;

namespace TransactionRunner.ImportExport
{
    /// <summary>
    /// Handler for XML file format
    /// </summary>
    public class XMLFileHandler : IFileHandler
    {
        private readonly XmlSerializer _serializer = new XmlSerializer(typeof(ParamsList), GetParamsTypes());

        private readonly XmlSerializer _serializerWithReferenceNumber = new XmlSerializer(typeof(ParamsList),
            GetOverrideForRerefenceNumber(), GetParamsTypes(), null, null);

        private static Type[] GetParamsTypes()
        {
            return Assembly.GetAssembly(typeof(BaseParams))
                .GetTypes()
                .Where(t => typeof(BaseParams).IsAssignableFrom(t))
                .ToArray();
        }

        private static XmlAttributeOverrides GetOverrideForRerefenceNumber()
        {
            XmlAttributeOverrides overrides = new XmlAttributeOverrides();
            XmlAttributes attributes = new XmlAttributes { XmlIgnore = false };
            attributes.XmlElements.Add(new XmlElementAttribute("Result"){Order = 0});
            overrides.Add(typeof(BaseParams), nameof(BaseParams.ReferenceNumber), attributes);
            return overrides;
        }

        /// <summary>
        /// File format
        /// </summary>
        public ExportFileFormat FileFormat => ExportFileFormat.XML;

        /// <summary>
        /// Creates header - empty in this case
        /// </summary>
        /// <param name="transactionParams"></param>
        /// <param name="includeReferenceNumber"></param>
        /// <returns></returns>
        public string GetHeader(BaseParams transactionParams, bool includeReferenceNumber)
        {
            return string.Empty;
        }

        /// <summary>
        /// Creates XML element for passed transaction criteria
        /// </summary>
        /// <param name="transactionParams"></param>
        /// <param name="includeReferenceNumber"></param>
        /// <returns></returns>
        public string GetData(IEnumerable<BaseParams> transactionParams, bool includeReferenceNumber)
        {
            return SerializeToString(new ParamsList(transactionParams), includeReferenceNumber);
        }

        /// <summary>
        /// Appends criteria to existing file contents
        /// </summary>
        /// <param name="existingContents"></param>
        /// <param name="transactionParams"></param>
        /// <param name="includeReferenceNumber"></param>
        /// <returns></returns>
        public string Append(string existingContents, IEnumerable<BaseParams> transactionParams, bool includeReferenceNumber)
        {
            ParamsList list = DeserializeFromString(existingContents);
            list.AddRange(transactionParams);
            return SerializeToString(list, includeReferenceNumber);
        }

        /// <summary>
        /// Loads XML file contents and returns transaction criteria
        /// </summary>
        /// <param name="fileContents"></param>
        /// <returns></returns>
        public IEnumerable<BaseParams> LoadTransactions(string fileContents)
        {
            return DeserializeFromString(fileContents);
        }

        private string SerializeToString(ParamsList list, bool includeReferenceNumber)
        {
            using (TextWriter writer = new StringWriterUtf8())
            using (XmlWriter xmlWriter = XmlWriter.Create(writer, new XmlWriterSettings { Indent = true, Encoding = Encoding.UTF8 }))
            {
                (includeReferenceNumber ? _serializerWithReferenceNumber : _serializer).Serialize(xmlWriter, list);
                string xml = writer.ToString();

                return xml;
            }
        }

        private ParamsList DeserializeFromString(string xml)
        {
            using (TextReader reader = new StringReader(xml))
            {
                return (ParamsList)_serializer.Deserialize(reader);
            }
        }

        /// <summary>
        /// StringWriter forcing UTF8 encoding
        /// </summary>
        public class StringWriterUtf8 : StringWriter
        {
            /// <summary>
            /// Forced UTF8 encoding
            /// </summary>
            public override Encoding Encoding => Encoding.UTF8;
        }
    }
}