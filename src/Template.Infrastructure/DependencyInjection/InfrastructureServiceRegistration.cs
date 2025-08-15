using Microsoft.Extensions.DependencyInjection;
using Template.Application.Managers;
using Template.Infrastructure.Authentication;
using Template.Infrastructure.Elastic;
using Template.Infrastructure.Events;
using Template.Infrastructure.Managers;

namespace Template.Infrastructure.DependencyInjection;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        // Repository ve UnitOfWork
        // services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        // services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<JwtTokenGenerator>();
        // Kafka, Elastic örnekleri
        services.AddSingleton<KafkaEventListener>();
        services.AddSingleton<ElasticSearchService>();
        services.AddSingleton<IConfigManager, ConfigManager>();
        services.AddScoped<ITransactionContextManager, TransactionContextManager>();
        // Diğer altyapı servisleri buraya eklenebilir

        return services;
    }
} 