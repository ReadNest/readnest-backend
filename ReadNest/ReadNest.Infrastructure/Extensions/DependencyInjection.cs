using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReadNest.Application.Services;
using ReadNest.Infrastructure.Services;
using StackExchange.Redis;

namespace ReadNest.Infrastructure.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            _ = services.AddConfigureOptions(configuration);
            _ = services.AddCustomDbContext(configuration);
            _ = services.AddCustomJwt(configuration);
            _ = services.AddCustomRedis(configuration);
            _ = services.AddServices();
            

            return services;
        }

    }
}
