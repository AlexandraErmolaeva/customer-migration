using Application.Dependencies.Logging;
using Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence;

public static class DbContextConfiguration
{
    public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
    {
        var connectionString = Environment.GetEnvironmentVariable("DefaultConnection")!;
        services.AddDbContext<CustomersDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
            if (isDevelopment)
                options.EnableSensitiveDataLogging();
        });
    }

    public static async Task ApplyMigrationsAsync(this IApplicationBuilder app, IConfiguration configuration)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var services = scope.ServiceProvider;

        try
        {
            var context = services.GetRequiredService<CustomersDbContext>();
            if (configuration.GetValue<bool>("AutoMigrations"))
                await context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILoggerManager>();
            logger.LogError($"Не удалось применить миграции при запуске приложения. Ошибка {ex.Message}, {ex.StackTrace}.");
            throw;
        }
    }
}
