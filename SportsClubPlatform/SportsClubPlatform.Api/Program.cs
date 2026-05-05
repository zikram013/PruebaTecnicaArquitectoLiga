using Microsoft.OpenApi.Models;
using SportsClubPlatform.Api.Middleware;
using SportsClubPlatform.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Sports Club Platform API",
        Version = "v1",
        Description = "Proof of Concept API for a sports club SaaS platform. Includes transfer orchestration, audit timeline and seeded demo data."
    });
});

builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.DocumentTitle = "Sports Club Platform API";
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Sports Club Platform API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.Services.InitializeDatabaseAsync();

app.Run();