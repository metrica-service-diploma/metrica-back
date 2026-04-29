namespace metrica_back.src.External.Configs;

public static class CorsConfig
{
    public static IServiceCollection AddCustomCors(this IServiceCollection services)
    {
        services.AddCors(opts =>
        {
            opts.AddDefaultPolicy(policy =>
            {
                policy
                    .WithMethods("GET", "POST", "PUT", "DELETE")
                    .SetIsOriginAllowed(origin => true)
                    .AllowAnyHeader()
                    .AllowCredentials();
                ;
            });
        });

        return services;
    }
}
