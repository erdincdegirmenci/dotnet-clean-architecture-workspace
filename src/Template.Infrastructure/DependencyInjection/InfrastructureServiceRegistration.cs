using Microsoft.Extensions.DependencyInjection;
using Template.Application.Managers;
using Template.Infrastructure.Caching;
using Template.Infrastructure.Elastic;
using Template.Infrastructure.Events;
using Template.Infrastructure.Helper;
using Template.Infrastructure.Logging;
using Template.Infrastructure.Managers;
using Template.Infrastructure.Security;

namespace Template.Infrastructure.DependencyInjection;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        // Repository ve UnitOfWork
        // services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        // services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddTransient<IUserContextService, UserContextService>();
        services.AddTransient<IUserContextModel, UserContextModel>();
        services.AddTransient(typeof(IUserContextManager<IUserContextModel>), typeof(UserContextManager));
        services.AddTransient<IJwtTokenHandler, JwtTokenHandler>();
        services.AddTransient<JwtHelper>();
        services.AddTransient<CustomJwtEventHandler>();
        // Kafka, Elastic örnekleri
        services.AddSingleton<KafkaEventListener>();
        services.AddSingleton<ElasticSearchService>();
        services.AddScoped(typeof(ILogManager<>), typeof(LogManager<>));
        services.AddSingleton<IConfigManager, ConfigManager>();
        services.AddScoped<ICacheManager, MemoryCacheManager>();
        services.AddScoped<ITransactionContextManager, TransactionContextManager>();
        // Diğer altyapı servisleri buraya eklenebilir

        return services;
    }
} 