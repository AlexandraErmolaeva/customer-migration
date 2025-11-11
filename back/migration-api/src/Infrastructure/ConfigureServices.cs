using Application.Dependencies.DataAccess;
using Application.Dependencies.DataAccess.Repositories;
using Application.Dependencies.Services;
using Infrastructure.ApplicationDependencies.DataAccess;
using Infrastructure.ApplicationDependencies.DataAccess.Repositories;
using Infrastructure.ApplicationDependencies.Services;
using Infrastructure.HostedServices;
using Infrastructure.HostedServices.Config;
using Infrastructure.InternalDependencies.Serializers;
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

        services.AddSingleton<IXmlSerealizer, XmlSerializer>();

        services.AddHostedService<XmlSynchronizer>();
        services.AddHostedService<XmlGenerator>();
        services.Configure<XmlSynchronizerConfig>(configuration.GetSection(nameof(XmlSynchronizerConfig)));
        services.Configure<XmlGeneratorConfig>(configuration.GetSection(nameof(XmlGeneratorConfig)));
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
        services.AddScoped<ICustomerPersistenceService, CustomerPersistenceService>();
    }
}
