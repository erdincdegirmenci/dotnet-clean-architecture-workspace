using Microsoft.Extensions.DependencyInjection;
using Template.Identity.Services;

namespace Template.Identity.DependencyInjection;

public static class IdentityServiceRegistration
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services)
    {
        services.AddScoped<JwtTokenGenerator>();
        // Diğer kimlik servisleri buraya eklenebilir
        return services;
    }
} 