using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ReadNest.WebAPI.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWebAPI(this IServiceCollection services, IConfiguration configuration)
        {
            
            return services;
        }

    }
}
