using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ReadNest.Application.UseCases.Implementations.Auth;
using ReadNest.Application.UseCases.Implementations.User;
using ReadNest.Application.UseCases.Interfaces.Auth;
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
