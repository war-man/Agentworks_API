using AwApi.Auth;
using AwApi.Business.Reports;
using AwApi.ViewModels;
using MoneyGram.Common.Extensions;
using System.Web.Http;
using AwApi.ViewModels.Reports;

namespace AwApi.Controllers
{
    [AwAuthorization(MgiAwRole.TellerManagerAdmin), AwDeviceAuthorization(DeviceSecurityLevel.Registered)]
    [RoutePrefix("api/report")]
    public class ReportController : ApiController
    {
        private readonly IReportsBusiness _reportsBusiness;

        public ReportController(IReportsBusiness reportsBusiness)
        {
            reportsBusiness.ThrowIfNull(nameof(reportsBusiness));
            _reportsBusiness = reportsBusiness;
        }

        /// <summary>
        /// Get report by type
        /// </summary>
        /// <param name="reportRequest">Instance of ReportRequest</param>
        /// <returns></returns>
        [Route("DailyTranDetail")]
        [HttpPost]
        public IHttpActionResult DailyTranDetailReport([FromBody] ReportRequest reportRequest)
        {
            reportRequest.ThrowIfNull(nameof(reportRequest));

            var res = _reportsBusiness.DailyTranDetailReport(reportRequest);

            return Ok(res);
        }

        /// <summary>
        /// Get report by type
        /// </summary>
        /// <param name="reportRequest">Instance of ReportRequest</param>
        /// <returns></returns>
        [Route("DailyTranSummary")]
        [HttpPost]
        public IHttpActionResult DailyTranSummaryReport([FromBody] ReportRequest reportRequest)
        {
            reportRequest.ThrowIfNull(nameof(reportRequest));

            var res = _reportsBusiness.DailyTranSummaryReport(reportRequest);

            return Ok(res);
        }


    }
}