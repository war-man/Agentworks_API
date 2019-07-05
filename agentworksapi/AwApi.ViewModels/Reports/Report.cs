using System;
using System.Collections.Generic;

namespace AwApi.ViewModels.Reports
{
    [Serializable]
    public class Report
    {
        public string Name { get; set; }
        public List<string> ColumnHeaders { get; set; } = new List<string>();
        public List<List<string>> Rows { get; set; } = new List<List<string>>();
    }
}