using Microsoft.EntityFrameworkCore;
using ReadNest.Application.Repositories;
using ReadNest.Domain.Entities;
using ReadNest.Infrastructure.Persistence.DBContext;

namespace ReadNest.Infrastructure.Persistence.Repositories
{
    public class TransactionRepository(AppDbContext context) : GenericRepository<Transaction, Guid>(context), ITransactionRepository
    {
        public async Task<Transaction> GetByOrderCodeAsync(long orderCode)
        {
            return await _context.Transactions
                .AsNoTracking()
                .Where(x => x.OrderCode == orderCode)
                .Include(x => x.Package)
                .FirstOrDefaultAsync();
        }
    }
}
