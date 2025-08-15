using Microsoft.Extensions.DependencyInjection;
using Template.Application.Interfaces;
using Template.Application.Mapping;
using Template.Application.Repositories;
using Template.Application.Services;

namespace Template.Application.DependencyInjection;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<InMemoryUserRepository, InMemoryUserRepository>();
        // Diğer servisler buraya eklenebilir
        return services;
    }

    public static IServiceCollection AddMappingProfiles(this IServiceCollection services)
    {
        // Tüm mapping profilleri burada eklenebilir
        services.AddAutoMapper(typeof(UserMappingProfile).Assembly);
        return services;
    }
} 