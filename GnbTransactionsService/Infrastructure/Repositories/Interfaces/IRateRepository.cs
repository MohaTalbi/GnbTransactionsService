using GnbTransactionsService.Domain.Models;

namespace GnbTransactionsService.Infrastructure.Repositories.Interfaces
{
    /// <summary>
    /// Defines the contract for a repository that provides access to currency exchange rates.
    /// </summary>
    public interface IRateRepository
    {
        List<Rate> GetAll();
    }
}
