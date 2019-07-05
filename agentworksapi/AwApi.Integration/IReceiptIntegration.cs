using PdfSharp.Pdf;
using System.Collections.Generic;

namespace AwApi.Integration
{
    public interface IReceiptIntegration
    {
        byte[] MergePdf(IEnumerable<byte[]> pdfFiles);

        PdfDocument AddAutoPrint(PdfDocument outputDocument, bool showPrintDialog = true, int numCopies = 1);
    }
}
