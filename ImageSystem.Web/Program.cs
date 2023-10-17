using ImageSystem.Core;
using ImageSystem.Infrastructure;
using ImageSystem.Web;
using ImageSystem.Web.Common.Definition;
using ImageSystem.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddInfrastructures(builder.Configuration)
    .AddAuthDefinition(builder.Configuration)
    .AddSwaggerDefinition()
    .AddCore();

builder.Services.AddHttpContextAccessor();

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));

builder.Services.AddTransient<IImageCreator, ImageCreator>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddControllers();

var app = builder.Build();

app.Configure();

app.Run();
