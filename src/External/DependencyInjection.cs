using metrica_back.src.Core.Interfaces.Repositories;
using metrica_back.src.Core.Interfaces.Services;
using metrica_back.src.External.Databases.PostgreSql;
using metrica_back.src.External.Repositories;
using metrica_back.src.External.Services;
using Microsoft.EntityFrameworkCore;

namespace metrica_back.src.External;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        // Подключение БД
        services.AddDbContext<PostgreSqlContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("PostgreSql"))
        );

        // Репозитории
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IWebsiteRepository, WebsiteRepository>();

        // Сервисы
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IDatabaseSeeder, DatabaseSeeder>();

        // Хостированный сервис для автоматического сидинга при старте
        services.AddHostedService<DatabaseInitializationHostedService>();

        return services;
    }
}
