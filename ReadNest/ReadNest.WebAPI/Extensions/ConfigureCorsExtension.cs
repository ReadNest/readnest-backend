namespace ReadNest.WebAPI.Extensions
{
    public static class ConfigureCorsExtension
    {
        public static IServiceCollection AddCustomCors(this IServiceCollection services)
        {
            _ = services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
                    policy =>
                    {
                        _ = policy.WithOrigins("http://localhost:5173", "https://readnest.id.vn")
                              .AllowAnyHeader()
                              .AllowAnyMethod()
                              .AllowCredentials();
                    });
            });

            return services;
        }

    }
}
