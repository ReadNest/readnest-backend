using ReadNest.Application.Repositories;

namespace ReadNest.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// GenericRepository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class GenericRepository<T, TKey> : IGenericRepository<T, TKey> where T : class
    {
    }
}
