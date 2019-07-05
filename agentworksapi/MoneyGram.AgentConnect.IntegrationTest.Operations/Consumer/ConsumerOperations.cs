using System;
using System.Collections.Generic;
using System.Linq;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data;
using MoneyGram.AgentConnect.IntegrationTest.Data.Consumer;
using MoneyGram.AgentConnect.IntegrationTest.Data.Extensions;
using MoneyGram.AgentConnect.IntegrationTest.Data.Setup;
using MoneyGram.AgentConnect.IntegrationTest.Operations.Common;

namespace MoneyGram.AgentConnect.IntegrationTest.Operations.Consumer
{
    public class ConsumerOperations
    {
        private TestAgentConnectIntegration _acIntegration { get; }
        private ValidationOperations _validationOperations { get; }

        public ConsumerOperations(TestRunner testRunner)
        {
            _acIntegration = new TestAgentConnectIntegration(testRunner);
            _validationOperations = new ValidationOperations(testRunner);
        }

        public CreateOrUpdateProfileConsumerResponse UpdateProfileConsumerData(CreateOrUpdateProfileConsumerData data, List<KeyValuePairType> fieldValues, string consumerProfileId, string consumerProfileTypeId)
        {
            data.CreateOrUpdateProfileConsumerRequest = CreateOrUpdateProfileConsumerRequestFactory.NewRequestWithBaseData();
            data.CreateOrUpdateProfileConsumerRequest.ConsumerProfileID = consumerProfileId;
            data.CreateOrUpdateProfileConsumerRequest.ConsumerProfileIDType = consumerProfileTypeId;
            data.CreateOrUpdateProfileConsumerRequest.ConsumerProfileIDTypeToReturn = consumerProfileTypeId;
            data.CreateOrUpdateProfileConsumerRequest.FieldValues.AddRange(fieldValues);            
            return _acIntegration.CreateOrUpdateProfileConsumer(data);
        }

        public CreateOrUpdateProfileConsumerResponse CreateProfileConsumer(
            CreateOrUpdateProfileConsumerData data)
        {
            var validationResponse = CreateOrUpdateProfileConsumerBaseData(data);
            return _validationOperations.CreateOrUpdateProfileConsumerValidate(data, validationResponse.Payload?.FieldsToCollect);            
        }

        public CreateOrUpdateProfileConsumerResponse CreateOrUpdateProfileConsumerBaseData(CreateOrUpdateProfileConsumerData data)
        {
            data.CreateOrUpdateProfileConsumerRequest = CreateOrUpdateProfileConsumerRequestFactory.NewRequestWithBaseData();
            return _acIntegration.CreateOrUpdateProfileConsumer(data);
        }

        public GetProfileConsumerResponse GetProfileConsumer(GetProfileConsumerData data)
        {
            data.GetProfileConsumerRequest = GetProfileConsumerRequestFactory.NewGetProfileConsumerRequest();
            return _acIntegration.GetProfileConsumer(data);
        }

        public CreateOrUpdateProfileSenderResponse UpdateProfileSenderData(CreateOrUpdateProfileSenderData data, List<KeyValuePairType> fieldValues, string senderProfileId, string senderProfileTypeId)
        {
            data.CreateOrUpdateProfileSenderRequest = CreateOrUpdateProfileSenderRequestFactory.NewRequestWithBaseData();
            data.CreateOrUpdateProfileSenderRequest.ConsumerProfileID = senderProfileId;
            data.CreateOrUpdateProfileSenderRequest.ConsumerProfileIDType = senderProfileTypeId;
            data.CreateOrUpdateProfileSenderRequest.FieldValues.AddRange(fieldValues);
            return _acIntegration.CreateOrUpdateProfileSender(data);
        }

        public CreateOrUpdateProfileSenderResponse CreateProfileSender(
            CreateOrUpdateProfileSenderData data)
        {
            var validationResponse = CreateOrUpdateProfileSenderBaseData(data);
            return _validationOperations.CreateOrUpdateProfileSenderValidate(data, validationResponse.Payload?.FieldsToCollect);
        }

        public CreateOrUpdateProfileSenderResponse CreateOrUpdateProfileSenderBaseData(CreateOrUpdateProfileSenderData data)
        {
            data.CreateOrUpdateProfileSenderRequest = CreateOrUpdateProfileSenderRequestFactory.NewRequestWithBaseData();
            return _acIntegration.CreateOrUpdateProfileSender(data);
        }

        public GetProfileSenderResponse GetProfileSender(GetProfileSenderData data)
        {
            data.GetProfileSenderRequest = GetProfileSenderRequestFactory.NewGetProfileSenderRequest();
            return _acIntegration.GetProfileSender(data);
        }

        public CreateOrUpdateProfileReceiverResponse UpdateProfileReceiverData(CreateOrUpdateProfileReceiverData data, List<KeyValuePairType> fieldValues, string receiverProfileId, string receiverProfileTypeId)
        {
            data.CreateOrUpdateProfileReceiverRequest = CreateOrUpdateProfileReceiverRequestFactory.NewRequestWithBaseData();
            data.CreateOrUpdateProfileReceiverRequest.ConsumerProfileID = receiverProfileId;
            data.CreateOrUpdateProfileReceiverRequest.ConsumerProfileIDType = receiverProfileTypeId;
            data.CreateOrUpdateProfileReceiverRequest.FieldValues.AddRange(fieldValues);
            return _acIntegration.CreateOrUpdateProfileReceiver(data);
        }

        public CreateOrUpdateProfileReceiverResponse CreateProfileReceiver(
            CreateOrUpdateProfileReceiverData data)
        {
            var validationResponse = CreateOrUpdateProfileReceiverBaseData(data);
            return _validationOperations.CreateOrUpdateProfileReceiverValidate(data, validationResponse.Payload?.FieldsToCollect);
        }

        public CreateOrUpdateProfileReceiverResponse CreateOrUpdateProfileReceiverBaseData(CreateOrUpdateProfileReceiverData data)
        {
            data.CreateOrUpdateProfileReceiverRequest = CreateOrUpdateProfileReceiverRequestFactory.NewRequestWithBaseData();
            return _acIntegration.CreateOrUpdateProfileReceiver(data);
        }

        public GetProfileReceiverResponse GetProfileReceiver(GetProfileReceiverData data)
        {
            data.GetProfileReceiverRequest = GetProfileReceiverRequestFactory.NewGetProfileReceiverRequest();
            return _acIntegration.GetProfileReceiver(data);
        }

        public string GetCityAddressOfConsumer(GetProfileConsumerResponse consumerProfileResponse)
        {
            return consumerProfileResponse.Payload.CurrentValues
                .FirstOrDefault(x => x.InfoKey == InfoKeyNames.consumer_City).Value;
        }

        public string SetRandomAddressCityOfConsumer(List<KeyValuePairType> values)
        {
            var randomCity = DataGenerator.City();
            values.FirstOrDefault(x => x.InfoKey == InfoKeyNames.consumer_City).Value = randomCity;
            return randomCity;
        }

        public string GetCityAddressOfReceiver(GetProfileReceiverResponse receiverProfileResponse)
        {
            return receiverProfileResponse.Payload.CurrentValues
                .FirstOrDefault(x => x.InfoKey == InfoKeyNames.receiver_City).Value;
        }

        public string SetRandomAddressCityOfReceiver(List<KeyValuePairType> values)
        {
            var randomCity = DataGenerator.City();
            values.FirstOrDefault(x => x.InfoKey == InfoKeyNames.receiver_City).Value = randomCity;
            return randomCity;
        }

        public string GetCityAddressOfSender(GetProfileSenderResponse senderProfileResponse)
        {
            return senderProfileResponse.Payload.CurrentValues
                .FirstOrDefault(x => x.InfoKey == InfoKeyNames.sender_City).Value;
        }

        public string SetRandomAddressCityOfSender(List<KeyValuePairType> values)
        {
            var randomCity = DataGenerator.City();
            values.FirstOrDefault(x => x.InfoKey == InfoKeyNames.sender_City).Value = randomCity;
            return randomCity;
        }

        public SearchConsumerProfilesResponse SearchConsumerProfiles(SearchConsumerProfilesData searchConsumerProfileData, string searchCriteria, List<KeyValuePairType> searchValues)
        {
            searchConsumerProfileData.SearchConsumerProfilesRequest =
                SearchConsumerProfilesRequestFactory.NewSearchConsumerProfilesRequest();
            searchConsumerProfileData.SearchConsumerProfilesRequest.FieldValues.Add(new KeyValuePairType()
            {
                InfoKey = InfoKeyNames.search_CriteriaName,
                Value = searchCriteria
            });
            if (searchValues != null && searchValues.Count > 0)
                searchConsumerProfileData.SearchConsumerProfilesRequest.FieldValues.AddRange(searchValues);
            return _acIntegration.SearchConsumerProfiles(searchConsumerProfileData);
        }

        public List<KeyValuePairType> GetCorectSearchValues(List<InfoBase> fieldsToCollect,
            List<KeyValuePairType> currentValues)
        {
            var fields = _validationOperations.GetAllFieldsToCollect(fieldsToCollect);
            var result = new List<KeyValuePairType>();
            foreach (var field in fields)
            {
                result.Add(new KeyValuePairType()
                {
                    InfoKey = field,
                    Value = currentValues.FirstOrDefault(x => x.InfoKey.Equals(field))?.Value
                });
            }

            return result;
        }

        public string GetConsumerProfileId(List<KeyValuePairType> values)
        {
            return values.FirstOrDefault(x => x.InfoKey == InfoKeyNames.consumer_ProfileID)?.Value;
        }

        public string GetReceiverProfileId(List<KeyValuePairType> values)
        {
            return values.FirstOrDefault(x => x.InfoKey == InfoKeyNames.receiver_ProfileID)?.Value;
        }

        public string GetSenderProfileId(List<KeyValuePairType> values)
        {
            return values.FirstOrDefault(x => x.InfoKey == InfoKeyNames.sender_ProfileID)?.Value;
        }
        
        public List<KeyValuePairType> GetIncorectSearchValues(List<InfoBase> fieldsToCollect)
        {
            var fields= _validationOperations.GetAllFieldsToCollect(fieldsToCollect);
            var result = new List<KeyValuePairType>();
            foreach (var field in fields)
            {
                result.Add(new KeyValuePairType()
                {
                    InfoKey = field,
                    Value = GetIncorectFieldValue(field)
                });
            }
            return result;
        }

        public List<KeyValuePairType> GetNotExistingSearchValues(List<InfoBase> fieldsToCollect)
        {
            var fields= _validationOperations.GetAllFieldsToCollect(fieldsToCollect);
            var result = new List<KeyValuePairType>();
            foreach (var field in fields)
            {
                result.Add(new KeyValuePairType()
                {
                    InfoKey = field,
                    Value = GetNotExistingValue(field)
                });
            }
            return result;
        }

        private string GetNotExistingValue(string fieldInfoKey)
        {
            switch (fieldInfoKey)
            {
                case InfoKeyNames.consumer_LastName:
                    return DataGenerator.LastName()+"A";
                case InfoKeyNames.consumer_DOB:
                    return new DateTime(1800, 1, 1).ToString("yyyy-MM-dd");
                case InfoKeyNames.consumer_FirstName:
                    return DataGenerator.FirstName()+"X";
                case InfoKeyNames.consumer_PrimaryPhone:
                    return "9999999999";
                case InfoKeyNames.mgiRewardsNumber:
                    return "1234567";
                case InfoKeyNames.consumer_ReferenceNumber:
                    return "1234567";
                case InfoKeyNames.consumer_AccountNumber:
                    return "99999999999999999999999999";
                case InfoKeyNames.consumer_LastName2:
                    return DataGenerator.LastName()+"QWERTY";
                case InfoKeyNames.consumer_Address:
                    return DataGenerator.Address() + "XYZ";

                default: return fieldInfoKey;
            }
        }

        private string GetIncorectFieldValue(string fieldInfoKey)
        {
            switch (fieldInfoKey)
            {
                case InfoKeyNames.consumer_LastName:
                    return DataGenerator.DOB();
                case InfoKeyNames.consumer_DOB:
                    return DataGenerator.LastName();
                case InfoKeyNames.consumer_FirstName:
                    return DataGenerator.DOB();
                case InfoKeyNames.consumer_PrimaryPhone:
                    return DataGenerator.FirstName();
                case InfoKeyNames.mgiRewardsNumber:
                    return DataGenerator.DOB();
                case InfoKeyNames.consumer_ReferenceNumber:
                    return DataGenerator.DOB();
                case InfoKeyNames.consumer_AccountNumber:
                    return DataGenerator.FirstName();
                case InfoKeyNames.consumer_LastName2:
                    return DataGenerator.DOB();
                case InfoKeyNames.consumer_Address:
                    return DataGenerator.AccountNumber();

                    default: return fieldInfoKey;
            }
        }
    }
}
