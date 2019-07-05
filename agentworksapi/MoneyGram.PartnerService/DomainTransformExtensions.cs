using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MoneyGram.PartnerService.EntityMapper.DomainModelExtensions;
using MoneyGram.PartnerService.EntityMapper.ServiceModelExtensions;
using SERVICE = MoneyGram.PartnerService.Service;
using DOMAIN = MoneyGram.PartnerService.DomainModel;

namespace MoneyGram.PartnerService
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

        public static List<DOMAIN.UserReportsInfoResponse> ToUserReportsInfoResponseList(this List<SERVICE.UserReportsInfo> response)
        {
            if (response == null)
                return new List<DOMAIN.UserReportsInfoResponse>();

            var returnResponse = new List<DOMAIN.UserReportsInfoResponse>();

            foreach (SERVICE.UserReportsInfo r in response)
            {
                returnResponse.Add(r.ToDomain());
            }
            return returnResponse;
        }

        public static List<SERVICE.AgentPosDevice> ToGetAgentsDeviceNamesRequestList(this List<DOMAIN.AgentPosDevice> request)
        {
            var requestList = new List<SERVICE.AgentPosDevice>();

            foreach (DOMAIN.AgentPosDevice r in request)
            {
                requestList.Add(r.ToService());
            }

            return requestList;
        }

        public static List<DOMAIN.AgentPosDevice> ToAgentPosDeviceList(this List<SERVICE.AgentPosDevice> response)
        {
            if (response == null)
                return new List<DOMAIN.AgentPosDevice>();

            var returnResponse = new List<DOMAIN.AgentPosDevice>();

            foreach (SERVICE.AgentPosDevice r in response)
            {
                returnResponse.Add(r.ToDomain());
            }
            return returnResponse;
        }

        public static List<DOMAIN.Agent> ToAgentList(this List<SERVICE.Agent> response)
        {
            if (response == null)
                return new List<DOMAIN.Agent>();

            var returnResponse = new List<DOMAIN.Agent>();

            foreach (SERVICE.Agent r in response)
            {
                returnResponse.Add(r.ToDomain());
            }
            return returnResponse;
        }

        public static List<DOMAIN.TransactionExceedReportsInfo> ToTransactionExceedInfoResponseList(this List<SERVICE.TransactionExceedReportsInfo> response)
        {
            if (response == null)
                return new List<DOMAIN.TransactionExceedReportsInfo>();

            var returnResponse = new List<DOMAIN.TransactionExceedReportsInfo>();

            foreach (SERVICE.TransactionExceedReportsInfo r in response)
            {
                returnResponse.Add(r.ToDomain());
            }
            return returnResponse;
        }

        public static List<DOMAIN.Agent> ToAgentLocList(this List<SERVICE.Agent> responseList)
        {
            if (responseList == null)
                return new List<DOMAIN.Agent>();

            var returnResponse = new List<DOMAIN.Agent>();

            foreach (SERVICE.Agent r in responseList)
            {
                returnResponse.Add(r.ToDomain());
            }
            return returnResponse;
        }




        public static List<DOMAIN.PointOfSaleDevice> ToPosDeviceList(this List<SERVICE.PointOfSaleDevice> responseList)
        {
            if (responseList == null)
                return new List<DOMAIN.PointOfSaleDevice>();

            var returnResponse = new List<DOMAIN.PointOfSaleDevice>();

            foreach (SERVICE.PointOfSaleDevice r in responseList)
            {
                returnResponse.Add(r.ToDomain());
            }
            return returnResponse;
        }
    }
}