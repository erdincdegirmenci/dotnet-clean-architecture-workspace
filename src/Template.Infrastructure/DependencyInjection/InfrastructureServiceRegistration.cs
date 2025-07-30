using Microsoft.Extensions.DependencyInjection;
using Template.Infrastructure.Repositories;
using Template.Infrastructure.Events;
using Template.Infrastructure.Elastic;
using Microsoft.Extensions.Configuration;
using Template.Config;
using Template.Infrastructure.Kafka;

namespace Template.Infrastructure.DependencyInjection;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        // Repository ve UnitOfWork
        // services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        // services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Kafka, Elastic örnekleri
        services.AddSingleton<KafkaEventListener>();
        services.AddSingleton<ElasticSearchService>();
        // Diğer altyapı servisleri buraya eklenebilir
        return services;
    }
} 