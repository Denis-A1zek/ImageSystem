using ImageSystem.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ImageSystem.Infrastructure;

public static class Dependencies
{
    public static IServiceCollection AddInfrastructures(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString
            = configuration.GetConnectionString(nameof(PostgreContext));
        services.AddDbContext<PostgreContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();   

        return services;
    }
}
