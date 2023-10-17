using ImageSystem.Core.Common.Utils;
using ImageSystem.Web.Common.Middlewares;
using ImageSystem.Web.Common.Utils;
using Microsoft.Extensions.FileProviders;

namespace ImageSystem.Web.Common.Definition;

public static class MiddlewareDefinition
{
    public static WebApplication Configure(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.MigrateDatabase();
        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();


        var imageStorage = string.IsNullOrEmpty(app.Configuration[ImageConstant.LocalPath]) ?
            Path.Combine(app.Environment.ContentRootPath, "Images")
            : app.Configuration[ImageConstant.LocalPath];

        app.UseStaticFiles(new StaticFileOptions
        {
            OnPrepareResponse = ctx =>
            {
                ctx.Context.Response.Headers.Append(
                     "Cache-Control", $"public, max-age={CacheControlUtils.MaxAge}");
            },
            DefaultContentType = "image/png",
            FileProvider = new PhysicalFileProvider(imageStorage),
            RequestPath = "/images",
        });

        app.MapControllers();
        app.UseMiddleware<CustomExceptionHandlerMiddleware>();

        return app;
    }
}
