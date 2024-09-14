using Azure.Storage.Blobs;
using CIDA.Api.Configuration.HealthChecks;
using CIDA.Api.Configuration.Routes;
using Cida.Data;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

#region Database

builder.Services.AddDbContext<CidaDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("AzureConnection"));
});

#endregion

#region Azure Blob Storage

// add singleton azure blob service
builder.Services.AddSingleton(x =>
{
    var connectionString = builder.Configuration.GetConnectionString("AzureStorage");
    return new BlobServiceClient(connectionString);
});

#endregion

// add HttpClient
builder.Services.AddHttpClient();

// HealthChecks
builder.Services.ConfigureHealthChecks(builder.Configuration);

#region Swagger Configuration

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

#endregion

var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});
//}

app.MapHealthChecks("/api/health", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.UseHealthChecksUI(options => { options.UIPath = "/healthcheck-ui"; });


app.MapGet("/", () =>
{
    // redirect to swagger
    return Results.Redirect("/swagger");
});
app.MapUsuarioEndpoints();
app.MapResumoEndpoints();
app.MapInsightEndpoints();
app.MapLoginEndpoints();
app.MapArquivoEndpoints();
app.Run();