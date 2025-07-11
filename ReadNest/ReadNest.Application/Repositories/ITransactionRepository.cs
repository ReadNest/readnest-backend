﻿using ReadNest.Domain.Entities;

namespace ReadNest.Application.Repositories
{
    public interface ITransactionRepository : IGenericRepository<Transaction, Guid>
    {
        Task<Transaction> GetByOrderCodeAsync(long orderCode);
    }
}
