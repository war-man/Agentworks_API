using AwApi.Auth;
using AwApi.Business;
using MoneyGram.Common.Extensions;
using System.Web.Http;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using AwApi.ViewModels;

namespace AwApi.Controllers
{
    [AwAuthorization(MgiAwRole.TellerManagerAdmin, MgiAwRole.Dt4Device)]
    [RoutePrefix("api/money-order")]
    public class MoneyOrderController : ApiController
    {
        private readonly IMoneyOrderBusiness _moneyOrderBusiness;

        public MoneyOrderController(IMoneyOrderBusiness moneyOrderBusiness)
        {
            moneyOrderBusiness.ThrowIfNull(nameof(moneyOrderBusiness));

            _moneyOrderBusiness = moneyOrderBusiness;
        }

        [HttpPost]
        [Route("totals")]
        public IHttpActionResult MoneyOrderTotal(MoneyOrderTotalRequest request)
        {
            var resp = _moneyOrderBusiness.MoneyOrderTotal(request);
            
            return Ok(resp);
        }

        [HttpPost]
        [Route("details")]
        public IHttpActionResult ComplianceTransaction(ComplianceTransactionRequest request)
        {
            var resp = _moneyOrderBusiness.ComplianceTransaction(request);

            return Ok(resp);
        }
    }
}