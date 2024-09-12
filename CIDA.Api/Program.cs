using System.Text.Json.Serialization;
using CIDA.Api;
using CIDA.Api.Configuration.HealthChecks;
using CIDA.Api.Configuration.Routes;
using Cida.Data;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CidaDbContext>(options =>
{
    options.UseOracle(builder.Configuration.GetConnectionString("FiapOracleConnection"));
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
    options.ExampleFilters();
});

builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
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
app.MapResumoEndpoints();
app.MapInsightEndpoints();
app.MapLoginEndpoints();
app.Run();