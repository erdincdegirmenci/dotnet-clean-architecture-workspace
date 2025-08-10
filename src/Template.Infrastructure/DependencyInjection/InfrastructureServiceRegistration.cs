using Microsoft.Extensions.DependencyInjection;
using Template.Infrastructure.Authentication;
using Template.Infrastructure.Elastic;
using Template.Infrastructure.Events;

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
        // Diğer altyapı servisleri buraya eklenebilir
        return services;
    }
} 