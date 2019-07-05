using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AwLogAnalyzer.Models;
using Microsoft.Office.Interop.Excel;

namespace AwLogAnalyzer.ReportGenerators
{
    class ApiOverviewReportGenerator : ReportGenerator
    {
        public override string CreateReport(IDictionary<string, List<Record>> logFileResults, Action<string> onUpdate)
        {
            onUpdate($"Creating Overview");
            var fileName = CreateTempFile("Timing Overview");
            var stringBuilder = new StringBuilder();

            var headers = new[]
            {
                "Api Name",
                "Number of Calls",
                "Avg. Elapsed Time (ms)",
                "Standard Deviation (ms)",
                "Avg. 1 Sigma (65%) (ms)",
                "Avg. 2 Sigma (95%) (ms)",
                "Avg. 3 Sigma (99.7%) (ms)",
                "80% of Response Times (ms)",
                "90% of Response Times (ms)",
                "Max Response Time (ms)",
                "Min Response Time (ms)"
            };

            stringBuilder.AppendLine(string.Join(",", headers));

            // Populate results
            var listResults = logFileResults.Select(x => x.Value).ToList();
            // Filter out the end times and apply total elapsed time to begin time records.
            var totalElapsedListResults = FilterOutEndTimes(listResults);
            var combinedRecordsList = totalElapsedListResults.SelectMany(x => x).ToList();
            var apiCallsList = combinedRecordsList
                .Where(x => x.ApiName != null)
                .GroupBy(x => x.ApiName);
            var resultsList = apiCallsList
                .Select(x =>
                    new
                    {
                        ApiName = x.First().ApiName,
                        NumberOfCalls = x.Count(),
                        ElapsedTimesList = x.Select(g => g.TotalElapsedMilliseconds)
                    });

            foreach (var result in resultsList)
            {
                var elapsedTimeList = result.ElapsedTimesList.ToList();
                var standardDeviation = StandardDeviation(elapsedTimeList);

                var rowValues = new dynamic[]
                {
                    result.ApiName,
                    result.NumberOfCalls,
                    result.ElapsedTimesList.Any() ? (int) result.ElapsedTimesList.Average() : 0,
                    (int) standardDeviation,
                    (int) AverageOfSigma(elapsedTimeList, standardDeviation, 1),
                    (int) AverageOfSigma(elapsedTimeList, standardDeviation, 2),
                    (int) AverageOfSigma(elapsedTimeList, standardDeviation, 3),
                    (int) GetMiddlePercentage(elapsedTimeList, 80),
                    (int) GetMiddlePercentage(elapsedTimeList, 90),
                    elapsedTimeList.Any() ? elapsedTimeList.Max() : 0,
                    elapsedTimeList.Any() ? elapsedTimeList.Min() : 0
                };

                stringBuilder.AppendLine(string.Join(",", rowValues.Select(val => val.ToString())));
            }

            var fileText = stringBuilder.ToString();
            File.WriteAllText(fileName, fileText);

            return fileName;
        }

        public override void FormatReport(Worksheet worksheet)
        {
            worksheet.Name = "API Overview";
            worksheet.Columns.ColumnWidth = 25;
            worksheet.Columns[1].ColumnWidth = 75;
        }
    }
}
