using System;
using System.Linq;
using System.Reflection;

namespace MoneyGram.Common
{
    public static class ServiceTransformExtensions
    {
        public static void ProcessSpecifiedDomainToService(object svc, object dm)
        {
            var searchKey = "specified";
            if (svc == null || dm == null)
            {
                return;
            }

            var serviceModelType = svc.GetType();
            var domainModelType = dm.GetType();
            var allServiceModelProperties = serviceModelType.GetProperties();
            var allDomainModelProperties = domainModelType.GetProperties();
            var listOfPropertiesWithAssociatedSpecified = allServiceModelProperties.Where(x=>x.Name.ToLower().EndsWith(searchKey)).Select(y=> y.Name.Remove(y.Name.Length - searchKey.Length, searchKey.Length)).ToList();
            var listOfSpecifiedProperties = allServiceModelProperties.Where(x => x.Name.ToLower().EndsWith(searchKey)).Select(y => y.Name).ToList();
            // Looking for specified properties
            foreach (PropertyInfo prop in allDomainModelProperties)
            {
                object propValue = prop.GetValue(dm, null);
                if (listOfPropertiesWithAssociatedSpecified.Contains(prop.Name, StringComparer.OrdinalIgnoreCase))
                {
                    // Assign service object value
                    foreach (var serviceProp in allServiceModelProperties)
                    {
                        if (!serviceProp.Name.Equals(prop.Name, StringComparison.InvariantCultureIgnoreCase))
                        {
                            continue;
                        }
                        if (propValue != null)
                        {
                            serviceProp.SetValue(svc, propValue);
                        }
                        else
                        {
                            Type propertyType = serviceProp.GetType();
                            serviceProp.SetValue(svc, DefaultGenerator.GetDefaultValue(propertyType));
                        }
                    }
                    // Assign service object specified value
                    foreach (var serviceProp in allServiceModelProperties)
                    {
                        if (!listOfSpecifiedProperties.Contains(serviceProp.Name, StringComparer.OrdinalIgnoreCase))
                        {
                            continue;
                        }
                        var actualProperty = allDomainModelProperties.First(x => x.Name.Equals(serviceProp.Name.Remove(serviceProp.Name.Length - searchKey.Length, searchKey.Length), StringComparison.InvariantCultureIgnoreCase));
                        var actualPropertyValue = actualProperty.GetValue(dm, null);
                        if (actualPropertyValue != null)
                        {
                            serviceProp.SetValue(svc, true);
                        }
                        else
                        {
                            serviceProp.SetValue(svc, false);
                        }
                    }
                }
            }
        }
    }
}