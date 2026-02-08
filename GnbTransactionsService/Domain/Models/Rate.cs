using System.Text.Json.Serialization;

namespace GnbTransactionsService.Domain.Models
{
    public class Rate
    {
        [JsonPropertyName("from")]
        public string From { get; set; }

        [JsonPropertyName("to")]
        public string To { get; set; }

        [JsonPropertyName("rate")]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public decimal Value { get; set; }

        /// <summary>
        /// Domain model representing an exchange rate between two currencies.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="value"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public Rate(string from, string to, decimal value)
        {
            if (string.IsNullOrWhiteSpace(from))
                throw new ArgumentException("From currency cannot be empty", nameof(from));

            if (string.IsNullOrWhiteSpace(to))
                throw new ArgumentException("To currency cannot be empty", nameof(to));

            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value), "Rate must be greater than zero");

            this.From = from;
            this.To = to;
            this.Value = value;
        }
    }
}
