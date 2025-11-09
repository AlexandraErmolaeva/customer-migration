using Application.Dependencies.DataAccess;
using Application.Dependencies.DataAccess.Repositories;
using Application.Dependencies.Services;
using Infrastructure.ApplicationDependencies.DataAccess;
using Infrastructure.ApplicationDependencies.DataAccess.Repositories;
using Infrastructure.ApplicationDependencies.Services;
using Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ConfigureServices
{
    public static void AddInfrastrucureServices(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
    {
        services.ConfigureDbContext(configuration, isDevelopment);
        services.AddRepositories();
        services.AddApplicationDependencies();
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IContactsRepository, ContactsRepository>();
        services.AddScoped<IFinancialProfileRepository, FinancialProfileRepository>();
    }

    private static void AddApplicationDependencies(this IServiceCollection services)
    {
        services.AddSingleton<IExcelCustomersReader, ExcelCustomersReader>();
        services.AddScoped<ICustomerBatchProcessor, CustomerBatchProcessor>();
    }
}
