using AwApi.ViewModels;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace AwApi.Business
{
    public interface IDepositBusiness
    {
        AcApiResponse<GetDepositInformationResponse, ApiData> GetDepositInformation(GetDepositInformationRequest reqVm);
        AcApiResponse<GetDepositBankListResponse, ApiData> GetDepositBankList(GetDepositBankListRequest req);
        AcApiResponse<DepositAnnouncementResponse, ApiData> DepositAnnouncement(DepositAnnouncementRequest req);
    }
}