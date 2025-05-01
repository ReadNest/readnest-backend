using Microsoft.Extensions.DependencyInjection;
using ReadNest.Application.Services;
using ReadNest.Infrastructure.Services;

namespace ReadNest.Infrastructure.Extensions
{
    public static class ConfigureServiceExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            _ = services.AddScoped<IJwtService, JwtService>();

            return services;
        }
    }
}
