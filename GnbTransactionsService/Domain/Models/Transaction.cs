using System.Text.Json.Serialization;

namespace GnbTransactionsService.Domain.Models
{
    public class Transaction
    {
        [JsonPropertyName("sku")]
        public string Sku { get; set; }

        [JsonPropertyName("amount")]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public decimal Amount { get; set; }


        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        /// <summary>
        /// Domain model representing a transaction with SKU, amount, and currency information.
        /// </summary>
        /// <param name="sku"></param>
        /// <param name="amount"></param>
        /// <param name="currency"></param>
        /// <exception cref="ArgumentException"></exception>
        public Transaction(string sku, decimal amount, string currency)
        {
            if (string.IsNullOrWhiteSpace(sku))
                throw new ArgumentException("SKU cannot be empty", nameof(sku));

            if (string.IsNullOrWhiteSpace(currency))
                throw new ArgumentException("Currency cannot be empty", nameof(currency));

            this.Sku = sku;
            this.Amount = amount;
            this.Currency = currency;
        }
    }
}
