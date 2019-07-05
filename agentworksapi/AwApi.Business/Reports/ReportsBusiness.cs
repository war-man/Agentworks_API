using System;
using System.Linq;
using AwApi.Business.LocationBusiness;
using AwApi.Business.Reports.Containers;
using AwApi.EntityMapper;
using AwApi.Integration;
using AwApi.ViewModels;
using AwApi.ViewModels.Reports;
using MoneyGram.Common.Extensions;

namespace AwApi.Business.Reports
{
    public class ReportsBusiness : IReportsBusiness
    {
        private readonly IDailyTranDetailContainer _dailyTranDetailContainer;
        private readonly IDailyTranSummaryContainer _dailyTranSummaryContainer;
        private readonly ILocationBusiness _locationBusiness;

        public ReportsBusiness(IDailyTranSummaryContainer dailyTranSummaryContainer,
            IDailyTranDetailContainer dailyTranDetailContainer,
            ILocationBusiness locationBusiness)
        {
            _dailyTranDetailContainer = dailyTranDetailContainer;
            _dailyTranSummaryContainer = dailyTranSummaryContainer;
            _locationBusiness = locationBusiness;
        }

        public ApiResponse<ReportResponse<DailyTranDetailReportResponsePayload>, ApiData> DailyTranDetailReport(ReportRequest reportRequest)
        {
            reportRequest.ThrowIfNull(nameof(reportRequest));
            AuthorizeReportRequest(reportRequest);

            var response = _dailyTranDetailContainer.GenerateReport(reportRequest); ;

            return new ApiResponse<ReportResponse<DailyTranDetailReportResponsePayload>, ApiData>
            {
                ResponseData = response,
                BusinessMetadata = MapperHelper.SetResponseProperties(null, DataSource.Dls)
            };
        }

        public ApiResponse<ReportResponse<DailyTranSummaryReportResponsePayload>, ApiData> DailyTranSummaryReport(ReportRequest reportRequest)
        {
            reportRequest.ThrowIfNull(nameof(reportRequest));
            AuthorizeReportRequest(reportRequest);

            var response = _dailyTranSummaryContainer.GenerateReport(reportRequest);

            return new ApiResponse<ReportResponse<DailyTranSummaryReportResponsePayload>, ApiData>
            {
                ResponseData = response,
                BusinessMetadata = MapperHelper.SetResponseProperties(null, DataSource.Dls)
            };
        }

        private void AuthorizeReportRequest(ReportRequest request)
        {
            var assignedLocations = AuthIntegration.GetAgentLocations().ToList();

            // assigned locations needs to be all main office locations
            if (assignedLocations.Any(location => location.ToLower() == "all"))
            {
                var mainOfficeLocations = _locationBusiness.LocationsForMainOffice(AuthIntegration.GetMainOfficeId());
                assignedLocations = mainOfficeLocations?.Select(location => location.id).ToList();
            }

            if (assignedLocations == null || !request.Locations.All(location => assignedLocations.Contains(location)))
            {
                throw new PrincipalException("User is not authorized to access this resource.")
                {
                    StatusCode = System.Net.HttpStatusCode.Unauthorized
                };
            }
        }
    }
}