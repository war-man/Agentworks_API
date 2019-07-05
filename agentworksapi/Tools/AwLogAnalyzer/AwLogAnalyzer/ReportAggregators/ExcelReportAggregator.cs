using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AwLogAnalyzer.Models;
using AwLogAnalyzer.ReportGenerators;
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace AwLogAnalyzer.ReportAggregators
{
    public class ExcelReportAggregator : IReportAggregator
    {
        private readonly ReportGenerator apiOverviewReportGenerator;
        private readonly ReportGenerator soapOverviewReportGenerator;
        private readonly ReportGenerator requestDetailsReportGenerator;

        public ExcelReportAggregator()
        {
            apiOverviewReportGenerator = new ApiOverviewReportGenerator();
            soapOverviewReportGenerator = new SoapOverviewReportGenerator();
            requestDetailsReportGenerator = new RequestDetailsReportGenerator();
        }

        public Task AggregateReport(IDictionary<string, List<Record>> logFileResults,
            Action<string> onUpdate)
        {
            return Task.Run(() =>
            {
                var excelApp = new Application();

                if (excelApp == null)
                {
                    throw new Exception("Unable to start Excel");
                }

                var workbooks = excelApp.Workbooks;
                var workbook = workbooks.Add();

                var overviewFileName = apiOverviewReportGenerator.CreateReport(logFileResults, onUpdate);
                var soapOverviewFileName = soapOverviewReportGenerator.CreateReport(logFileResults, onUpdate);
                var detailsFileName = requestDetailsReportGenerator.CreateReport(logFileResults, onUpdate);

                // Open each of the CSVs
                var overviewWorkbook = OpenFile(excelApp, overviewFileName);
                var soapWorkbook = OpenFile(excelApp, soapOverviewFileName);
                var detailsWorkbook = OpenFile(excelApp, detailsFileName);

                // Copy data from the individual workbooks to the report workbook
                overviewWorkbook.Worksheets[1].Copy(workbook.Worksheets[1]);
                soapWorkbook.Worksheets[1].Copy(workbook.Worksheets[2]);
                detailsWorkbook.Worksheets[1].Copy(workbook.Worksheets[3]);

                // Format each worksheet appropriately
                apiOverviewReportGenerator.FormatReport(workbook.Worksheets[1]);
                soapOverviewReportGenerator.FormatReport(workbook.Worksheets[2]);
                requestDetailsReportGenerator.FormatReport(workbook.Worksheets[3]);

                RemoveExtraWorksheet(workbook);

                // Close the individual workbooks
                overviewWorkbook.Close(false);
                soapWorkbook.Close(false);
                detailsWorkbook.Close(false);
                
                excelApp.Visible = true;

                Marshal.ReleaseComObject(workbook);
                Marshal.ReleaseComObject(workbooks);
                Marshal.ReleaseComObject(excelApp);
            });
        }

        private static Workbook OpenFile(Application excelApp, string fileName, string delimeter = ",")
        {
            return excelApp.Workbooks.Open(
                fileName,
                Type.Missing,
                Type.Missing,
                XlFileFormat.xlCSV, // Format
                Type.Missing,
                Type.Missing,
                Type.Missing,
                Type.Missing,
                delimeter,
                Type.Missing,
                Type.Missing,
                Type.Missing,
                Type.Missing,
                Type.Missing,
                Type.Missing
            );
        }

        private static void RemoveExtraWorksheet(Workbook workbook)
        {
            var extraWorksheetIndex = workbook.Worksheets.Count;

            workbook.Activate();
            workbook.Worksheets[extraWorksheetIndex].Activate();
            workbook.Worksheets[extraWorksheetIndex].Delete();
            workbook.Worksheets[1].Activate();
        }
    }
}