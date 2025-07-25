﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            _ = services.AddConfigureGemini(configuration);
            _ = services.AddServices();

            return services;
        }

    }
}
