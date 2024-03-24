using CQRS.Pattern.Infastructure.Models;

namespace CQRS.Pattern.Infastructure.Repositories.Interfaces
{
    public interface ITransactionRepository
    {
        Task<Transaction> GetByIdAsync(long transactionId);
        Task<IEnumerable<Transaction>> GetByAccountIdAsync(long accountId);
        Task<IEnumerable<Transaction>> GetAllAsync();
        Task AddAsync(Transaction transaction);
        Task AddRangeAsync(IEnumerable<Transaction> transactions);
        Task UpdateAsync(Transaction transaction);
        Task DeleteAsync(long transactionId);
        Task<decimal> GetTotalBalanceAsync(long accountId);
        Task<IEnumerable<Transaction>> TransactionsByAccountIdAsync(long accountNo);
    }
}
