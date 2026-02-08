namespace GnbTransactionsService.Application.Dtos
{
    /// <summary>
    /// Represents the result of retrieving detailed information for a specific SKU (transactions & the total value in euros)
    /// </summary>
    public class SkuDetailResult
    {
        public string Sku { get; }
        public decimal TotalInEur { get; }
        public List<ConvertedTransaction> Transactions { get; }


        public SkuDetailResult(
            string sku,
            List<ConvertedTransaction> transactions,
            decimal totalInEur)
        {
            Sku = sku;
            TotalInEur = totalInEur;
            Transactions = transactions;
        }

        public static SkuDetailResult Empty(string sku) => new(sku, new List<ConvertedTransaction>(), 0m);
    }
}
