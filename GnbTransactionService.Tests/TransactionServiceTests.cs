using GnbTransactionsService.Application.Services;
using GnbTransactionsService.Domain.Models;
using Microsoft.Extensions.Logging.Abstractions;
using GnbTransactionsService.Infrastructure.Repositories.Interfaces;

namespace GnbTransactionService.Tests
{
    public class TransactionServiceTests
    {
        [Fact]
        public void GetSkuDetail_SkuDoesNotExist_ReturnsEmptyResult()
        {
            var transactions = new List<Transaction>();
            var txRepo = new InMemoryTransactionRepository(transactions);
            var converter = new CurrencyConverterService(new List<Rate>());
            var logger = new NullLogger<TransactionService>();

            var service = new TransactionService(txRepo, converter, logger);

            var result = service.GetSkuDetail("NON_EXISTENT");

            Assert.Equal("NON_EXISTENT", result.Sku);
            Assert.Empty(result.Transactions);
            Assert.Equal(0m, result.TotalInEur);
        }

        [Fact]
        public void GetSkuDetail_SomeTransactionsNotConvertible_ExcludesThem()
        {
            var transactions = new List<Transaction>
            {
                new Transaction("A1", 10m, "USD"),
                new Transaction("A1", 5m, "BRL") // BRL has no path
            };

            var txRepo = new InMemoryTransactionRepository(transactions);
            var rates = new List<Rate>
            {
                new Rate("USD", "EUR", 0.5m)
            };
            var converter = new CurrencyConverterService(rates);
            var logger = new NullLogger<TransactionService>();

            var service = new TransactionService(txRepo, converter, logger);

            var result = service.GetSkuDetail("A1");

            Assert.Single(result.Transactions);
            Assert.Equal(5.0m, result.TotalInEur); // 10 * 0.5
        }

        [Fact]
        public void GetSkuDetail_IncludesNegativeAmounts()
        {
            var transactions = new List<Transaction>
            {
                new Transaction("B1", 10m, "USD"),
                new Transaction("B1", -3m, "USD")
            };

            var txRepo = new InMemoryTransactionRepository(transactions);
            var rates = new List<Rate>
            {
                new Rate("USD", "EUR", 1.0m)
            };
            var converter = new CurrencyConverterService(rates);
            var logger = new NullLogger<TransactionService>();

            var service = new TransactionService(txRepo, converter, logger);

            var result = service.GetSkuDetail("B1");

            Assert.Equal(7m, result.TotalInEur);
            Assert.Equal(2, result.Transactions.Count);
        }

        // InMemory repo for tests
        private class InMemoryTransactionRepository : ITransactionRepository
        {
            private readonly List<Transaction> transactions;
            public InMemoryTransactionRepository(List<Transaction> transactions)
            {
                this.transactions = transactions;
            }

            public List<Transaction> GetAll() => transactions;
        }
    }
}
