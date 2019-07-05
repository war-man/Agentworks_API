using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AwLogAnalyzer.Models;
using Microsoft.Office.Interop.Excel;

namespace AwLogAnalyzer.ReportGenerators
{
    public class RequestDetailsReportGenerator : ReportGenerator
    {
        public override string CreateReport(IDictionary<string, List<Record>> logFileResults, Action<string> onUpdate)
        {
            onUpdate($"Creating Details");
            var fileName = CreateTempFile("Timing Details");
            var stringBuilder = new StringBuilder();

            var headers = new[]
            {
                "Application Name",
                "Transaction ID",
                "API Name",
                "Action",
                "Elapsed Time (ms)",
                "User",
                "IP"
            };

            stringBuilder.AppendLine(string.Join(",", headers));

            var listResults = logFileResults.Select(x => x.Value).ToList();
            // Filter out the end times and apply total elapsed time to begin time records.
            var totalElapsedListResults = FilterOutEndTimes(listResults);

            for (var i = 0; i < totalElapsedListResults.Count; i++)
            {
                var percentComplete = ((decimal)i / totalElapsedListResults.Count) * 100;
                onUpdate($"Creating Details: {(int)percentComplete}% Complete");

                var result = totalElapsedListResults[i].OrderBy(x => x.DateStamp);
                foreach (var innerRecord in result)
                {
                    if (innerRecord.IsEndRequest)
                    {
                        continue;
                    }

                    var currentRow = new List<string>
                    {
                        innerRecord.ApplicationName,
                        innerRecord.transactionId,
                        innerRecord.ApiName
                    };

                    if (innerRecord.IsSoap)
                    {
                        currentRow.Add(innerRecord.SoapCall);
                    }
                    else
                    {
                        currentRow.Add(innerRecord.MethodAction);
                    }

                    currentRow.Add(innerRecord.ElapsedMilliseconds != 0
                        ? innerRecord.ElapsedMilliseconds.ToString()
                        : innerRecord.TotalElapsedMilliseconds.ToString());

                    currentRow.Add(innerRecord.UserName);
                    currentRow.Add(innerRecord.ServerIp);

                    stringBuilder.AppendLine(string.Join(",", currentRow));
                }

                // add a blank line
                stringBuilder.AppendLine();
            }

            var fileText = stringBuilder.ToString();
            File.WriteAllText(fileName, fileText);

            return fileName;
        }

        public override void FormatReport(Worksheet worksheet)
        {
            worksheet.Name = "Request Details";
            worksheet.Columns.ColumnWidth = 20;
            worksheet.Columns[2].ColumnWidth = 40;
            worksheet.Columns[3].ColumnWidth = 40;
            worksheet.Columns[4].ColumnWidth = 40;
        }
    }
}