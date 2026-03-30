using metrica_back.Data;
using Microsoft.EntityFrameworkCore;

namespace metrica_back.Extensions;

public static class ApplicationExtensions
{
    public static async Task InitializePostgresAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await context.Database.MigrateAsync();
    }

    public static async Task InitializeClickHouseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ClickHouseContext>();
        await context.InitializeAsync();
    }

    public static async Task InitializeAllDatabasesAsync(this WebApplication app)
    {
        await app.InitializePostgresAsync();
        await app.InitializeClickHouseAsync();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseCors();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.MapControllers();

        return app;
    }
}
