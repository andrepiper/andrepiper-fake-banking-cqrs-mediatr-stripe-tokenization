using CQRS.Pattern.Infastructure.Models;

namespace CQRS.Pattern.Infastructure.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        Task<Account> GetByIdAsync(long accountId);
        Task<List<Account>> GetAccountsAsync(string emailAddress);
        Task<IEnumerable<Account>> GetAllAsync();
        Task AddAsync(Account account);
        Task UpdateAsync(Account account);
        Task DeleteAsync(long accountId);
        Task<decimal> GetBalanceAsync(long accountId);
        Task LedgerBalance(Account account, decimal amount, bool isDebit);
        Task<Account> GetDefaultAccountByEmail(string emailAddress);
        Task EnforceDefaultAccount(string email, long defaultAccount);
    }
}
