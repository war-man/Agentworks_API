using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace AwApi.Infrastructure
{
    public class OctetStreamMediaFormatter : JsonMediaTypeFormatter
    {
        public OctetStreamMediaFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/octet-stream"));
        }
    }
}