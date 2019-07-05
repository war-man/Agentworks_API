using System;
using System.Collections;
using System.Linq;

namespace MoneyGram.Common.Extensions
{
    public static class ValidationExtensions
    {
        public static void ThrowIfNullOrEmpty(this string param, string paramName)
        {
            if(string.IsNullOrEmpty(param))
            {
                throw new ArgumentException(paramName);
            }
        }

        public static void ThrowIfNullOrEmpty(this IDictionary param, string paramName)
        {
            if(param == null || param.Count == 0)
            {
                throw new ArgumentException(paramName);
            }
        }

        /// <summary>
        /// Throws an ArgumentNullException if the input parameter is null.
        /// </summary>
        public static void ThrowIfNull(this object param, string paramName)
        {
            if (param == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        /// <summary>
        /// Throws an ArgumentNullException if ANY of the input parameters are null.
        /// </summary>
        public static void ThrowIfNull(params object[] p)
        {
            foreach (var param in p.ToList())
            {
                var name = param.GetType().ToString().Split('.').LastOrDefault();
                param.ThrowIfNull(name);
            }
        }
    }
}
