using Auth.Business;
using Auth.Business.Contracts.Requests;
using Auth.Business.Services;
using Auth.Data;
using Auth.Data.Persistence;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks()
    .AddDbContextCheck<AuthDbContext>("database", tags: new[] { "ready" });

builder.Services.AddBusiness();
builder.Services.AddData(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection();
}

app.MapPost("/api/auth/login", async (LoginRequest request, IAuthService authService, CancellationToken cancellationToken) =>
{
    var response = await authService.ValidateCredentialsAsync(request, cancellationToken);
    return response.IsValid
        ? Results.Ok(response)
        : Results.Json(response, statusCode: StatusCodes.Status401Unauthorized);
})
.WithName("Login")
.WithOpenApi();

app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = healthCheck => healthCheck.Tags.Contains("ready"),
    ResultStatusCodes =
    {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Degraded] = StatusCodes.Status503ServiceUnavailable,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
    }
});

app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = _ => false
});

await AuthDbInitializer.InitialiseAsync(app.Services);

app.Run();
