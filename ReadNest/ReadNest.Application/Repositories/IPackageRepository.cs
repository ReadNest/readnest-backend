using ReadNest.Domain.Entities;

namespace ReadNest.Application.Repositories
{
    public interface IPackageRepository : IGenericRepository<Package, Guid>
    {
    }
}
