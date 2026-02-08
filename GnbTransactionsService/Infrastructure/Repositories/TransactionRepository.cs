using GnbTransactionsService.Domain.Models;
using GnbTransactionsService.Infrastructure.Repositories.Interfaces;
using System.Text.Json;

namespace GnbTransactionsService.Infrastructure.Repositories
{
    /// <summary>
    /// Implements ITransactionRepository to provide access to transactions from a JSON file
    /// </summary>
    public class TransactionRepository : ITransactionRepository
    {
        private readonly string filePath;

        public TransactionRepository(string filePath)
        {
            this.filePath = filePath;
        }

        public List<Transaction> GetAll()
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException(
                    $"Transactions file not found at {filePath}"
                );

            string json = File.ReadAllText(filePath);

            var transactions = JsonSerializer.Deserialize<List<Transaction>>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

            return transactions ?? new List<Transaction>();
        }
    }
}
