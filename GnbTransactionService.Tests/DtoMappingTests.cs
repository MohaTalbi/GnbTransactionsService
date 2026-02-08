using GnbTransactionsService.Application.Dtos;
using GnbTransactionsService.Domain.Models;

namespace GnbTransactionService.Tests
{
    public class DtoMappingTests
    {
        [Fact]
        public void Transaction_To_Dto_Mapping()
        {
            var tx = new Transaction("T1", 10m, "USD");
            var dto = new TransactionDto(tx.Sku, tx.Amount, tx.Currency);

            Assert.Equal(tx.Sku, dto.Sku);
            Assert.Equal(tx.Amount, dto.Amount);
            Assert.Equal(tx.Currency, dto.Currency);
        }

        [Fact]
        public void Rate_To_Dto_Mapping()
        {
            var rate = new Rate("EUR", "USD", 1.2m);
            var dto = new RateDto(rate.From, rate.To, rate.Value);

            Assert.Equal(rate.From, dto.From);
            Assert.Equal(rate.To, dto.To);
            Assert.Equal(rate.Value, dto.Value);
        }
    }
}
