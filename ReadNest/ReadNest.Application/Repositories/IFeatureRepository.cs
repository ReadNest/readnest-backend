using ReadNest.Domain.Entities;

namespace ReadNest.Application.Repositories
{
    public interface IFeatureRepository : IGenericRepository<Feature, Guid>
    {
        Task<List<Guid>> GetInvalidFeatureIdsAsync(List<Guid> featureIds);
    }
}
