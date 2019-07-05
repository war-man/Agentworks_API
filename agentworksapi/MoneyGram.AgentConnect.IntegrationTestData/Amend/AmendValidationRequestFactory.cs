using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.Setup;
using System.Collections.Generic;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.Amend
{
    public static class AmendValidationRequestFactory
    {
        public static AmendValidationRequest NewRequestWithBaseData(string mgiSessionId)
        {
            return new AmendValidationRequest
            {
                MgiSessionID = mgiSessionId,
                FieldValues = new List<KeyValuePairType>(),
                VerifiedFields = new List<string>()
            };
        }

        public static AmendValidationRequest FromMockData(this AmendValidationRequest amendValReq)
        {
            amendValReq.FieldValues = amendValReq.FieldValues ?? new List<KeyValuePairType>();
            amendValReq.FieldValues.Clear();

            //add all required fields
            amendValReq.FieldValues.Add(InfoKeyWithValues.GetFieldValue(InfoKeyNames.receiver_FirstName));
            amendValReq.FieldValues.Add(InfoKeyWithValues.GetFieldValue(InfoKeyNames.receiver_MiddleName));
            amendValReq.FieldValues.Add(InfoKeyWithValues.GetFieldValue(InfoKeyNames.receiver_LastName));
            amendValReq.FieldValues.Add(InfoKeyWithValues.GetFieldValue(InfoKeyNames.receiver_LastName2));
            amendValReq.FieldValues.Add(InfoKeyWithValues.GetFieldValue(InfoKeyNames.receiver_NameSuffix));

            return amendValReq;
        }
    }
}