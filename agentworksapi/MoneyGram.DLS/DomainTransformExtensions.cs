using System.Collections.Generic;
using System.Linq;
using SERVICE = MoneyGram.DLS.Service;
using DOMAIN = MoneyGram.DLS.DomainModel;
using System.Reflection;
using MoneyGram.DLS.EntityMapper.ServiceModelExtensions;

namespace MoneyGram.DLS
{
    public static class DomainTransformExtensions
    {

        public static void NullifyWhiteSpaceStrings(object obj, IEnumerable<string> propertyExemptions = null)
        {
            if (obj == null)
            {
                return;
            }

            if (propertyExemptions == null)
            {
                propertyExemptions = Enumerable.Empty<string>();
            }
            var type = obj.GetType();
            var allProperties = type.GetProperties();
            foreach (PropertyInfo prop in allProperties)
            {
                object propValue = prop.GetValue(obj, null);
                if (propValue == null)
                {
                    continue;
                }
                if (prop.PropertyType.Assembly == type.Assembly && !prop.PropertyType.IsEnum)
                {
                    NullifyWhiteSpaceStrings(propValue, null);
                }
                else if (prop.PropertyType.FullName == "System.String")
                {
                    if (prop.SetMethod == null)
                    {
                        continue;
                    }
                    var stringValue = (string)propValue;
                    if (!propertyExemptions.Contains(prop.Name) && string.IsNullOrWhiteSpace(stringValue))
                    {
                        prop.SetValue(obj, null);
                    }
                }
            }
        }

        public static List<DOMAIN.TransactionDetailLookupResult> ToResponseList(this List<SERVICE.TransactionDetailLookupResult> response)
        {
            if (response == null)
            {
                return new List<DOMAIN.TransactionDetailLookupResult>();
            }

            var returnResponse = new List<DOMAIN.TransactionDetailLookupResult>();

            foreach (SERVICE.TransactionDetailLookupResult r in response)
            {
                returnResponse.Add(r.ToDomain());
            }
            return returnResponse;
        }


    }
}
