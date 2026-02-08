using GnbTransactionsService.Application.Services;
using GnbTransactionsService.Application.Dtos;
using Microsoft.AspNetCore.Mvc;
using GnbTransactionsService.Domain.Models;

namespace GnbTransactionsService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly TransactionService transactionService;

        public TransactionsController(
            TransactionService transactionService
            )
        {
            this.transactionService = transactionService;
        }

        /// <summary>
        /// List of all transactions
        /// </summary>
        [HttpGet()]
        public ActionResult<TransactionDto> GetTransactions()
        {
            List<Transaction> transactions = transactionService.GetAllTransactions();
            var dtos = transactions.Select(t => new TransactionDto(t.Sku, t.Amount, t.Currency)).ToList();

            if (dtos.Count == 0)
            {
                return NoContent();
            }
            else
            {
                return Ok(dtos);
            }
        }
    }
}
