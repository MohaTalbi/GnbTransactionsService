using GnbTransactionsService.Domain.Models;
using GnbTransactionsService.Infrastructure.Repositories.Interfaces;
using System.Text.Json;

namespace GnbTransactionsService.Infrastructure.Repositories
{
    /// <summary>
    /// Implements IRateRepository to provide access to currency exchange rates from a JSON file
    /// </summary>
    public class RateRepository : IRateRepository
    {
        private readonly string filePath;

        public RateRepository(string filePath)
        {
            this.filePath = filePath;
        }

        public List<Rate> GetAll()
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException(
                    $"Rates file not found at {filePath}"
                );

            string json = File.ReadAllText(filePath);

            var rates = JsonSerializer.Deserialize<List<Rate>>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

            return rates ?? new List<Rate>();
        }
    }
}
