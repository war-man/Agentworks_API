using PdfSharp.Pdf;
using PdfSharp.Pdf.Advanced;
using PdfSharp.Pdf.IO;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AwApi.Integration
{
    public class ReceiptIntegration : IReceiptIntegration
    {
        public byte[] MergePdf(IEnumerable<byte[]> pdfFiles)
        {
            var outputPdfDocument = new PdfDocument();
            foreach (byte[] pdfFile in pdfFiles)
            {
                if (pdfFile.Count() != 0)
                {
                    var inputPdfDocument = PdfReader.Open(new MemoryStream(pdfFile), PdfDocumentOpenMode.Import);
                    outputPdfDocument.Version = inputPdfDocument.Version;
                    foreach (PdfPage page in inputPdfDocument.Pages)
                    {
                        outputPdfDocument.AddPage(page);
                    }
                }
            }
            if (!pdfFiles.Any())
            {
                return null;
            }
            var outDoc = AddAutoPrint(outputPdfDocument);
            var ms = new MemoryStream();
            outDoc.Save(ms, false);
            return ms.ToArray();
        }

        public PdfDocument AddAutoPrint(PdfDocument outputDocument, bool showPrintDialog = true, int numCopies = 1)
        {
            outputDocument.Info.Author = "author name";

            var JSScript = string.Empty;
            JSScript += "var pp = this.getPrintParams(); ";

            if (numCopies > 0)
            {
                JSScript += "pp.NumCopies = " + numCopies.ToString() + "; ";
            }

            if (!showPrintDialog)
            {
                JSScript += "pp.interactive = pp.constants.interactionLevel.automatic; ";
            }


            JSScript += "this.print({printParams: pp}); ";


            var dictJS = new PdfDictionary();
            dictJS.Elements["/S"] = new PdfName("/JavaScript");
            dictJS.Elements["/JS"] = new PdfStringObject(outputDocument, JSScript);

            outputDocument.Internals.AddObject(dictJS);

            var dict = new PdfDictionary();
            var a = new PdfArray();
            dict.Elements["/Names"] = a;
            a.Elements.Add(new PdfString("EmbeddedJS"));
            a.Elements.Add(PdfInternals.GetReference(dictJS));

            outputDocument.Internals.AddObject(dict);

            var group = new PdfDictionary();
            group.Elements["/JavaScript"] = PdfInternals.GetReference(dict);
            outputDocument.Internals.Catalog.Elements["/Names"] = group;
            return outputDocument;
        }
    }
}