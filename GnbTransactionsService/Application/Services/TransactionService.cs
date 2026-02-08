using GnbTransactionsService.Application.Dtos;
using GnbTransactionsService.Domain.Models;
using GnbTransactionsService.Infrastructure.Repositories.Interfaces;

namespace GnbTransactionsService.Application.Services
{
    public class TransactionService
    {
        private readonly ITransactionRepository transactionRepository;
        private readonly CurrencyConverterService currencyConverter;
        private readonly ILogger<TransactionService> logger;

        private const string TargetCurrency = "EUR";

        /// <summary>
        /// Initializes a new instance of the TransactionService class with the specified transaction repository, currency converter, and logger.
        /// </summary>
        public TransactionService(
            ITransactionRepository transactionRepository,
            CurrencyConverterService currencyConverter,
            ILogger<TransactionService> logger)
        {
            this.transactionRepository = transactionRepository;
            this.currencyConverter = currencyConverter;
            this.logger = logger;
        }

        public List<Transaction> GetAllTransactions()
        {
            return transactionRepository.GetAll();
        }

        public SkuDetailResult GetSkuDetail(string sku)
        {
            List<Transaction> transactions = transactionRepository
                .GetAll()
                .Where(t => t.Sku == sku)
                .ToList();

            if (transactions.Count == 0)
            {
                logger.LogInformation("No transactions founded for SKU {Sku}", sku);
                return SkuDetailResult.Empty(sku);
            }

            List<ConvertedTransaction> convertedTransactions = new();
            decimal total = 0m;

            foreach (Transaction transaction in transactions)
            {
                try
                {
                    decimal amountInEur = currencyConverter.Convert(
                        transaction.Amount,
                        transaction.Currency,
                        TargetCurrency
                    );

                    convertedTransactions.Add(new ConvertedTransaction(
                        transaction.Sku,
                        transaction.Amount,
                        transaction.Currency,
                        amountInEur
                    ));

                    total += amountInEur;
                }
                catch (Exception ex)
                {
                    logger.LogWarning(
                        ex,
                        "Excluding transaction for SKU {Sku} amount {Amount} {Currency} due to conversion error: {Message}",
                        transaction.Sku,
                        transaction.Amount,
                        transaction.Currency,
                        ex.Message
                    );
                    continue;
                }
            }

            return new SkuDetailResult(
                sku,
                convertedTransactions,
                Math.Round(total, 2, MidpointRounding.ToEven)
            );
        }
    }
}
