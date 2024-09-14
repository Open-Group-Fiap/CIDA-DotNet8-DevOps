using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CIDA.Api.Configuration.HealthChecks;

public static class HealthCheck
{
    public static void ConfigureHealthChecks(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHealthChecks().AddSqlServer(
            configuration.GetConnectionString("AzureConnection") ?? string.Empty,
            healthQuery: "SELECT 1",
            name: "SQL Server Health Check",
            failureStatus: HealthStatus.Unhealthy,
            tags: new[] { "feedback", "database", "sqlserver" });


        services.AddHealthChecksUI(opt =>
        {
            opt.SetEvaluationTimeInSeconds(240);
            opt.MaximumHistoryEntriesPerEndpoint(60);
            opt.SetApiMaxActiveRequests(1);
            opt.AddHealthCheckEndpoint("feedback", "/api/health");
        }).AddInMemoryStorage();
    }
}