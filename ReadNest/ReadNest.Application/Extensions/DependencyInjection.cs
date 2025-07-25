﻿using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ReadNest.Application.UseCases.Implementations.AffiliateLink;
using ReadNest.Application.UseCases.Implementations.Auth;
using ReadNest.Application.UseCases.Implementations.Badge;
using ReadNest.Application.UseCases.Implementations.Book;
using ReadNest.Application.UseCases.Implementations.Category;
using ReadNest.Application.UseCases.Implementations.ChatMessage;
using ReadNest.Application.UseCases.Implementations.Comment;
using ReadNest.Application.UseCases.Implementations.CommentReport;
using ReadNest.Application.UseCases.Implementations.Event;
using ReadNest.Application.UseCases.Implementations.EventReward;
using ReadNest.Application.UseCases.Implementations.FavoriteBook;
using ReadNest.Application.UseCases.Implementations.Feature;
using ReadNest.Application.UseCases.Implementations.Leaderboard;
using ReadNest.Application.UseCases.Implementations.Package;
using ReadNest.Application.UseCases.Implementations.Post;
using ReadNest.Application.UseCases.Implementations.Recommendation;
using ReadNest.Application.UseCases.Implementations.TradingPost;
using ReadNest.Application.UseCases.Implementations.Transaction;
using ReadNest.Application.UseCases.Implementations.User;
using ReadNest.Application.UseCases.Implementations.UserBadge;
using ReadNest.Application.UseCases.Implementations.UserSubscription;
using ReadNest.Application.UseCases.Interfaces.AffiliateLink;
using ReadNest.Application.UseCases.Interfaces.Auth;
using ReadNest.Application.UseCases.Interfaces.Badge;
using ReadNest.Application.UseCases.Interfaces.Book;
using ReadNest.Application.UseCases.Interfaces.Category;
using ReadNest.Application.UseCases.Interfaces.ChatMessage;
using ReadNest.Application.UseCases.Interfaces.Comment;
using ReadNest.Application.UseCases.Interfaces.CommentReport;
using ReadNest.Application.UseCases.Interfaces.Event;
using ReadNest.Application.UseCases.Interfaces.EventReward;
using ReadNest.Application.UseCases.Interfaces.FavoriteBook;
using ReadNest.Application.UseCases.Interfaces.Feature;
using ReadNest.Application.UseCases.Interfaces.Leaderboard;
using ReadNest.Application.UseCases.Interfaces.Package;
using ReadNest.Application.UseCases.Interfaces.Post;
using ReadNest.Application.UseCases.Interfaces.Recommendation;
using ReadNest.Application.UseCases.Interfaces.TradingPost;
using ReadNest.Application.UseCases.Interfaces.Transaction;
using ReadNest.Application.UseCases.Interfaces.User;
using ReadNest.Application.UseCases.Interfaces.UserBadge;
using ReadNest.Application.UseCases.Interfaces.UserSubscription;
using ReadNest.Application.Validators.Auth;

namespace ReadNest.Application.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddUseCases(this IServiceCollection services)
        {
            _ = services.AddScoped<IAuthenticationUseCase, AuthenticationUseCase>();
            _ = services.AddScoped<IUserUseCase, UserUseCase>();
            _ = services.AddScoped<IBookUseCase, BookUseCase>();
            _ = services.AddScoped<IAffiliateLinkUseCase, AffiliateLinkUseCase>();
            _ = services.AddScoped<ICategoryUseCase, CategoryUseCase>();
            _ = services.AddScoped<IFavoriteBookUseCase, FavoriteBookUseCase>();
            _ = services.AddScoped<ICommentUseCase, CommentUseCase>();
            _ = services.AddScoped<ICommentReportUseCase, CommentReportUseCase>();
            _ = services.AddScoped<IPostUseCase, PostUseCase>();
            _ = services.AddScoped<IBadgeUseCase, BadgeUseCase>();
            _ = services.AddScoped<IUserBadgeUseCase, UserBadgeUseCase>();
            _ = services.AddScoped<IEventUseCase, EventUseCase>();
            _ = services.AddScoped<IEventRewardUseCase, EventRewardUseCase>();
            _ = services.AddScoped<ILeaderboardUseCase, LeaderboardUseCase>();
            _ = services.AddScoped<IChatMessageUseCase, ChatMessageUseCase>();
            _ = services.AddScoped<ITradingPostUseCase, TradingPostUseCase>();
            _ = services.AddScoped<IFeatureUseCase, FeatureUseCase>();
            _ = services.AddScoped<IPackageUseCase, PackageUseCase>();
            _ = services.AddScoped<ITransactionUseCase, TransactionUseCase>();
            _ = services.AddScoped<IUserSubscriptionUseCase, UserSubscriptionUseCase>();
            _ = services.AddScoped<IRecommendationUseCase, RecommendationUseCase>();

            return services;
        }

        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            _ = services.AddUseCases();
            _ = services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();

            return services;
        }
    }
}
