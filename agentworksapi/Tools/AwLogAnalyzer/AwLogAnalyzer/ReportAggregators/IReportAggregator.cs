using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AwLogAnalyzer.Models;

namespace AwLogAnalyzer.ReportAggregators
{
    public interface IReportAggregator
    {
        Task AggregateReport(IDictionary<string, List<Record>> logFileResults, Action<string> onUpdate);
    }
}