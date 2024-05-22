using CardGuardian.Application.Services;
using CardGuardian.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace CardGuardian.Infrastructure.Repositories
{
    public abstract class GenericRepository<T, TId> : IGenericService<T, TId> where T : class
    {
        protected readonly ApplicationDbContext _dbContext;
        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<T> GetByIdAsync(TId id)
        {
            try
            {
                return await _dbContext.Set<T>().FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                return await _dbContext.Set<T>().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task AddAsync(T entity)
        {
            try
            {
                _dbContext.Set<T>().Add(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteAsync(TId id)
        {
            try
            {
                T entityToDelete = await GetByIdAsync(id);
                if (entityToDelete != null)
                {
                    _dbContext.Set<T>().Remove(entityToDelete);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
