using ColorsApi.Database;
using Microsoft.EntityFrameworkCore;

namespace ColorsApi.Configurations;

public static partial class ServiceCollectionExtensions
{
    public static WebApplicationBuilder AddDatabase(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<ColorsDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("ColorsDb")));

        return builder;
    }

    public static async Task ApplyMigrationsAsync(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();
        await using ColorsDbContext applicationDbContext =
            scope.ServiceProvider.GetRequiredService<ColorsDbContext>();

        try
        {
            await applicationDbContext.Database.MigrateAsync();
            app.Logger.LogInformation("ColorsDatabase migrated successfully.");
        }
        catch (Exception ex)
        {
            app.Logger.LogError(ex, "An error occurred while migrating the ColorsDatabase.");
            throw;
        }
    }
}
