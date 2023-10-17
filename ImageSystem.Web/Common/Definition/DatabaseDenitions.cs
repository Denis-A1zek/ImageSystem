using ImageSystem.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ImageSystem.Web.Common.Definition;

public static class DatabaseDenitions
{
    public static void MigrateDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<PostgreContext>();
        context.Database.Migrate();
    }
}
