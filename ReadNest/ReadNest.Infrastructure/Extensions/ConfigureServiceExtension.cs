using Microsoft.Extensions.DependencyInjection;
using ReadNest.Application.Repositories;
using ReadNest.Application.Services;
using ReadNest.Infrastructure.Persistence.Repositories;
using ReadNest.Infrastructure.Services;

namespace ReadNest.Infrastructure.Extensions
{
    public static class ConfigureServiceExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            _ = services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
            _ = services.AddScoped<IUserRepository, UserRepository>();
            _ = services.AddScoped<IBookRepository, BookRepository>();
            _ = services.AddScoped<ICategoryRepository, CategoryRepository>();
            _ = services.AddScoped<IAffiliateLinkRepository, AffiliateLinkRepository>();
            _ = services.AddScoped<IFavoriteBookRepository, FavoriteBookRepository>();
            _ = services.AddScoped<ICommentRepository, CommentRepository>();
            _ = services.AddScoped<ICommentReportRepository, CommentReportRepository>();
            _ = services.AddScoped<IPostRepository, PostRepository>();
            _ = services.AddScoped<IBadgeRepository, BadgeRepository>();
            _ = services.AddScoped<IUserBadgeRepository, UserBadgeRepository>();
            _ = services.AddScoped<IEventRepository, EventRepository>();
            _ = services.AddScoped<IEventRewardRepository, EventRewardRepository>();
            _ = services.AddScoped<ILeaderboardRepository, LeaderboardRepository>();
            _ = services.AddScoped<IChatMessageRepository, ChatMessageRepository>();
            _ = services.AddScoped<ITradingPostRepository, TradingPostRepository>();
            _ = services.AddScoped<ITradingRequestRepository, TradingRequestRepository>();
            _ = services.AddScoped<IFeatureRepository, FeatureRepository>();

            _ = services.AddScoped<IJwtService, JwtService>();
            // Redis tracker
            _ = services.AddScoped<IViewTracker, RedisViewTracker>();
            _ = services.AddScoped<IRedisChatQueue, RedisChatQueue>();

            return services;
        }
    }
}
