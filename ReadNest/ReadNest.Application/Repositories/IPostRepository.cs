﻿using ReadNest.Domain.Entities;

namespace ReadNest.Application.Repositories
{
    public interface IPostRepository : IGenericRepository<Post, Guid>
    {
        Task<IEnumerable<Post>> GetAllPostsAsync();
        Task<Post> GetPostWithLikesByIdAsync(Guid postId);
        Task<IEnumerable<Post>> GetPostsByUserIdAsync(Guid userId);
        Task<int> GetPostCountByUserIdAsync(Guid userId);
        Task<IEnumerable<Post>> GetPostsByBookIdAsync(Guid bookId);
        Task<IEnumerable<Post>> GetTopMostLikedPostsAsync(int count);
        Task<IEnumerable<Post>> GetTopMostViewedPostsAsync(int count);
        IQueryable<Post> GetQueryableWithIncludes();

        Task<List<Guid>> GetUserIdsWithPostsInTimeRange(DateTime from, DateTime to);
        Task<int> CountPostsByUserInEventAsync(Guid userId, DateTime from, DateTime to);
        Task<int> CountLikesByUserInEventAsync(Guid userId, DateTime from, DateTime to);
        Task<int> CountViewsByUserInEventAsync(Guid userId, DateTime from, DateTime to);
    }
}
