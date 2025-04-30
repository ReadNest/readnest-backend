using System.Linq.Expressions;

namespace ReadNest.Application.Repositories
{
    /// <summary>
    /// IGenericRepository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface IGenericRepository<T, TKey> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(bool asNoTracking = true);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, bool asNoTracking = true);
        Task<T?> GetByIdAsync(TKey id);
        Task<T> AddAsync(T entity);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);
        Task UpdateAsync(T entity);
        Task UpdateRangeAsync(IEnumerable<T> entities);
        Task DeleteAsync(T entity);
        Task DeleteByIdAsync(TKey id);
        Task DeleteRangeAsync(IEnumerable<T> entities);
        Task<int> CountAsync();
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
        void SaveChanges();
        Task SaveChangesAsync();
        Task<IEnumerable<T>> GetAllWithIncludeAsync(Func<IQueryable<T>, IQueryable<T>>? include = null, bool asNoTracking = true);
        Task<IEnumerable<T>> FindWithIncludeAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IQueryable<T>>? include = null, bool asNoTracking = true);
        Task<IEnumerable<T>> GetPagedAsync(int pageNumber, int pageSize, bool asNoTracking = true);
        Task<IEnumerable<T>> FindPagedAsync(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize, bool asNoTracking = true);
        Task<IEnumerable<T>> FindWithIncludePagedAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IQueryable<T>>? include, int pageNumber, int pageSize, bool asNoTracking = true);
    }
}
