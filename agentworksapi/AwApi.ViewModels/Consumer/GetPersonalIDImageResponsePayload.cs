using System.Collections.Generic;

namespace AwApi.ViewModels.Consumer
{
    public class GetPersonalIDImageResponsePayload
    {
        public string MimeType { get; set; }
        public List<ImageItemType> ImageItems { get; set; }
    }
}