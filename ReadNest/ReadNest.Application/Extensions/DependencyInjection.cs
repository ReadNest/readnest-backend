using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ReadNest.Application.Validators.Auth;

namespace ReadNest.Application.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            _ = services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();

            return services;
        }
    }
}
