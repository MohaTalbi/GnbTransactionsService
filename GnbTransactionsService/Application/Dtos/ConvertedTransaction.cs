namespace GnbTransactionsService.Application.Dtos
{
    /// <summary>
    /// Immutable ConvertedTransaction
    /// </summary>
    /// <param name="Sku"></param>
    /// <param name="OriginalAmount"></param>
    /// <param name="OriginalCurrency"></param>
    /// <param name="AmountInEur"></param>
    public record ConvertedTransaction(
        string Sku,
        decimal OriginalAmount,
        string OriginalCurrency,
        decimal AmountInEur
    );
}
