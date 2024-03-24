using CQRS.Pattern.Infastructure.Data;
using CQRS.Pattern.Infastructure.Models;
using CQRS.Pattern.Infastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CQRS.Pattern.Infastructure.Repositories.Implementation
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly CQRSDbContext _context;

        public TransactionRepository(CQRSDbContext context)
        {
            _context = context;
        }

        public async Task<Transaction> GetByIdAsync(long transactionId)
        {
            return await _context.Set<Transaction>().FindAsync(transactionId);
        }

        public async Task<IEnumerable<Transaction>> GetByAccountIdAsync(long accountId)
        {
            return await _context.Set<Transaction>()
                .Where(t => t.AccountId == accountId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetAllAsync()
        {
            return await _context.Set<Transaction>().ToListAsync();
        }

        public async Task AddAsync(Transaction transaction)
        {
            _context.Set<Transaction>().Add(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<Transaction> transactions)
        {
            _context.Set<Transaction>().AddRange(transactions);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Transaction transaction)
        {
            _context.Set<Transaction>().Update(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(long transactionId)
        {
            var transaction = await GetByIdAsync(transactionId);
            if (transaction != null)
            {
                _context.Set<Transaction>().Remove(transaction);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<decimal> GetTotalBalanceAsync(long accountId)
        {
            // Optimized query to directly sum transaction amounts
            return await _context.Set<Transaction>()
                .Where(t => t.AccountId == accountId)
                .SumAsync(t => t.Amount * (t.IsDebit ? -1 : 1));
        }

        public async Task<IEnumerable<Transaction>> TransactionsByAccountIdAsync(long accountNo)
        {
            return await _context.Set<Transaction>().Where(a => a.AccountId == accountNo).ToListAsync();
        }
    }
}
