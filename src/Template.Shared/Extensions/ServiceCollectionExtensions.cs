using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Template.Application.DependencyInjection;
using Template.Infrastructure.DependencyInjection;
using Template.Persistence.DependencyInjection;

namespace Template.Shared.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddProjectModules(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplicationServices();
        services.AddInfrastructureServices();
        services.AddPersistenceServices(configuration.GetConnectionString("DefaultConnection")!);
        // Diğer modüller ve konfigürasyonlar buraya eklenebilir
        return services;
    }
} 