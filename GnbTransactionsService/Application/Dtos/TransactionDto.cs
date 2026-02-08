namespace GnbTransactionsService.Application.Dtos
{
    /// <summary>
    /// Represents a DTO for a transaction (includes SKU, amount, and currency information)
    /// </summary>
    public class TransactionDto
    {
        public string Sku { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }

        public TransactionDto() { }

        public TransactionDto(string sku, decimal amount, string currency)
        {
            Sku = sku;
            Amount = amount;
            Currency = currency;
        }
    }
}
