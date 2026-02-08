using GnbTransactionsService.Domain.Models;
using GnbTransactionsService.Application.Services;
using GnbTransactionsService.Domain.Exceptions;
using System.Globalization;

namespace GnbTransactionService.Tests
{
    public class CurrencyConverterServiceTests
    {
        [Fact]
        public void Convert_DirectRate_ReturnsExpected()
        {
            var rates = new List<Rate>
            {
                new Rate("USD", "EUR", 0.8m)
            };

            var converter = new CurrencyConverterService(rates);

            decimal result = converter.Convert(10m, "USD", "EUR");

            Assert.Equal(8.0m, result);
        }

        [Fact]
        public void Convert_MultiHopRate_ReturnsExpected()
        {
            var rates = new List<Rate>
            {
                new Rate("USD", "CAD", 1.25m),
                new Rate("CAD", "EUR", 0.7m)
            };

            var converter = new CurrencyConverterService(rates);

            decimal result = converter.Convert(10m, "USD", "EUR");

            // USD -> CAD (1.25) -> EUR (0.7) => total rate 0.875 => 10 * 0.875 = 8.75
            Assert.Equal(8.75m, result);
        }

        [Fact]
        public void Convert_NoPath_ThrowsCurrencyConversionException()
        {
            var rates = new List<Rate>
            {
                new Rate("USD", "CAD", 1.25m)
            };

            var converter = new CurrencyConverterService(rates);

            Assert.Throws<CurrencyConversionException>(() => converter.Convert(5m, "BRL", "EUR"));
        }

        [Fact]
        public void Convert_Cache_ReturnsConsistentResults()
        {
            var rates = new List<Rate>
            {
                new Rate("USD", "EUR", 0.8m)
            };

            var converter = new CurrencyConverterService(rates);

            decimal r1 = converter.Convert(2m, "USD", "EUR");
            decimal r2 = converter.Convert(3m, "USD", "EUR");

            Assert.Equal(1.6m, r1);
            Assert.Equal(2.4m, r2);
        }

        [Theory]
        [InlineData("1.005", "1.00")]
        [InlineData("1.015", "1.02")]
        public void Rounding_Banker_RoundsHalfToEven(string input, string expected)
        {
            decimal value = decimal.Parse(input, NumberStyles.Number, CultureInfo.InvariantCulture);
            decimal rounded = Math.Round(value, 2, MidpointRounding.ToEven);

            Assert.Equal(decimal.Parse(expected, CultureInfo.InvariantCulture), rounded);
        }
    }
}
