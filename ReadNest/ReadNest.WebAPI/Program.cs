using Microsoft.AspNetCore.SignalR;
using ReadNest.Application.Extensions;
using ReadNest.BackgroundServices;
using ReadNest.Infrastructure.Extensions;
using ReadNest.WebAPI.Extensions;
using ReadNest.WebAPI.Hubs;
using ReadNest.WebAPI.Middlewares;

namespace ReadNest.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            _ = builder.Services.AddApplication();
            _ = builder.Services.AddInfrastructure(builder.Configuration);
            _ = builder.Services.AddWebAPI(builder.Configuration);

            _ = builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            _ = builder.Services.AddEndpointsApiExplorer();
            _ = builder.Services.AddSwaggerGen();
            _ = builder.Services.AddHostedService<ChatMessageSaver>();
            _ = builder.Services.AddHostedService<EmailInvoiceConsumer>();

            // SignalR
            _ = builder.Services.AddSignalR();
            _ = builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                _ = app.UseSwagger();
                _ = app.UseSwaggerUI();
            }

            _ = app.UseValidationExceptionMiddleware();

            _ = app.UseHttpsRedirection();

            _ = app.UseCors("AllowFrontend");

            _ = app.UseAuthentication();

            _ = app.UseAuthorization();

            _ = app.MapControllers();

            _ = app.MapHub<ChatHub>("/chatHub");

            app.Run();
        }
    }
}
