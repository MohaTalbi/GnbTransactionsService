namespace GnbTransactionsService.Application.Dtos
{
    /// <summary>
    /// Represents an exchange rate between two currencies
    /// </summary>
    public class RateDto
    {
        public string From { get; set; }
        public string To { get; set; }
        public decimal Value { get; set; }

        public RateDto() { }

        public RateDto(string from, string to, decimal value)
        {
            From = from;
            To = to;
            Value = value;
        }
    }
}
