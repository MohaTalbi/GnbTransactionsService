using GnbTransactionsService.Domain.Models;

namespace GnbTransactionsService.Infrastructure.Repositories.Interfaces
{
    /// <summary>
    /// Defines the contract for a repository that provides access to transactions.
    /// </summary>
    public interface ITransactionRepository
    {
        List<Transaction> GetAll();
    }
}
