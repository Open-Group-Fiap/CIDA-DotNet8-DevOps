using CIDA.Api.Models;
using CIDA.Api.Models.Metadatas;
using Cida.Data;
using CIDA.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Filters;

namespace CIDA.Api.Configuration.Routes;

public static class InsightEndpoints
{
    public static void MapInsightEndpoints(this WebApplication app)
    {
        var insightGroup = app.MapGroup("/insight");

        #region Queries

        insightGroup.MapGet("/search", async (CidaDbContext db, int page = 1, int pagesize = 30) =>
            {
                var skip = (page - 1) * pagesize;
                var results = db.Insights.Skip(skip).Take(pagesize).ToList();
                return new InsightsListModel(
                    page,
                    pagesize,
                    results.Count,
                    results);
            })
            .Produces<InsightsListModel>()
            .WithName("GetInsights")
            .WithTags("Insight")
            .WithOpenApi();

        insightGroup.MapGet("/{id:int}", async (CidaDbContext db, int id) =>
            {
                var insight = await db.Insights.FindAsync(id);
                return insight == null ? Results.NotFound() : Results.Ok(insight);
            })
            .Produces<Insight>()
            .Produces(StatusCodes.Status404NotFound)
            .WithName("GetInsightById")
            .WithTags("Insight")
            .WithOpenApi(
                generatedOperation =>
                {
                    var parameter = generatedOperation.Parameters[0];
                    parameter.Description = "Id do insight a ser consultado";
                    parameter.Required = true;
                    return generatedOperation;
                }
            );

        insightGroup.MapGet("/{email}", async (CidaDbContext db, string email) =>
            {
                var usuario = await db.Usuarios.FirstOrDefaultAsync(x => x.Autenticacao.Email == email);
                if (usuario == null)
                {
                    return Results.NotFound();
                }

                var insight = await db.Insights.FirstOrDefaultAsync(x => x.IdUsuario == usuario.IdUsuario);

                return insight == null ? Results.NotFound() : Results.Ok(insight);
            })
            .Produces<Insight>()
            .Produces(StatusCodes.Status404NotFound)
            .WithName("GetInsightByEmail")
            .WithTags("Insight")
            .WithOpenApi(
                generatedOperation =>
                {
                    var parameter = generatedOperation.Parameters[0];
                    parameter.Description = "Email do insight a ser consultado";
                    parameter.Required = true;
                    return generatedOperation;
                }
            );

        #endregion

        #region Commands

        insightGroup.MapPost("/", async (CidaDbContext db, Insight InsightAddOrUpdateModel) =>
            {
                var insight = new Insight
                {
                    IdUsuario = InsightAddOrUpdateModel.IdUsuario,
                    IdResumo = InsightAddOrUpdateModel.IdResumo,
                    Descricao = InsightAddOrUpdateModel.Descricao,
                    DataGeracao = DateTime.Now
                };

                db.Insights.Add(insight);
                await db.SaveChangesAsync();
                return Results.Created($"/insight/{insight.IdInsight}", insight);
            })
            .Produces(StatusCodes.Status400BadRequest)
            .Produces<Insight>(StatusCodes.Status201Created)
            .WithName("CreateInsight")
            .WithMetadata(new SwaggerRequestExampleAttribute(typeof(InsightAddOrUpdateMetadata),
                typeof(InsightAddOrUpdateMetadata)))
            .WithTags("Insight")
            .WithOpenApi();

        insightGroup.MapPut("/{id:int}", async (CidaDbContext db, int id, Insight InsightAddOrUpdateModel) =>
            {
                var insight = await db.Insights.FindAsync(id);
                if (insight == null)
                {
                    return Results.NotFound();
                }

                insight.IdUsuario = InsightAddOrUpdateModel.IdUsuario;
                insight.IdResumo = InsightAddOrUpdateModel.IdResumo;
                insight.Descricao = InsightAddOrUpdateModel.Descricao;
                insight.DataGeracao = DateTime.Now;

                await db.SaveChangesAsync();
                return Results.Ok(insight);
            })
            .Produces(StatusCodes.Status400BadRequest)
            .Produces<Insight>()
            .WithName("PutInsight")
            .WithTags("Insight")
            .WithMetadata(new SwaggerRequestExampleAttribute(typeof(InsightAddOrUpdateMetadata),
                typeof(InsightAddOrUpdateMetadata)))
            .WithOpenApi(
                generatedOperation =>
                {
                    var parameter = generatedOperation.Parameters[0];
                    parameter.Description = "Id do insight a ser alterado";
                    parameter.Required = true;
                    return generatedOperation;
                }
            );

        insightGroup.MapDelete("/{id:int}", async (CidaDbContext db, int id) =>
            {
                var insight = await db.Insights.FindAsync(id);
                if (insight == null)
                {
                    return Results.NotFound();
                }

                db.Insights.Remove(insight);
                await db.SaveChangesAsync();
                return Results.NoContent();
            })
            .Produces(StatusCodes.Status404NotFound)
            .Produces<Insight>(StatusCodes.Status204NoContent)
            .WithName("DeleteInsight")
            .WithTags("Insight")
            .WithOpenApi(
                generatedOperation =>
                {
                    var parameter = generatedOperation.Parameters[0];
                    parameter.Description = "Id do insight a ser deletado";
                    parameter.Required = true;
                    return generatedOperation;
                }
            );

        #endregion
    }
}