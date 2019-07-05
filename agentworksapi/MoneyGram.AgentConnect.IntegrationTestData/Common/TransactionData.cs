using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.Common.Json;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.Common
{
    public class TransactionData
    {
        public TransactionData()
        {
            ExecutionOrder = new List<string>();
            BeginTimeStamp = DateTime.Now;
            Errors = new List<BusinessError>();
            GafInfoKeysWithGroups = new Dictionary<string, string>();
        }

        public List<string> ExecutionOrder { get; set; }
        public DateTime? BeginTimeStamp { get; set; }
        public List<BusinessError> Errors { get; }
        public DateTime? EndTimeStamp { get; set; }
        public Dictionary<string, string> GafInfoKeysWithGroups { get; private set; }

        protected void SetExecutionOrder(string name)
        {
            ExecutionOrder.Add(name);
            EndTimeStamp = DateTime.Now;
        }

        public List<string> ToFormattedString(List<string> formattedStringList)
        {
            var lastRequestName = string.Empty;
            IList lastRequestValueList = null;
            DateTime? lastRequestTimeStamp = null;
            DateTime? lastResponseTimeStamp = null;

            foreach(var name in ExecutionOrder)
            {
                var value = GetType().GetProperty(name).GetValue(this);
                var valueType = value.GetType();
                var valueBaseType = valueType.BaseType;
                var valueList = value is IList ? (IList) value : null;
                var valueListBaseType = valueList?.Count > 0 ? valueList[0].GetType().BaseType : null;

                var type = TransactionDataPropertyType.Default;

                if(valueBaseType == typeof(TransactionData))
                {
                    type = TransactionDataPropertyType.TransactionData;
                }
                if(valueBaseType == typeof(Request))
                {
                    type = TransactionDataPropertyType.Request;
                }
                if(valueBaseType == typeof(Response))
                {
                    type = TransactionDataPropertyType.Response;
                }
                if(valueListBaseType == typeof(Request))
                {
                    type = TransactionDataPropertyType.RequestList;
                }
                if(valueListBaseType == typeof(Response))
                {
                    type = TransactionDataPropertyType.ResponseList;
                }

                switch(type)
                {
                    case TransactionDataPropertyType.TransactionData:
                        valueType.GetMethod("ToFormattedString").Invoke(value, new object[] {formattedStringList});
                        break;
                    case TransactionDataPropertyType.Request:
                        lastRequestTimeStamp = GetTimeStamp(value, TransactionDataPropertyType.Request);
                        formattedStringList.Add(GetFormattedRequest(name, value));
                        break;
                    case TransactionDataPropertyType.Response:
                        lastResponseTimeStamp = GetTimeStamp(value, TransactionDataPropertyType.Response);
                        formattedStringList.Add(GetFormattedResponse(name, lastRequestTimeStamp, lastResponseTimeStamp, value));
                        break;
                    case TransactionDataPropertyType.RequestList:
                        lastRequestName = name;
                        lastRequestValueList = valueList;
                        break;
                    case TransactionDataPropertyType.ResponseList:
                        if(lastRequestValueList != null && valueList != null && lastRequestValueList.Count == valueList.Count)
                        {
                            for(var i = 0; i < valueList.Count; i++)
                            {
                                var request = lastRequestValueList[i];
                                lastRequestTimeStamp = GetTimeStamp(request, TransactionDataPropertyType.Request);
                                formattedStringList.Add(GetFormattedRequest($"{lastRequestName} {i + 1}", request));

                                var response = valueList[i];
                                lastResponseTimeStamp = GetTimeStamp(response, TransactionDataPropertyType.Response);
                                formattedStringList.Add(GetFormattedResponse($"{name} {i + 1}", lastRequestTimeStamp, lastResponseTimeStamp, response));
                            }
                        }
                        break;
                    default:
                        formattedStringList.Add(GetFormattedObject(name, value));
                        break;
                }
            }

            return formattedStringList;
        }

        private DateTime GetTimeStamp(object value, TransactionDataPropertyType type)
        {
            switch(type)
            {
                case TransactionDataPropertyType.Request:
                    return ((Request) value).TimeStamp;
                case TransactionDataPropertyType.Response:
                    var payload = value.GetType().GetProperty("Payload")?.GetValue(value);
                    var timeStamp = payload?.GetType().GetProperty("TimeStamp")?.GetValue(payload);
                    if(timeStamp != null)
                    {
                        return (DateTime) timeStamp;
                    }
                    else
                    {
                        return DateTime.Now;
                    }
                default:
                    return DateTime.MinValue;
            }
        }

        private string GetFormattedRequest(string name, object value)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("|--------------------------------------------------");
            stringBuilder.AppendLine($"{name}");
            stringBuilder.AppendLine($"{JsonProcessor.SerializeObject(value, indented: true)}\n");

            return stringBuilder.ToString();
        }

        private string GetFormattedResponse(string name, DateTime? lastRequestTimeStamp, DateTime? lastResponseTimeStamp, object value)
        {
            var stringBuilder = new StringBuilder();
            var timeSpan = lastRequestTimeStamp.HasValue && lastResponseTimeStamp.HasValue ? lastResponseTimeStamp.Value - lastRequestTimeStamp.Value : new TimeSpan();

            stringBuilder.AppendLine($"{name} (finished in {timeSpan.Minutes}min {timeSpan.Seconds}sec {timeSpan.Milliseconds}ms)");
            stringBuilder.AppendLine($"{JsonProcessor.SerializeObject(value, indented: true)}");
            stringBuilder.AppendLine("--------------------------------------------------|\n");

            return stringBuilder.ToString();
        }

        private string GetFormattedObject(string name, object value)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"{name}");
            stringBuilder.AppendLine($"{JsonProcessor.SerializeObject(value, indented: true)}\n");

            return stringBuilder.ToString();
        }

        public void Set(BusinessError businessError)
        {
            var isFirstSet = !Errors.Any();

            Errors.Add(businessError);
            if(isFirstSet)
            {
                SetExecutionOrder(nameof(Errors));
            }
        }

        public void Set(List<BusinessError> businessErrors)
        {
            var isFirstSet = businessErrors.Any() && !Errors.Any();

            Errors.AddRange(businessErrors);
            if(isFirstSet)
            {
                SetExecutionOrder(nameof(Errors));
            }
        }

        public void Set(Dictionary<string, string> gafInfoKeysWithGroups)
        {
            var isFirstSet = gafInfoKeysWithGroups.Any() && !GafInfoKeysWithGroups.Any();

            GafInfoKeysWithGroups = gafInfoKeysWithGroups;
            if(isFirstSet)
            {
                SetExecutionOrder(nameof(GafInfoKeysWithGroups));
            }
        }
    }
}