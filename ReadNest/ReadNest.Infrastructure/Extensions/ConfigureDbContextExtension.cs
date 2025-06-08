using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ReadNest.Infrastructure.Options;
using ReadNest.Infrastructure.Persistence.DBContext;

namespace ReadNest.Infrastructure.Extensions
{
    public static class ConfigureDbContextExtension
    {
        public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var databaseOptions = configuration.GetSection(nameof(DatabaseOptions)).Get<DatabaseOptions>();
            _ = services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(databaseOptions?.ConnectionStrings);
                options.EnableSensitiveDataLogging();
            });

            return services;
        }
    }
}
