using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Template.Application.Interfaces;
using Template.Application.Managers;
using Template.Domain.Interfaces;
using Template.Domain.QueryTemplate;
using Template.Persistence.Database;
using Template.Persistence.Repositories;

namespace Template.Persistence.DependencyInjection;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));
        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddTransient<IUserRepository, UserRepository>(x => new UserRepository(new MsSqlDatabaseManager("DBConnection", x.GetService<IConfigManager>()), new UserQueryTemplate()));

        return services;
    }
} 