﻿using System.Collections.Generic;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.Lookup
{
    public static class InfoKeysWithGroupType
    {
        public static Dictionary<string, string> GetInfoKeyGroupTypeDictionary => new Dictionary<string, string>()
        {
            {"agent_UseSendData", "UseSendData"},
            {"send_AgentTransactionId", "AgentTransactionId"},
            {"sender_NationalityAtBirth", "NationalityAtBirth"},
            {"sender_Nationality", "Nationality"},
            {"sender_Birth_City", "City"},
            {"sender_Birth_Country_SubdivisionCode", "SubdivisionCode"},
            {"sender_Birth_Country_SubdivisionText", "Text"},
            {"sender_Birth_Country", "Country"},
            {"sender_CitizenshipAtBirthCountry", "Country"},
            {"sender_CitizenshipCountry", "Country"},
            {"sender_DualCitizenshipCountry", "Country"},
            {"sender_PersonalId1_Type", "enum_PERSONAL_ID1_TYPE"},
            {"sender_PersonalId1_Number", @"PersonalId\d_Number"},
            {"sender_PersonalId1_Issue_Year", "Year"},
            {"sender_PersonalId1_Issue_Month", "Month"},
            {"sender_PersonalId1_Issue_Day", "Day"},
            {"sender_PersonalId1_Expiration_Year", "Year"},
            {"sender_PersonalId1_Expiration_Month", "Month"},
            {"sender_PersonalId1_Expiration_Day", "Day"},
            {"sender_PersonalId1_Issue_Country", "Country"},
            {"sender_PersonalId1_Issue_Country_SubdivisionCode", "SubdivisionCode"},
            {"sender_PersonalId1_Issue_City", "City"},
            {"sender_PersonalId1_Issue_Authority", "enum_PRSONL_ID_ISUR_TYPE"},
            {"sender_PersonalId2_Type", "enum_PERSONAL_ID2_TYPE"},
            {"sender_PersonalId2_Number", @"PersonalId\d_Number"},
            {"sender_PersonalId2_Issue_Year", "Year"},
            {"sender_PersonalId2_Issue_Month", "Month"},
            {"sender_PersonalId2_Issue_Day", "Day"},
            {"sender_PersonalId2_Expiration_Year", "Year"},
            {"sender_PersonalId2_Expiration_Month", "Month"},
            {"sender_PersonalId2_Expiration_Day", "Day"},
            {"sender_PersonalId2_Issue_Country", "Country"},
            {"sender_PersonalId2_Issue_Country_SubdivisionCode", "SubdivisionCode"},
            {"sender_PersonalId2_Issue_City", "City"},
            {"sender_PersonalId2_Issue_Authority", "enum_PRSONL_ID_ISUR_TYPE"},
            {"sender_FirstName", "FirstName"},
            {"sender_MiddleName", "MiddleName"},
            {"sender_LastName", @"LastName\d?"},
            {"sender_LastName2", @"LastName\d?"},
            {"sender_NameSuffix", "enum_NAME_SUFFIX"},
            {"sender_NameSuffixOther", "Other"},
            {"sender_Country", "Country"},
            {"sender_Address", "Address"},
            {"sender_Address2", "Address2"},
            {"sender_Address3", "Address3"},
            {"sender_City", "City"},
            {"sender_Country_SubdivisionCode", "SubdivisionCode"},
            {"sender_PostalCode", "PostalCode"},
            {"sender_PrimaryPhoneCountryCode", "PhoneCountryCode"},
            {"sender_PrimaryPhone", "Phone"},
            {"sender_PrimaryPhoneSMSEnabled", "PhoneSMSEnabled"},
            {"sender_SecondaryPhoneCountryCode", "PhoneCountryCode"},
            {"sender_SecondaryPhone", "Phone"},
            {"sender_SecondaryPhoneSMSEnabled", "PhoneSMSEnabled"},
            {"sender_Email", "Email"},
            {"sender_DOB", "DOB"},
            {"sender_Gender", "enum_GENDER"},
            {"sender_Occupation", "enum_OCCUPATION"},
            {"sender_OccupationOther", "Other"},
            {"receiver_PersonalId1_Type", "enum_PERSONAL_ID1_TYPE"},
            {"receiver_PersonalId1_Number", @"PersonalId\d_Number"},
            {"receiver_PersonalId1_Issue_Year", "Year"},
            {"receiver_PersonalId1_Issue_Month", "Month"},
            {"receiver_PersonalId1_Issue_Day", "Day"},
            {"receiver_PersonalId1_Expiration_Year", "Year"},
            {"receiver_PersonalId1_Expiration_Month", "Month"},
            {"receiver_PersonalId1_Expiration_Day", "Day"},
            {"receiver_PersonalId1_Issue_Country", "Country"},
            {"receiver_PersonalId1_Issue_Country_SubdivisionCode", "SubdivisionCode"},
            {"receiver_PersonalId1_Issue_City", "City"},
            {"receiver_PersonalId1_Issue_Authority", "enum_PRSONL_ID_ISUR_TYPE"},
            {"receiver_FirstName", "FirstName"},
            {"receiver_MiddleName", "MiddleName"},
            {"receiver_LastName", @"LastName\d"},
            {"receiver_LastName2", @"LastName\d?"},
            {"receiver_NameSuffix", "enum_NAME_SUFFIX"},
            {"receiver_NameSuffixOther", "Other"},
            {"receiver_Country", "Country"},
            {"receiver_Address", "Address"},
            {"receiver_Address2", "Address2"},
            {"receiver_Address3", "Address3"},
            {"receiver_City", "City"},
            {"receiver_Country_SubdivisionCode", "SubdivisionCode"},
            {"receiver_PostalCode", "PostalCode"},
            {"receiver_PrimaryPhoneCountryCode", "PhoneCountryCode"},
            {"receiver_PrimaryPhone", "Phone"},
            {"destination_Country_SubdivisionCode", "SubdivisionCode"},
            {"serviceOption", "ServiceOption"},
            {"bankIdentifier_WithLookup", "WithLookup"},
            {"customerReceiveNumber", "CustomerReceiveNumber"},
            {"accountType", "AccountType"},
            {"accountNumber", "AccountNumber"},
            {"bankCode", "BankCode"},
            {"bankName", "enum_"},
            {"bankNameText", "Text"},
            {"benefIdNumber", "BenefIdNumber"},
            {"bankBranchCode", "BranchCode"},
            {"bankIdentifier", "BankIdentifier"},
            {"receiverCubanCommunistPartyFlag", "Flag"},
            {"receiverCubanGovernmentFlag", "Flag"},
            {"receiverPurposeOfTransactionEmigrationFlag", "Flag"},
            {"receiverIsCloseRelativeFlag", "Flag"},
            {"direction1", @"direction\d"},
            {"direction2", @"direction\d"},
            {"direction3", @"direction\d"},
            {"messageField1", @"MessageField\d"},
            {"messageField2", @"MessageField\d"},
            {"testQuestion", "Question"},
            {"testAnswer", "Answer"},
            {"send_ConsumerId", "ConsumerId"},
            {"operatorName", "OperatorName"},
            {"timeToLive", "TimeToLive"},
            {"creatorText", "Text"},
            {"transactionSMSNotificationOptIn", "OptIn"},
            {"pcTerminalNumber", "PcTerminalNumber"},
            {"paymentTenderedType", "enum_PAYMENT_TYPE"},
            {"proofOfFunds", "enum_PROOF_OF_FUNDS"},
            {"proofOfFundsOther", "Other"},
            {"sourceOfFunds", "enum_SOURCE_OF_FUNDS"},
            {"sourceOfFundsOther", "Other"},
            {"relationshipToReceiver", "enum_RLTIONSHP_TO_RECIVR"},
            {"relationshipToReceiverOther", "Other"},
            {"send_PurposeOfTransaction", "enum_PURPSE_OF_TRNSCTION"},
            {"send_PurposeOfTransactionOther", "Other"},
            {"mgiRewardsNumber", "MgiRewardsNumber"},
            {"agent_CustomerNumber", "CustomerNumber"},
            {"thirdParty_Sender_Type", "enum_THRD_PRTY_TYPE_CODS"},
            {"thirdParty_Sender_Address", "Address"},
            {"thirdParty_Sender_City", "City"},
            {"thirdParty_Sender_Country", "Country"},
            {"thirdParty_Sender_Country_SubdivisionCode", "SubdivisionCode"},
            {"thirdParty_Sender_Organization", "Organization"},
            {"thirdParty_Sender_PersonalId2_Type", "enum_PERSONAL_ID2_TYPE"},
            {"thirdParty_Sender_PostalCode", "PostalCode"},
            {"thirdParty_Sender_PersonalId2_Number", @"PersonalId\d_Number"},
            {"thirdParty_Sender_DOB", "DOB"},
            {"thirdParty_Sender_FirstName", "FirstName"},
            {"thirdParty_Sender_LastName", @"LastName\d?"},
            {"thirdParty_Sender_Occupation", "enum_OCCUPATION"},
            {"thirdParty_Sender_OccupationOther", "Other"},
            {"receive_AgentConsumerID", "ConsumerId"},
            {"agent_UseReceiveData", "UseReceiveData"},
            {"receive_AgentTransactionId", "AgentTransactionId"},
            {"otherPayoutType", "OtherPayoutType"},
            {"otherPayoutAmount", "Amount"},
            {"card_SwipedFlag", "Flag"},
            {"relationshipToSender", "enum_RELATIONSHP_TO_SNDR"},
            {"relationshipToSenderOther", "Other"},
            {"receive_PurposeOfTransaction", "enum_PURPSE_OF_TRNSCTION"},
            {"receive_PurposeOfTransactionOther", "Other"},
            {"receive_ConsumerId", "ConsumerId"},
            {"transactionPin", "TransactionPin"},
            {"receiver_NationalityAtBirth", "NationalityAtBirth"},
            {"receiver_Nationality", "Nationality"},
            {"receiver_Birth_City", "City"},
            {"receiver_Birth_Country_SubdivisionCode", "SubdivisionCode"},
            {"receiver_Birth_Country_SubdivisionText", "Text"},
            {"receiver_Birth_Country", "Country"},
            {"receiver_CitizenshipAtBirthCountry", "Country"},
            {"receiver_CitizenshipCountry", "Country"},
            {"receiver_DualCitizenshipCountry", "Country"},
            {"receiver_PersonalId2_Type", "enum_PERSONAL_ID2_TYPE"},
            {"receiver_PersonalId2_Number", @"PersonalId\d_Number"},
            {"receiver_PersonalId2_Issue_Year", "Year"},
            {"receiver_PersonalId2_Issue_Month", "Month"},
            {"receiver_PersonalId2_Issue_Day", "Day"},
            {"receiver_PersonalId2_Expiration_Year", "Year"},
            {"receiver_PersonalId2_Expiration_Month", "Month"},
            {"receiver_PersonalId2_Expiration_Day", "Day"},
            {"receiver_PersonalId2_Issue_Country", "Country"},
            {"receiver_PersonalId2_Issue_Country_SubdivisionCode", "SubdivisionCode"},
            {"receiver_PersonalId2_Issue_City", "City"},
            {"receiver_PersonalId2_Issue_Authority", "enum_PRSONL_ID_ISUR_TYPE"},
            {"receiver_PrimaryPhoneSMSEnabled", "PhoneSMSEnabled"},
            {"receiver_SecondaryPhoneCountryCode", "PhoneCountryCode"},
            {"receiver_SecondaryPhone", "Phone"},
            {"receiver_SecondaryPhoneSMSEnabled", "PhoneSMSEnabled"},
            {"receiver_Email", "Email"},
            {"receiver_DOB", "DOB"},
            {"receiver_Gender", "enum_GENDER"},
            {"receiver_Occupation", "enum_OCCUPATION"},
            {"receiver_OccupationOther", "Other"},
            {"thirdParty_Receiver_Type", "enum_THRD_PRTY_TYPE_CODS"},
            {"thirdParty_Receiver_Address", "Address"},
            {"thirdParty_Receiver_City", "City"},
            {"thirdParty_Receiver_Country", "Country"},
            {"thirdParty_Receiver_Country_SubdivisionCode", "SubdivisionCode"},
            {"thirdParty_Receiver_Organization", "Organization"},
            {"thirdParty_Receiver_PersonalId2_Type", "enum_PERSONAL_ID2_TYPE"},
            {"thirdParty_Receiver_PostalCode", "PostalCode"},
            {"thirdParty_Receiver_PersonalId2_Number", @"PersonalId\d_Number"},
            {"thirdParty_Receiver_DOB", "DOB"},
            {"thirdParty_Receiver_FirstName", "FirstName"},
            {"thirdParty_Receiver_LastName", @"LastName\d?"},
            {"thirdParty_Receiver_Occupation", "enum_OCCUPATION"},
            {"thirdParty_Receiver_OccupationOther", "Other"}
        };
    }
}