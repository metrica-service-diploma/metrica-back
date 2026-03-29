using System.Text;
using metrica_back.Data;
using metrica_back.Repositories;
using metrica_back.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace metrica_back.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
        );
        services.AddSingleton<ClickHouseContext>();

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IWebsiteRepository, WebsiteRepository>();
        services.AddScoped<ITrackingEventRepository, TrackingEventRepository>();

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();

        return services;
    }

    public static IServiceCollection AddCustomCors(this IServiceCollection services)
    {
        services.AddCors(opts =>
        {
            opts.AddDefaultPolicy(policy =>
            {
                policy
                    .WithMethods(["GET", "POST", "PUT", "DELETE"])
                    .AllowAnyOrigin()
                    .AllowAnyHeader();
            });
        });

        return services;
    }

    public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition(
                "Bearer",
                new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter JWT token in format: Bearer {token}",
                }
            );

            options.AddSecurityRequirement(
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                        },
                        Array.Empty<string>()
                    },
                }
            );
        });

        return services;
    }

    public static IServiceCollection AddCustomAuthentication(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddAuthorization();

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:Secret"])
                    ),
                    ValidateIssuerSigningKey = true,
                };
            });

        return services;
    }
}
