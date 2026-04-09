using metrica_back.src.Business.Interfaces.Repositories;
using metrica_back.src.Business.Interfaces.Services;
using metrica_back.src.External.Databases.ClickHouse;
using metrica_back.src.External.Databases.PostgreSql;
using metrica_back.src.External.HostedServices;
using metrica_back.src.External.Interfaces;
using metrica_back.src.External.Repositories;
using metrica_back.src.External.Services;
using Microsoft.EntityFrameworkCore;

namespace metrica_back.src.External;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration config
    )
    {
        // Добавления контекста PostgreSQL
        services.AddDbContext<PostgreSqlContext>(options =>
            options.UseNpgsql(config.GetConnectionString("PostgreSql"))
        );

        // Регистрация контекста и сервисов ClickHouse
        services.Configure<ClickHouseOptions>(config.GetSection("ClickHouse"));
        services.AddScoped<IClickHouseSchemaManager, ClickHouseSchemaManager>();

        // Фабрика для ClickHouseContext
        services.AddScoped(serviceProvider =>
            ActivatorUtilities.CreateInstance<ClickHouseContext>(
                serviceProvider,
                config.GetConnectionString("ClickHouse")
            )
        );

        // Репозитории
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IWebsiteRepository, WebsiteRepository>();
        services.AddScoped<ITrackingEventRepository, TrackingEventRepository>();

        // Сервисы
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<PostgreSqlSeeder>();
        services.AddScoped<ClickHouseSeeder>();

        // Хостированные сервисы
        services.AddHostedService<PostgreSqlInitHostedService>();
        services.AddHostedService<ClickHouseInitHostedService>();

        return services;
    }
}
