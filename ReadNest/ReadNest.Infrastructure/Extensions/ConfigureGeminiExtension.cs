using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReadNest.Infrastructure.Options;
using ReadNest.Infrastructure.Services;

namespace ReadNest.Infrastructure.Extensions
{
    public static class ConfigureGeminiExtension
    {
        public static IServiceCollection AddConfigureGemini(this IServiceCollection services, IConfiguration configuration)
        {
            var geminiOptions = configuration.GetSection(nameof(GeminiOptions)).Get<GeminiOptions>();

            _ = services.AddHttpClient(geminiOptions.GoogleBooks, client =>
            {
                client.BaseAddress = new Uri(geminiOptions.GoogleApiLink);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            _ = services.AddSingleton(sp =>
            {
                return new GeminiClient(geminiOptions.ApiKey, geminiOptions.GenerativeApiLink);
            });

            return services;
        }
    }
}
