using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ReadNest.Application.UseCases.Implementations.AffiliateLink;
using ReadNest.Application.UseCases.Implementations.Auth;
using ReadNest.Application.UseCases.Implementations.Book;
using ReadNest.Application.UseCases.Implementations.User;
using ReadNest.Application.UseCases.Interfaces.AffiliateLink;
using ReadNest.Application.UseCases.Interfaces.Auth;
using ReadNest.Application.UseCases.Interfaces.Book;
using ReadNest.Application.UseCases.Interfaces.User;
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
