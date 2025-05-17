using Microsoft.Extensions.DependencyInjection;
using ReadNest.Application.Repositories;
using ReadNest.Application.Services;
using ReadNest.Infrastructure.Persistence.Repositories;
using ReadNest.Infrastructure.Services;

namespace ReadNest.Infrastructure.Extensions
{
    public static class ConfigureServiceExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            _ = services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
            _ = services.AddScoped<IUserRepository, UserRepository>();
            _ = services.AddScoped<IBookRepository, BookRepository>();
            _ = services.AddScoped<ICategoryRepository, CategoryRepository>();

            _ = services.AddScoped<IJwtService, JwtService>();

            return services;
        }
    }
}
