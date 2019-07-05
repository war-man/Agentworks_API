using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using TransactionRunner.Transactions;

namespace TransactionRunner.ImportExport
{
    /// <summary>
    /// Handler for CSV file format
    /// </summary>
    public class CSVFileHandler : IFileHandler
    {
        /// <summary>
        /// File format
        /// </summary>
        public ExportFileFormat FileFormat => ExportFileFormat.CSV;

        /// <summary>
        /// Creates header - property names
        /// </summary>
        /// <param name="transactionParams"></param>
        /// <param name="includeReferenceNumber"></param>
        /// <returns></returns>
        public string GetHeader(BaseParams transactionParams, bool includeReferenceNumber)
        {
            IList<string> names = transactionParams.GetPropertiesToExport()
                .Select(x => x.GetCustomAttribute<XmlElementAttribute>().ElementName).ToList();

            if (includeReferenceNumber)
            {
                names.Insert(0, "Result");
            }

            return $"{string.Join(",", names)}";
        }

        /// <summary>
        /// Creates row of comma separated values
        /// </summary>
        /// <param name="transactionParams"></param>
        /// <param name="includeReferenceNumber"></param>
        /// <returns></returns>
        public string GetData(IEnumerable<BaseParams> transactionParams, bool includeReferenceNumber)
        {
            IList<string> rows = new List<string>();
            foreach (BaseParams p in transactionParams)
            {
                IList<string> values = p.GetPropertiesToExport().Select(x => GetValueAsString(p, x)).ToList();
                if (includeReferenceNumber)
                {
                    values.Insert(0, p.ReferenceNumber);
                }
                rows.Add(string.Join(",", values));
            }

            return string.Join(Environment.NewLine, rows);
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
            return $"{existingContents}{Environment.NewLine}{GetData(transactionParams, includeReferenceNumber)}";
        }

        /// <summary>
        /// Loads CSV file contents and returns transaction criteria
        /// </summary>
        /// <param name="fileContents"></param>
        /// <returns></returns>
        public IEnumerable<BaseParams> LoadTransactions(string fileContents)
        {
            IList<BaseParams> transactions = new List<BaseParams>();
            IList<string> lines = fileContents
                .Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries).ToList();
            IList<string> propertyOrder = lines[0].Split(',').ToList();
            int transactionNameIndex = propertyOrder.IndexOf("Transaction_Name");

            IList<BaseParams> emptyObjects = GetEmptyParamObjects();

            foreach (string row in lines.Skip(1))
            {
                IList<string> rowValues = row.Split(',').ToList();
                string transactionName = rowValues[transactionNameIndex];

                var emptyObject = emptyObjects.SingleOrDefault(x => x.TransactionName == transactionName);
                if (emptyObject != null)
                {
                    BaseParams newObject = (BaseParams) Activator.CreateInstance(emptyObject.GetType());

                    FillProperties(newObject, rowValues, propertyOrder);
                    transactions.Add(newObject);
                }
            }
            return transactions;
        }

        private void FillProperties(BaseParams newObject, IList<string> rowValues, IList<string> propertyOrder)
        {
            var properties = newObject.GetPropertiesToExport();

            foreach (PropertyInfo property in properties.Where(p => p.CanWrite))
            {
                ExportableAttribute attribute = property.GetCustomAttribute<ExportableAttribute>();
                int index = propertyOrder.IndexOf(attribute.ElementName);
                string value = string.Empty;
                if (index < rowValues.Count)
                {
                    value = rowValues[index];
                }

                try
                {
                    property.SetValue(newObject,
                        Convert.ChangeType(value, property.PropertyType, ImportExportSvc.USCulture), null);
                }
                catch (FormatException exception)
                {
                    newObject.AddValidationError(property.Name, $"Exception when parsing value from file: {exception.Message}");
                }
            }
        }

        private IList<BaseParams> GetEmptyParamObjects()
        {
            var types = Assembly.GetAssembly(typeof(BaseParams))
                .GetTypes()
                .Where(t => typeof(BaseParams).IsAssignableFrom(t) && !t.IsAbstract)
                .ToList();

            return types.Select(Activator.CreateInstance).Cast<BaseParams>().ToList();
        }

        private string GetValueAsString(BaseParams obj, PropertyInfo propertyInfo)
        {
            object value = propertyInfo.GetValue(obj);
            if (value == null)
            {
                return String.Empty;
            }

            if (value is decimal d)
            {
                return d.ToString(ImportExportSvc.USCulture);
            }

            return value.ToString();
        }
    }
}