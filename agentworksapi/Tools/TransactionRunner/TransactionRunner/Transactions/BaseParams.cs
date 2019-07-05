using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using TransactionRunner.ImportExport;

namespace TransactionRunner.Transactions
{
    /// <summary>
    /// Base class for transaction parameters
    /// </summary>
    [XmlType("Transaction")]
    public abstract class BaseParams : IDataErrorInfo, INotifyPropertyChanged
    {
        /// <summary>
        ///     Returns transaction name
        /// </summary>
        [Exportable(ElementName = "Transaction_Name", ColumnHeader = "Transaction Name", Order = 1)]
        [XmlIgnore]
        public abstract string TransactionName { get; }

        /// <summary>
        ///     Gets or sets the Environment parameter.
        /// </summary>
        [Exportable(ElementName = "Environment", ColumnHeader = "Environment", Order = 2)]
        public string Environment { get; set; }

        /// <summary>
        ///     Gets or sets the Agent Country Iso Code parameter.
        /// </summary>
        [Exportable(ElementName = "Agent_Country", ColumnHeader = "Agent Country", Order = 3)]
        public string AgentCountryIsoCode { get; set; }

        /// <summary>
        ///     Gets or sets the Agent Id parameter.
        /// </summary>
        [Exportable(ElementName = "Agent_ID", ColumnHeader = "Agent ID", Order = 4)]
        public string AgentId { get; set; }

        /// <summary>
        ///     Get or sets the Agent Pos parameter.
        /// </summary>
        [Exportable(ElementName = "Agent_POS", ColumnHeader = "Agent POS", Order = 5)]
        public string AgentPos { get; set; }

        /// <summary>
        ///     Gets or sets the Agent State parameter.
        /// </summary>
        [Exportable(ElementName = "Agent_State", ColumnHeader = "Agent State", Order = 6)]
        public string AgentState { get; set; }

        /// <summary>
        ///     Gets or set the Amt Range parameter.
        /// </summary>
        [Exportable(ElementName = "Amount_Range", ColumnHeader = "Amount Range", Order = 7)]
        public string AmtRange { get; set; }

        /// <summary>
        ///     Get or sets the Custom Amount parameter.
        /// </summary>
        [Exportable(ElementName = "Custom_Amount", ColumnHeader = "Custom Amount", Order = 8)]
        public decimal CustomAmount { get; set; }

        /// <summary>
        ///     Gets or sets the Third Party Type parameter.
        /// </summary>
        [Exportable(ElementName = "Third_Party_Type", ColumnHeader = "Third Party Type", Order = 9)]
        public string ThirdPartyType { get; set; }

        /// <summary>
        /// Clones object
        /// </summary>
        /// <returns></returns>
        public BaseParams Clone()
        {
            return (BaseParams)MemberwiseClone();
        }

        /// <summary>
        /// Gets list of properties that are supported by import/export, in correct order
        /// </summary>
        /// <returns></returns>
        public IList<PropertyInfo> GetPropertiesToExport()
        {
            return GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty)
                .Where(x => x.GetCustomAttribute<XmlElementAttribute>() != null)
                .Select(x => new { Property = x, XmlElementAttr = x.GetCustomAttribute<ExportableAttribute>() })
                .OrderBy(x => x.XmlElementAttr.Order)
                .Select(x => x.Property)
                .ToList();
        }

        /// <summary>
        /// Returns Agent State code for building requests
        /// </summary>
        /// <returns></returns>
        public string GetAgentState()
        {
            if (AgentState != null)
            {
                return AgentState.Contains("-") ? AgentState.Split('-')[1] : AgentState;
            }

            return null;
        }

        /// <summary>
        /// Returns error for property name. IDataError info member
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public string this[string columnName]
        {
            get
            {
                if (_errors.ContainsKey(columnName))
                {
                    return string.Join(System.Environment.NewLine, _errors[columnName]);
                }

                return null;
            }
        }

        /// <summary>
        /// IDataError info member
        /// </summary>
        public string Error { get; }

        private readonly Dictionary<string, IList<string>> _errors = new Dictionary<string, IList<string>>();
        private bool _isEnabled;
        private string _referenceNumber;
        private bool _isBusy;
        private string _resultFilePath;
        private bool _isSelectable;
        private bool _showResultText;

        /// <summary>
        /// Returns validation status
        /// </summary>
        public bool IsValid => !_errors.Any();

        /// <summary>
        /// True when transaction is enabled for the batch
        /// </summary>
        [XmlIgnore]
        public bool IsEnabled
        {
            get => _isEnabled && IsValid;
            set
            {
                _isEnabled = value;
                OnPropertyChanged(nameof(IsEnabled));
            }
        }

        /// <summary>
        /// Transaction id in the batch
        /// </summary>
        [XmlIgnore]
        public Guid TransactionGuid { get; set; }

        /// <summary>
        /// Reference number
        /// </summary>
        [XmlIgnore]
        public string ReferenceNumber
        {
            get => _referenceNumber;
            set
            {
                _referenceNumber = value;
                OnPropertyChanged(nameof(ReferenceNumber));
            }
        }

        /// <summary>
        /// True when transaction is currently processed
        /// </summary>
        [XmlIgnore]
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
                OnPropertyChanged(nameof(ShowResultLink));
            }
        }

        /// <summary>
        /// File name for transaction results
        /// </summary>
        [XmlIgnore]
        public string ResultFilePath
        {
            get => _resultFilePath;
            set
            {
                _resultFilePath = value;
                OnPropertyChanged(nameof(ResultFilePath));
                OnPropertyChanged(nameof(ShowResultLink));
            }
        }

        /// <summary>
        /// True when result for the row should be displayed and clickable
        /// </summary>
        public bool ShowResultLink => !IsBusy && IsValid && !string.IsNullOrEmpty(ResultFilePath);

        /// <summary>
        /// True when result for the row should be displayed and NOT clickable
        /// </summary>
        [XmlIgnore]
        public bool ShowResultText
        {
            get => _showResultText;
            set
            {
                _showResultText = value;
                OnPropertyChanged(nameof(ShowResultText));
            }
        }

        /// <summary>
        /// True when item on the import list can be selected for transaction batch
        /// </summary>
        [XmlIgnore]
        public bool IsSelectable
        {
            get => _isSelectable && IsValid;
            set
            {
                _isSelectable = value;
                OnPropertyChanged(nameof(IsSelectable));
            }
        }

        /// <summary>
        /// Adds error for property
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="errorMessage"></param>
        public void AddValidationError(string propertyName, string errorMessage)
        {
            if (!_errors.ContainsKey(propertyName))
            {
                _errors[propertyName] = new List<string>();
            }

            _errors[propertyName].Add(errorMessage);
        }

        /// <summary>
        /// PropertyChanged event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises PropertyChanged event
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}