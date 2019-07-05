using System;
using System.Collections.Generic;

namespace AwApi.ViewModels.Consumer
{
    public class ConsumerProfileDocumentContentType
    {
        public string Identifier { get; set; }
        public DateTime DocumentIssueDate { get; set; }
        public string MimeType { get; set; }
        public List<ImageItemType> ImageItems { get; set; }
    }
}
