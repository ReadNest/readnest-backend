using ReadNest.Domain.Entities;

namespace ReadNest.Application.Repositories
{
    public interface ICategoryRepository : IGenericRepository<Category, Guid>
    {
        Task<List<Guid>> FindCategoryIdsByBookIdAsync(Guid bookId);
        Task<List<Category>> FindByIdsAsync(IEnumerable<Guid> bookIds);
    }
}
