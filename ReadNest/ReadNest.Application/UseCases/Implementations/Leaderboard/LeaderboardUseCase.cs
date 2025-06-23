using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ReadNest.Application.Models.Responses.Leaderboard;
using ReadNest.Application.Models.Responses.User;
using ReadNest.Application.Repositories;
using ReadNest.Application.UseCases.Interfaces.Leaderboard;
using ReadNest.Shared.Common;

namespace ReadNest.Application.UseCases.Implementations.Leaderboard
{
    public class LeaderboardUseCase : ILeaderboardUseCase
    {
        private readonly ILeaderboardRepository _leaderboardRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IPostRepository _postRepository;

        public LeaderboardUseCase(ILeaderboardRepository leaderboardRepository, IEventRepository eventRepository, IPostRepository postRepository)
        {
            _leaderboardRepository = leaderboardRepository;
            _eventRepository = eventRepository;
            _postRepository = postRepository;
        }

        public async Task<ApiResponse<IEnumerable<LeaderboardResponse>>> GetTopNAsync(Guid eventId, int top)
        {
            var list = await _leaderboardRepository.GetTopNAsync(eventId, top);
            var result = list.Select(ToResponse);
            return ApiResponse<IEnumerable<LeaderboardResponse>>.Ok(result);
        }

        public async Task<ApiResponse<LeaderboardResponse?>> GetUserLeaderboardAsync(Guid eventId, Guid userId)
        {
            var entity = await _leaderboardRepository.GetUserLeaderboardAsync(eventId, userId);
            if (entity == null)
                return ApiResponse<LeaderboardResponse?>.Fail("User is not in leaderboard.");

            return ApiResponse<LeaderboardResponse?>.Ok(ToResponse(entity));
        }

        public async Task<ApiResponse<IEnumerable<LeaderboardResponse>>> GetTopByTimeRangeAsync(DateTime from, DateTime to, int top)
        {
            var list = await _leaderboardRepository.GetTopByTimeRangeAsync(from, to, top);
            var result = list.Select(ToResponse);
            return ApiResponse<IEnumerable<LeaderboardResponse>>.Ok(result);
        }

        public async Task<ApiResponse<LeaderboardRankResponse>> GetUserRankAsync(Guid eventId, Guid userId)
        {
            var all = await _leaderboardRepository.GetTopNAsync(eventId, int.MaxValue);
            var ranked = all
                .OrderByDescending(x => x.Score)
                .Select((entry, index) => new { entry.UserId, Rank = index + 1 })
                .FirstOrDefault(x => x.UserId == userId);

            if (ranked == null)
                return ApiResponse<LeaderboardRankResponse>.Fail("User not found in leaderboard.");

            return ApiResponse<LeaderboardRankResponse>.Ok(new LeaderboardRankResponse
            {
                UserId = userId,
                Rank = ranked.Rank
            });
        }

        private LeaderboardResponse ToResponse(Domain.Entities.Leaderboard l)
        {
            return new LeaderboardResponse
            {
                UserId = l.UserId,
                TotalPosts = l.TotalPosts,
                TotalLikes = l.TotalLikes,
                TotalViews = l.TotalViews,
                Score = l.Score,
                Rank = l.Rank,
                User = new GetUserResponse
                {
                    UserId = l.User.Id,
                    UserName = l.User.UserName,
                    FullName = l.User.FullName,
                    AvatarUrl = l.User.AvatarUrl
                }
            };
        }

        public async Task<ApiResponse<string>> RecalculateLeaderboardScoresAsync(Guid eventId)
        {
            var leaderboards = (await _leaderboardRepository.FindAsync(
                predicate: l => l.EventId == eventId && !l.IsDeleted,
                include: q => q.Include(l => l.User)
            )).ToList();

            var eventEntity = (await _eventRepository.FindAsync(
                predicate: query => query.Id == eventId && !query.IsDeleted)
            ).FirstOrDefault();

            if (eventEntity == null)
                return ApiResponse<string>.Fail("Event not found.");

            // Step 1: Lấy danh sách user có bài viết trong thời gian của event
            var userIdsWithPosts = await _postRepository.GetUserIdsWithPostsInTimeRange(eventEntity.StartDate, eventEntity.EndDate);

            if (userIdsWithPosts == null || !userIdsWithPosts.Any())
                return ApiResponse<string>.Fail("No users with posts found for this event.");

            // Step 2: Tạo leaderboard mới cho các user chưa có entry
            var existingUserIds = leaderboards.Select(l => l.UserId).ToHashSet();
            var newEntries = new List<Domain.Entities.Leaderboard>();

            foreach (var userId in userIdsWithPosts)
            {
                if (!existingUserIds.Contains(userId))
                {
                    var entry = new Domain.Entities.Leaderboard
                    {
                        Id = Guid.NewGuid(),
                        EventId = eventId,
                        UserId = userId,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        IsDeleted = false
                    };
                    newEntries.Add(entry);
                }
            }

            if (newEntries.Any())
            {
                await _leaderboardRepository.AddRangeAsync(newEntries);
                await _leaderboardRepository.SaveChangesAsync();

                leaderboards.AddRange(newEntries);
            }

            // Step 3: Tính toán lại thống kê cho toàn bộ leaderboard
            foreach (var entry in leaderboards)
            {
                int totalPosts = await _postRepository.CountPostsByUserInEventAsync(entry.UserId, eventEntity.StartDate, eventEntity.EndDate);
                int totalLikes = await _postRepository.CountLikesByUserInEventAsync(entry.UserId, eventEntity.StartDate, eventEntity.EndDate);
                int totalViews = await _postRepository.CountViewsByUserInEventAsync(entry.UserId, eventEntity.StartDate, eventEntity.EndDate);

                entry.TotalPosts = totalPosts;
                entry.TotalLikes = totalLikes;
                entry.TotalViews = totalViews;

                entry.Score = totalPosts * 10 + totalLikes * 2 + totalViews * 1;
                entry.UpdatedAt = DateTime.UtcNow;
            }

            // Step 4: Gán lại thứ hạng (rank)
            var sorted = leaderboards.OrderByDescending(l => l.Score).ToList();
            for (int i = 0; i < sorted.Count; i++)
            {
                sorted[i].Rank = i + 1;
            }

            await _leaderboardRepository.UpdateRangeAsync(leaderboards);
            await _leaderboardRepository.SaveChangesAsync();

            return ApiResponse<string>.Ok("Leaderboard scores recalculated successfully.");
        }

    }
}
