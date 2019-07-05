using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AwLogAnalyzer.Models;

namespace AwLogAnalyzer.LogParsers
{
    public interface ILogParser
    {
        Task<Dictionary<string, List<Record>>> ParseLogFile(string fileNames, Action<string> onUpdate,
            DateTime? startTime = null, DateTime? endTime = null);
    }
}