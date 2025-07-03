using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReadNest.Infrastructure.Options;
using StackExchange.Redis;

namespace ReadNest.Infrastructure.Extensions
{
    public static class ConfigureRedisExtension
    {
        public static IServiceCollection AddCustomRedis(this IServiceCollection services, IConfiguration configuration)
        {
            var redisOptions = configuration.GetSection(nameof(RedisOptions)).Get<RedisOptions>();
            var multiplexer = ConnectionMultiplexer.Connect(redisOptions.ConnectionStrings);
            _ = services.AddSingleton<IConnectionMultiplexer>(multiplexer);
            return services;
        }
    }
}
