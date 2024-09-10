using System.Text.Json.Serialization;
using CIDA.Api;
using CIDA.Api.Configuration.HealthChecks;
using CIDA.Api.Configuration.Routes;
using Cida.Data;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CidaDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("AzureConnection"));
});

builder.Services.ConfigureHealthChecks(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Cida API",
        Description = "Uma API para gerenciamento do banco de dados da CIDA",
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => { 
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.MapHealthChecks("/api/health", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.UseHealthChecksUI(options => { options.UIPath = "/healthcheck-ui"; });

app.MapUsuarioEndpoints();

app.Run();


