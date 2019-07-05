using System;
using System.Collections.Generic;

namespace AwApi.ViewModels
{
    [Serializable]
    public class ApiResponse<T,U> where U: ApiData
    {
        public T ResponseData { get; set; }
        public U AdditionalData { get; set; }
        public BusinessMetadata BusinessMetadata { get; set; }
        public Dictionary<string, string> ApiErrors { get; set; }
    }
}