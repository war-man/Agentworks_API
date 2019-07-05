using MoneyGram.AgentConnect.DomainModel.Transaction;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace MoneyGram.AgentConnect.IntegrationTest.Operations.Helpers
{
    public static class DataErrorHandler
    {
        public static List<BusinessError> CheckForNestedErrors(Object data)
        {
            List<BusinessError> found = FindAllInstances<BusinessError>(data);
            return found;
        }

        private static List<T> FindAllInstances<T>(object value) where T : class
        {

            HashSet<object> exploredObjects = new HashSet<object>();
            List<T> found = new List<T>();

            FindAllInstances(value, exploredObjects, found);

            return found;
        }

        private static void FindAllInstances<T>(object value, HashSet<object> exploredObjects, List<T> found) where T : class
        {
            if (value == null)
                return;

            if (exploredObjects.Contains(value))
                return;

            exploredObjects.Add(value);

            IEnumerable enumerable = value as IEnumerable;

            if (enumerable != null)
            {
                foreach (object item in enumerable)
                {
                    FindAllInstances<T>(item, exploredObjects, found);
                }
            }
            else
            {
                T possibleMatch = value as T;

                if (possibleMatch != null)
                {
                    found.Add(possibleMatch);
                }

                Type type = value.GetType();

                PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetProperty);

                foreach (PropertyInfo property in properties)
                {
                    object propertyValue = property.GetValue(value, null);

                    FindAllInstances<T>(propertyValue, exploredObjects, found);
                }

            }
        }
    }
}
