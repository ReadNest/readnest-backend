using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReadNest.Infrastructure.Options;

namespace ReadNest.Infrastructure.Extensions
{
    public static class ConfigureOptionsExtension
    {
        public static IServiceCollection AddConfigureOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));
            services.Configure<DatabaseOptions>(configuration.GetSection(nameof(DatabaseOptions)));

            return services;
        }
    }
}
