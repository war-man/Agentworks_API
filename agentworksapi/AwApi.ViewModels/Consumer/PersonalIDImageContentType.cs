using System.Collections.Generic;

namespace AwApi.ViewModels.Consumer
{
    public class PersonalIDImageContentType
    {
        public string Identifier { get; set; }
        public string PersonalIDChoice { get; set; }
        public string PersonalIDVerificationStr { get; set; }
        public string PersonalIDNumber { get; set; }
        public string MimeType { get; set; }
        public List<ImageItemType> ImageItems { get; set; }
    }
}