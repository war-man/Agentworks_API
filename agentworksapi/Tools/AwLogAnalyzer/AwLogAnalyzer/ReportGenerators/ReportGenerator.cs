using System;
using System.Collections.Generic;
using AwLogAnalyzer.Models;
using Microsoft.Office.Interop.Excel;
using System.Linq;
using System.IO;

namespace AwLogAnalyzer.ReportGenerators
{
    public abstract class ReportGenerator
    {
        public abstract string CreateReport(IDictionary<string, List<Record>> logFileResults, Action<string> onUpdate);

        public abstract void FormatReport(Worksheet worksheet);

        protected static double GetMiddlePercentage(IList<int> values, int percentile)
        {
            if (values.Any())
            {
                var percentage = (double)percentile / 100;
                var numResultsToSelect = (int)Math.Floor(values.Count * percentage);

                var resultsToPrune = values.Count - numResultsToSelect;
                resultsToPrune = resultsToPrune - resultsToPrune % 2;

                var orderedValues = values.OrderBy(x => x);
                var prunedValues = orderedValues.Select((val, index) =>
                {
                    if (index < resultsToPrune / 2 || index > numResultsToSelect + resultsToPrune / 2)
                    {
                        return -1;
                    }

                    return val;
                });
                return prunedValues.Where(x => x != -1).Average();
            }
            return 0.0;
        }

        protected static double StandardDeviation(List<int> values)
        {
            if (values.Any())
            {
                var average = values.Average();

                var sumOfSquares = values.Sum(x => (x - average) * (x - average));

                return Math.Sqrt(sumOfSquares / values.Count);
            }

            return 0;
        }

        protected static double AverageOfSigma(IList<int> values, double standardDeviation, int sigma)
        {
            var average = 0.0;
            var variance = 0.0;
            var minValue = 0.0;
            var maxValue = 0.0;
            if (values.Any())
            {
                average = values.Average();
                variance = sigma * standardDeviation;
                minValue = average - variance;
                maxValue = average + variance;
            }
            return values.Any() ? values.Where(x => x >= minValue && x <= maxValue).Average() : 0.0;
        }

        protected static List<List<Record>> FilterOutEndTimes(List<List<Record>> records)
        {
            foreach (var recordList in records)
            {
                foreach (var innerRecord in recordList)
                {
                    if (innerRecord == null)
                    {
                        continue;
                    }

                    if (innerRecord.IsEndRequest)
                    {
                        if (recordList.Any(x => x.IsBeginRequest))
                        {
                            recordList.First(x => x.IsBeginRequest).TotalElapsedMilliseconds =
                                innerRecord.TotalElapsedMilliseconds;
                        }
                    }
                }
            }
            // remove all EndRequests..
            var result = records.Select(x => x.Where(y => y.IsEndRequest == false).ToList()).ToList();
            return result;
        }

        protected static string CreateTempFile(string fileName, string fileExtension = "csv")
        {
            return $"{Path.GetTempPath()}{fileName}.{Guid.NewGuid().ToString()}.{fileExtension}";
        }
    }
}