using Microsoft.EntityFrameworkCore;

namespace CQRS.Pattern.Infastructure.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity, TDbContext>
    where TEntity : class
    where TDbContext : DbContext
    {
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        Task<bool> SaveChangesAsync();
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(object id);
        Task RemoveAsync(object id);
    }

}
