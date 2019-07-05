using System.Web.Http;
using AwApi.Auth;
using AwApi.Business;
using AwApi.ViewModels;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.Common.Extensions;

namespace AwApi.Controllers
{
    /// <summary>
    /// Document related endponts
    /// </summary>
    [AwAuthorization(MgiAwRole.Teller), AwDeviceAuthorization(DeviceSecurityLevel.Transactional)]
    [RoutePrefix("api/document")]
    public class DocumentController : ApiController
    {
        private readonly IDocumentBusiness documentBusiness;

        public DocumentController(IDocumentBusiness documentBusiness)
        {
            documentBusiness.ThrowIfNull(nameof(documentBusiness));

            this.documentBusiness = documentBusiness;
        }

        /// <summary>
        /// Save Transaction Document
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public IHttpActionResult SaveTransactionDocument(SaveTransactionDocumentRequest req)
        {
            var response = documentBusiness.SaveTransactionDocument(req);

            return Ok(response);
        }
    }
}