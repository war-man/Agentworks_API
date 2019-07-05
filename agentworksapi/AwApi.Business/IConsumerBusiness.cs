using AwApi.ViewModels;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace AwApi.Business
{
    public interface IConsumerBusiness
    {
        AcApiResponse<ConsumerHistoryLookupResponse, ApiData> ConsumerHistoryLookup(ConsumerHistoryLookupRequest reqVm);
        AcApiResponse<GetProfileReceiverResponse, ApiData> GetProfileReceiver(GetProfileReceiverRequest reqVm);
        AcApiResponse<GetProfileSenderResponse, ApiData> GetProfileSender(GetProfileSenderRequest reqVm);
        AcApiResponse<CreateOrUpdateProfileReceiverResponse, ApiData> CreateOrUpdateProfileReceiver(CreateOrUpdateProfileReceiverRequest reqVm);
        AcApiResponse<CreateOrUpdateProfileSenderResponse, ApiData> CreateOrUpdateProfileSender(CreateOrUpdateProfileSenderRequest reqVm);
        AcApiResponse<GetProfileConsumerResponse, ApiData> GetProfileConsumer(GetProfileConsumerRequest reqVm);
        AcApiResponse<CreateOrUpdateProfileConsumerResponse, ApiData> CreateOrUpdateProfileConsumer(CreateOrUpdateProfileConsumerRequest reqVm);
        AcApiResponse<SaveConsumerProfileImageResponse, ApiData> SaveConsumerProfileImage(SaveConsumerProfileImageRequest reqVm);
        AcApiResponse<SearchConsumerProfilesResponse, ApiData> SearchConsumerProfiles(SearchConsumerProfilesRequest reqVm);
        AcApiResponse<GetConsumerProfileTransactionHistoryResponse, ApiData> GetConsumerProfileTransactionHistory(GetConsumerProfileTransactionHistoryRequest reqVm);
        AcApiResponse<SavePersonalIDImageResponse, ApiData> SavePersonalIDImage(ViewModels.Consumer.SavePersonalIDImageRequest reqVm);
        AcApiResponse<ViewModels.Consumer.GetPersonalIDImageResponse, ApiData> GetPersonalIDImage(GetPersonalIDImageRequest reqVm);
        AcApiResponse<SaveConsumerProfileDocumentResponse, ApiData> SaveConsumerProfileDocument(ViewModels.Consumer.SaveConsumerProfileDocumentRequest req);
        AcApiResponse<ViewModels.Consumer.GetConsumerProfileDocumentResponse, ApiData> GetConsumerProfileDocument(GetConsumerProfileDocumentRequest reqVm);
    }
}