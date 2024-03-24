using CQRS.Pattern.Infastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CQRS.Pattern.Infastructure.Repositories.Implementation
{
    public class GenericRepository<TEntity, TDbContext> : IGenericRepository<TEntity, TDbContext>
    where TEntity : class
    where TDbContext : DbContext
    {
        private readonly TDbContext _dbContext;

        public GenericRepository(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual async Task AddAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await _dbContext.Set<TEntity>().AddAsync(entity);
        }

        public virtual void Update(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual async Task<bool> SaveChangesAsync()
        {
            try
            {
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Console.WriteLine($"Error saving changes - {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error saving changes - {ex.Message}");
                return false;
            }
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbContext.Set<TEntity>().ToListAsync();
        }

        public virtual async Task<TEntity> GetByIdAsync(object id)
        {
            return await _dbContext.Set<TEntity>().FindAsync(id);
        }

        public virtual async Task RemoveAsync(object id)
        {
            var entity = await _dbContext.Set<TEntity>().FindAsync(id);
            if (entity != null)
            {
                _dbContext.Set<TEntity>().Remove(entity);
            }
        }

        public virtual async Task SoftDeleteAsync(object id)
        {
            var entity = await _dbContext.Set<TEntity>().FindAsync(id);
            if (entity != null)
            {
                _dbContext.Entry(entity).Property("Deleted").CurrentValue = true;
                _dbContext.Entry(entity).State = EntityState.Modified;
            }
        }
    }
}