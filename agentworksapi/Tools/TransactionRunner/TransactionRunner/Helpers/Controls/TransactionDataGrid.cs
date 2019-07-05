using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using TransactionRunner.ImportExport;
using TransactionRunner.Transactions;

namespace TransactionRunner.Helpers.Controls
{
    /// <summary>
    /// DataGrid with customized column generation (for transaction list in Import window)
    /// </summary>
    public class TransactionDataGrid : DataGrid
    {
        private readonly IList<DataGridColumn> _generatedColumns = new List<DataGridColumn>();

        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);
            
            foreach (var column in _generatedColumns)
            {
                Columns.Remove(column);
            }
            _generatedColumns.Clear();

            IList<BaseParams> list = (IList<BaseParams>)newValue;
            BaseParams firstItem = list.FirstOrDefault();
            if (firstItem != null)
            {
                var properties = firstItem.GetPropertiesToExport();
                foreach (PropertyInfo property in properties)
                {
                    var attribute = property.GetCustomAttribute<ExportableAttribute>();

                    DataGridColumn column = null;
                    if (property.PropertyType == typeof(string) || property.PropertyType == typeof(decimal) || property.PropertyType == typeof(bool))
                    {
                        column = new DataGridTextColumn
                        {
                            Header = attribute.ColumnHeader,
                            Binding = new Binding(property.Name) { ValidatesOnDataErrors = true },
                            ElementStyle = (Style)FindResource("errorStyle")
                        };
                    }

                    if (column != null)
                    {
                        _generatedColumns.Add(column);
                        Columns.Add(column);
                    }
                }
            }
        }
    }
}