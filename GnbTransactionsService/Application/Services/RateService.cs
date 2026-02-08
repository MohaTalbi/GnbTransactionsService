using GnbTransactionsService.Domain.Models;
using GnbTransactionsService.Infrastructure.Repositories.Interfaces;

namespace GnbTransactionsService.Application.Services
{
    public class RateService
    {
        private readonly IRateRepository rateRepository;
        private readonly ILogger<RateService> logger;

        /// <summary>
        /// Initializes a new instance of the RateService class using the specified rate repository.
        /// </summary>
        public RateService(IRateRepository rateRepository, ILogger<RateService> logger)
        {
            this.rateRepository = rateRepository;
        }

        public List<Rate> GetAllRates()
        {
            List<Rate> rates = rateRepository.GetAll();

            if (rates.Count == 0)
            {
                logger.LogInformation("No rates founded!");
            }

            return rates;
        }

        public CurrencyConverterService BuildCurrencyConverter()
        {
            List<Rate> rates = rateRepository.GetAll();

            if (rates.Count == 0)
            {
                logger.LogInformation("No rates founded!");
            }

            return new CurrencyConverterService(rates);
        }
    }
}
