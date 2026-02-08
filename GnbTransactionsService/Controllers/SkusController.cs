using GnbTransactionsService.Application.Services;
using GnbTransactionsService.Application.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace GnbTransactionsService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SkusController : ControllerBase
    {
        private readonly TransactionService transactionService;

        public SkusController(
            TransactionService transactionService
            )
        {
            this.transactionService = transactionService;
        }

        /// <summary>
        /// Detail of an SKU in EUR
        /// </summary>
        [HttpGet("{sku}")]
        public ActionResult<SkuDetailResult> GetSkuDetail(string sku)
        {
            SkuDetailResult result = transactionService.GetSkuDetail(sku);

            if (result.Transactions.Count == 0)
                return NotFound(new
                {
                    sku,
                    message = "No transactions found for this SKU"
                });

            return Ok(result);
        }
    }
}
