using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSK.Končar.WeatherApp.Server.Endpoints;
using OpenWeatherMap;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<ApplicationDBContext>(
    "postgresdb",
    null,
    options => options.EnableDetailedErrors().EnableSensitiveDataLogging());
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddEntityFrameworkStores<ApplicationDBContext>();

//builder.Services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);

builder.Services.AddAuthorization();

builder.Services.AddSingleton<IOpenWeatherMapService>(x =>
{
    var logger = x.GetRequiredService<ILogger<OpenWeatherMapService>>();
    var options = new OpenWeatherMapOptions
    {
        ApiKey = builder.Configuration["OpenWeatherMap:ApiKey"] ?? throw new InvalidOperationException("OpenWeatherMap API key is not configured."),
        Language = "hr"
    };
    return new OpenWeatherMapService(logger, options);
});

// Add services to the container.
builder.Services.AddProblemDetails();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseMigrationsEndPoint();
}

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapGroup("/identity").MapIdentityApi<IdentityUser>();

var api = app.MapGroup("/api").RequireAuthorization();
api.MapWeatherEndpoints();
api.MapStatisticsEndpoints();

app.MapPost("/logout", async (SignInManager<IdentityUser> signInManager,
    [FromBody] object empty) =>
{
    if (empty != null)
    {
        await signInManager.SignOutAsync();
        return Results.Ok();
    }
    return Results.Unauthorized();
})
.RequireAuthorization();

app.MapDefaultEndpoints();

app.UseFileServer();

using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
await context.Database.MigrateAsync();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
