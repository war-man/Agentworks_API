using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.Models;
using MoneyGram.AgentConnect.IntegrationTest.Data.Repositories;
using MoneyGram.AgentConnect.IntegrationTest.Data.Setup;
using TransactionRunner.ConfigProviders;
using TransactionRunner.Helpers;
using TransactionRunner.Settings;
using TransactionRunner.Transactions;
using TransactionRunner.Transactions.Handlers;
using AmountRange = MoneyGram.AgentConnect.IntegrationTest.Data.Models.AmountRange;
using FeeType = MoneyGram.AgentConnect.IntegrationTest.Data.Models.FeeType;

namespace TransactionRunner.ImportExport
{
    /// <summary>
    ///     Service for exporting and importing transaction criteria from files
    /// </summary>
    public class ImportExportSvc
    {
        /// <summary>
        /// Creates new service instance
        /// </summary>
        public ImportExportSvc()
        {
            _fileHandlers.Add(ExportFileFormat.CSV, new CSVFileHandler());
            _fileHandlers.Add(ExportFileFormat.XML, new XMLFileHandler());

            _maps.Add(nameof(_countryMap), _countryMap);
            _maps.Add(nameof(_subdivisionMap), _subdivisionMap);
            _maps.Add(nameof(_amountRangeMap), _amountRangeMap);
            _maps.Add(nameof(_feeTypeMap), _feeTypeMap);
            _maps.Add(nameof(_currencyMap), _currencyMap);
            _maps.Add(nameof(_serviceOptionMap), _serviceOptionMap);
        }

        private readonly IList<string> _environmentsList = StaticSettings.SettingsSvc?
            .UserSettings?
            .EnvironmentAgents?
            .Select(x => x.Key)
            .ToList();
        private readonly Dictionary<string, string> _countryMap = new CountryRepository()
            .GetAllValues<List<CountryInfo>>()
            .ToDictionary(x => x.CountryCode, x => x.CountryName);

        private readonly IList<string> _agentStateList = new CountrySubdivisionRepository()
            .GetAllValues<List<CountrySubdivisionInfo>>()
            .SelectMany(x => x.Subdivisions)
            .Select(x => x.CountrySubdivisionCode.Contains("-") ? x.CountrySubdivisionCode.Split('-')[1] : x.CountrySubdivisionCode)
            .Distinct()
            .ToList();

        private readonly Dictionary<string, string> _subdivisionMap = new CountrySubdivisionRepository()
            .GetAllValues<List<CountrySubdivisionInfo>>()
            .SelectMany(x => x.Subdivisions)
            .ToDictionary(x => x.CountrySubdivisionCode, x => x.CountrySubdivisionName);

        private readonly Dictionary<string, string> _amountRangeMap = new AmountRangeRepository()
            .GetAllValues<List<AmountRange>>()
            .Append(new AmountRange { Code = "CustomAmount", Display = "Custom Amount" })
            .ToDictionary(x => x.Code, x => x.Display);

        private readonly Dictionary<string, string> _currencyMap = new CurrencyRepository()
            .GetAllValues<List<CurrencyInfo>>()
            .ToDictionary(x => x.CurrencyCode, x => x.CurrencyName);

        private readonly Dictionary<string, string> _feeTypeMap = new FeeTypeRepository()
            .GetAllValues<List<FeeType>>()
            .ToDictionary(x => x.Code, x => x.Display);

        private readonly IList<string> _thirdPartyList =
            new List<string> {TestThirdPartyType.None, TestThirdPartyType.Org, TestThirdPartyType.Person};

        private readonly Dictionary<string, string> _serviceOptionMap = new ServiceOptionRepository()
            .GetAllValues<List<ServiceOption>>()
            .ToDictionary(x => x.Key, x => x.Display);

        private readonly IList<string> _refundReasonList = new RefundReasonRepository()
            .GetAllValues<List<EnumeratedIdentifierInfo>>()
            .Select(x => x.Identifier)
            .ToList();

        /// <summary>
        /// Constant culture used to format values when importing and exporting
        /// </summary>
        public static readonly CultureInfo USCulture = CultureInfo.CreateSpecificCulture("en-us");

        private readonly Dictionary<ExportFileFormat, IFileHandler> _fileHandlers = new Dictionary<ExportFileFormat, IFileHandler>();
        private readonly Dictionary<string, Dictionary<string, string>> _maps = new Dictionary<string, Dictionary<string, string>>();

        /// <summary>
        /// Export operation result
        /// </summary>
        public enum ExportResult
        {
            /// <summary>
            /// Success
            /// </summary>
            Success,

            /// <summary>
            /// Parameters set in current transaction is different than in existing file
            /// </summary>
            ParameterTypeMismatch
        }

        /// <summary>
        /// Exports transaction criteria to file
        /// </summary>
        /// <param name="transactionParams"></param>
        /// <param name="fileName"></param>
        /// <param name="mode"></param>
        /// <param name="includeReferenceNumber"></param>
        /// <returns></returns>
        public ExportResult Export(IList<BaseParams> transactionParams, string fileName, ExportMode mode, bool includeReferenceNumber)
        {
            var fileHandler = _fileHandlers[GetFormatForFile(fileName)];

            IList<BaseParams> mappedParams = transactionParams.Select(MapParams).ToList();

            StringBuilder builder = new StringBuilder();
            if (mode == ExportMode.Overwrite)
            {
                string headers = fileHandler.GetHeader(mappedParams.First(), includeReferenceNumber);
                builder.AppendLine(headers);
                builder.AppendLine(fileHandler.GetData(mappedParams, includeReferenceNumber));
            }
            else if (mode == ExportMode.Append)
            {
                string currentContents = File.ReadAllText(fileName);

                bool canAppend = CanAppend(currentContents, transactionParams, fileHandler);
                if (!canAppend)
                {
                    return ExportResult.ParameterTypeMismatch;
                }

                builder.Append(fileHandler.Append(currentContents, mappedParams, includeReferenceNumber));
            }

            string fileData = builder.ToString().Trim();
            FileHelper.SaveOutputData(fileData, fileName, false);
            return ExportResult.Success;
        }

        private bool CanAppend(string currentContents, IEnumerable<BaseParams> transactionParams, IFileHandler fileHandler)
        {
            var currentParams = fileHandler.LoadTransactions(currentContents);
            var propertiesSet = transactionParams.SelectMany(x => x.GetPropertiesToExport().Select(p => p.Name)).Distinct().ToList();
            
            foreach (BaseParams current in currentParams)
            {
                var currentProperties = current.GetPropertiesToExport().Select(x => x.Name).ToList();
                if (!propertiesSet.SequenceEqual(currentProperties))
                {
                    return false;
                }
            }

            return true;
        }

        private BaseParams MapParams(BaseParams transactionParams)
        {
            var mapped = transactionParams.Clone();
            foreach (PropertyInfo propertyInfo in transactionParams.GetPropertiesToExport())
            {
                var attribute = propertyInfo.GetCustomAttribute<ExportableAttribute>();
                if (attribute?.ExportValueMap != null)
                {
                    var mappedValue = GetDisplayedValue(propertyInfo.GetValue(mapped) as string, attribute.ExportValueMap);
                    propertyInfo.SetValue(mapped, mappedValue);
                }
            }

            return mapped;
        }

        private string GetDisplayedValue(string key, string mapName)
        {
            var map = _maps[mapName];
            if (map.ContainsKey(key))
            {
                return map[key];
            }

            return key;
        }

        private ExportFileFormat GetFormatForFile(string fileName)
        {
            string extension = Path.GetExtension(fileName)?.Replace(".", "");
            if (extension != null)
            {
                return (ExportFileFormat) Enum.Parse(typeof(ExportFileFormat), extension, true);
            }

            return ExportFileFormat.CSV;
        }

        /// <summary>
        /// Loads and validates transactions from file (for Import window)
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public IEnumerable<BaseParams> Load(string fileName)
        {
            IFileHandler handler = _fileHandlers[GetFormatForFile(fileName)];

            var transactions = handler.LoadTransactions(File.ReadAllText(fileName)).ToList();

            foreach (var transaction in transactions)
            {
                Validate(transaction);
            }

            return transactions;
        }

        private void Validate(BaseParams transaction)
        {
            foreach (PropertyInfo propertyInfo in transaction.GetPropertiesToExport())
            {
                string propertyName = propertyInfo.Name;
                object propertyValue = propertyInfo.GetValue(transaction);

                var attribute = propertyInfo.GetCustomAttribute<ExportableAttribute>();
                string errorMessage = null;
                if (attribute.IsRequired && propertyInfo.PropertyType == typeof(string) && string.IsNullOrWhiteSpace((string)propertyValue))
                {
                    errorMessage = "Value is required";
                }

                if (errorMessage == null)
                {
                    switch (propertyName)
                    {
                        case nameof(BaseParams.TransactionName):
                            errorMessage = ValidateTransactionName((string) propertyValue);
                            break;
                        case nameof(BaseParams.Environment):
                            errorMessage = ValidateEnvironment((string) propertyValue);
                            break;
                        case nameof(BaseParams.AgentCountryIsoCode):
                            errorMessage = ValidateCountry((string) propertyValue);
                            break;
                        case nameof(BaseParams.AgentState):
                            errorMessage = ValidateAgentState((string) propertyValue);
                            break;
                        case nameof(BaseParams.AmtRange):
                            errorMessage = ValidateAmountRange((string) propertyValue);
                            break;
                        case nameof(BaseParams.CustomAmount):
                            //currently no specific validation
                            break;
                        case nameof(BaseParams.ThirdPartyType):
                            errorMessage = ValidateThirdPartyType((string) propertyValue);
                            break;
                        case nameof(SendParameters.Country):
                            errorMessage = ValidateCountry((string) propertyValue);
                            break;
                        case nameof(SendParameters.State):
                            errorMessage = ValidateState((string) propertyValue);
                            break;
                        case nameof(SendParameters.FeeType):
                            errorMessage = ValidateFeeType((string) propertyValue);
                            break;
                        case nameof(SendParameters.SendCurr):
                            errorMessage = ValidateCurrency((string) propertyValue);
                            break;
                        case nameof(SendParameters.ServiceOption):
                            errorMessage = ValidateServiceOption((string) propertyValue);
                            break;
                        case nameof(SendReversalParameters.RefundReason):
                            errorMessage = ValidateRefundReason((string)propertyValue);
                            break;
                        case nameof(BillPayParameters.BillerCode):
                            errorMessage = ValidateBillerCode((string)propertyValue);
                            break;
                        case nameof(BillPayParameters.BillerAccountNumber):
                            errorMessage = ValidateBillerAccountNumber((string)propertyValue);
                            break;
                    }
                }

                if (errorMessage != null)
                {
                    transaction.AddValidationError(propertyName, errorMessage);
                }
            }

            transaction.IsEnabled = transaction.IsValid;
        }

        private string ValidateTransactionName(string value)
        {
            if (TransactionProvider.TransactionHandlers.All(x => x.Display != value))
            {
                return "Unsupported transaction name";
            }

            return null;
        }

        private string ValidateEnvironment(string value)
        {
            if (_environmentsList.All(x => x != value))
            {
                return "Unsupported environment";
            }

            return null;
        }

        private string ValidateCountry(string value)
        {
            if (!_countryMap.ContainsKey(value))
            {
                return "Unsupported country code";
            }

            return null;
        }

        private string ValidateAgentState(string value)
        {
            if (_agentStateList.All(x => x != value))
            {
                return "Unsupported agent state code";
            }

            return null;
        }

        private string ValidateAmountRange(string value)
        {
            if (!_amountRangeMap.ContainsKey(value))
            {
                return "Unsupported amount range";
            }

            return null;
        }

        private string ValidateThirdPartyType(string value)
        {
            if (_thirdPartyList.All(x => x != value))
            {
                return "Unsupported third party type";
            }

            return null;
        }

        private string ValidateState(string value)
        {
            if (!string.IsNullOrEmpty(value) && !_subdivisionMap.ContainsKey(value))
            {
                return "Unsupported state code";
            }

            return null;
        }

        private string ValidateFeeType(string value)
        {
            if (!_feeTypeMap.ContainsKey(value))
            {
                return "Unsupported fee type";
            }

            return null;
        }

        private string ValidateCurrency(string value)
        {
            if (!_currencyMap.ContainsKey(value))
            {
                return "Unsupported currency code";
            }

            return null;
        }

        private string ValidateServiceOption(string value)
        {
            if (!_serviceOptionMap.ContainsKey(value))
            {
                return "Unsupported service option";
            }

            return null;
        }

        private string ValidateRefundReason(string value)
        {
            if (_refundReasonList.All(x => x != value))
            {
                return "Unsupported refund reason";
            }

            return null;
        }

        private string ValidateBillerCode(string value)
        {
            if (value.Any(x => !char.IsDigit(x)))
            {
                return "Only digits are allowed";
            }

            return null;
        }

        private string ValidateBillerAccountNumber(string value)
        {
            if (value.Any(x => !char.IsDigit(x)))
            {
                return "Only digits are allowed";
            }

            return null;
        }
    }
}