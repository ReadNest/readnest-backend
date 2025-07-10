using Microsoft.EntityFrameworkCore;
using ReadNest.Application.Repositories;
using ReadNest.Domain.Entities;
using ReadNest.Infrastructure.Persistence.DBContext;

namespace ReadNest.Infrastructure.Persistence.Repositories
{
    public class FeatureRepository(AppDbContext context) : GenericRepository<Feature, Guid>(context), IFeatureRepository
    {
        public async Task<List<Guid>> GetInvalidFeatureIdsAsync(List<Guid> featureIds)
        {
            var validFeatureIds = await _context.Features
                .Where(f => featureIds.Contains(f.Id))
                .Select(f => f.Id)
                .ToListAsync();

            return featureIds.Except(validFeatureIds).ToList(); 
        }
    }
}
