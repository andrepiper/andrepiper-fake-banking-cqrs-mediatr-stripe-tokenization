using CQRS.Pattern.Infastructure.Data;
using CQRS.Pattern.Infastructure.Models;
using CQRS.Pattern.Infastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CQRS.Pattern.Infastructure.Repositories.Implementation
{
    public class AccountRepository : IAccountRepository
    {
        private readonly CQRSDbContext _context;

        public AccountRepository(CQRSDbContext context)
        {
            _context = context;
        }

        public async Task<Account> GetByIdAsync(long accountId)
        {
            return await _context.Set<Account>().FirstOrDefaultAsync(a => a.Id == accountId);
        }

        public async Task<List<Account>> GetAccountsAsync(string emailAddress)
        {
            return await _context.Set<Account>().Where(a => a.Email == emailAddress).ToListAsync();
        }

        public async Task<IEnumerable<Account>> GetAllAsync()
        {
            return await _context.Set<Account>().ToListAsync();
        }

        public async Task AddAsync(Account account)
        {
            _context.Set<Account>().Add(account);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Account account)
        {
            _context.Set<Account>().Update(account);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(long accountId)
        {
            var account = await GetByIdAsync(accountId);
            if (account != null)
            {
                _context.Set<Account>().Remove(account);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<decimal> GetBalanceAsync(long accountId)
        {
            // Optimized query to directly sum transaction amounts
            return await _context.Set<Transaction>()
                .Where(t => t.AccountId == accountId)
                .SumAsync(t => t.Amount * (t.IsDebit ? -1 : 1));
        }

        /// <summary>
        /// Can be ran at the end of all transactional Ops
        /// </summary>
        /// <param name="account"></param>
        /// <param name="acmount"></param>
        /// <param name="isDebit"></param>
        /// <returns></returns>
        public async Task LedgerBalance(Account account, decimal amount, bool isDebit)
        {
            var currentBalace = await GetBalanceAsync(account.Id);
            var newBal = 0.0m;
            switch (isDebit)
            {
                case true:
                    newBal = currentBalace - amount;
                    break;
                default:
                    newBal = currentBalace + amount;
                    break;
            }
            account.Balance = newBal;
            await UpdateAsync(account);
        }

        /// <summary>
		/// 
		/// </summary>
		/// <param name="email"></param>
		/// <param name="defaultAccount"></param>
		/// <returns></returns>
		public async Task EnforceDefaultAccount(string email, long defaultAccount)
        {
            var accounts = await GetAccountsAsync(email);
            if (accounts.Any())
            {
                foreach (var account in accounts)
                {
                    if (account.Id == defaultAccount)
                        account.IsDefault = true;
                    else
                        account.IsDefault = false;
                    await UpdateAsync(account);
                }
            }
        }

        public async Task<Account> GetDefaultAccountByEmail(string emailAddress)
        {
            return await _context.Set<Account>().Where(a => a.Email == emailAddress && a.IsDefault == true).FirstOrDefaultAsync();
        }
    }
}
