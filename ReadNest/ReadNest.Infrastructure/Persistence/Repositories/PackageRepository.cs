using ReadNest.Application.Repositories;
using ReadNest.Domain.Entities;
using ReadNest.Infrastructure.Persistence.DBContext;

namespace ReadNest.Infrastructure.Persistence.Repositories
{
    public class PackageRepository(AppDbContext context) : GenericRepository<Package, Guid>(context), IPackageRepository
    {
    }
}
