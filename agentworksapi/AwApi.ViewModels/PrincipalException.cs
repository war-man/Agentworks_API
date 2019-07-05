using System;
using System.Net;

namespace AwApi.ViewModels
{
    [Serializable]
    public class PrincipalException: Exception
    {   

        public PrincipalException(string message, Exception innerException = null)
            : base(message, innerException)
        {
            ErrorMessage = message;
            StatusCode = HttpStatusCode.BadRequest;
        }
        public string ErrorMessage { get; set; }
        public int Code { get; set; }
        // * IMPORTANT * InternalDetails property is for server side logging only, it may contain sensitive information and should
        // NEVER be mapped to the vm.
        public string InternalDetails { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}