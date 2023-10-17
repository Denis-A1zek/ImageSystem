
using ImageSystem.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ImageSystem.Core;

public static class Dependencies
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddMediatR(config => config.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

        services.AddSingleton<IPathManager, PathManager>();
        services.AddTransient<IPasswordHasher, PasswordHasher>();

        return services;
    }
}
