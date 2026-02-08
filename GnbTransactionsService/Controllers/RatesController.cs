using GnbTransactionsService.Application.Dtos;
using GnbTransactionsService.Application.Services;
using GnbTransactionsService.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace GnbTransactionsService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatesController : ControllerBase
    {
        private readonly RateService rateService;

        public RatesController(
            RateService rateService)
        {
            this.rateService = rateService;
        }

        /// <summary>
        /// List of all rates
        /// </summary>
        [HttpGet()]
        public ActionResult<RateDto> GetRates()
        {
            List<Rate> rates = rateService.GetAllRates();
            List<RateDto> dtos = rates.Select(r => new RateDto(r.From, r.To, r.Value)).ToList();

            if(dtos.Count == 0)
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
