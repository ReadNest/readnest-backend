using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ReadNest.Application.UseCases.Implementations.AffiliateLink;
using ReadNest.Application.UseCases.Implementations.Auth;
using ReadNest.Application.UseCases.Implementations.Book;
using ReadNest.Application.UseCases.Implementations.Category;
using ReadNest.Application.UseCases.Implementations.FavoriteBook;
using ReadNest.Application.UseCases.Implementations.Comment;
using ReadNest.Application.UseCases.Implementations.User;
using ReadNest.Application.UseCases.Interfaces.AffiliateLink;
using ReadNest.Application.UseCases.Interfaces.Auth;
using ReadNest.Application.UseCases.Interfaces.Book;
using ReadNest.Application.UseCases.Interfaces.Category;
using ReadNest.Application.UseCases.Interfaces.FavoriteBook;
using ReadNest.Application.UseCases.Interfaces.Comment;
using ReadNest.Application.UseCases.Interfaces.User;
using ReadNest.Application.Validators.Auth;
using ReadNest.Application.UseCases.Interfaces.Post;
using ReadNest.Application.UseCases.Implementations.Post;

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
            _ = services.AddScoped<IPostUseCase, PostUseCase>();

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
