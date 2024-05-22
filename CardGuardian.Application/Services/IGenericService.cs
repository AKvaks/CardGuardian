namespace CardGuardian.Application.Services
{
    public interface IGenericService<T, TId> where T : class
    {
        public Task<T> GetByIdAsync(TId id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task DeleteAsync(TId id);
    }
}
