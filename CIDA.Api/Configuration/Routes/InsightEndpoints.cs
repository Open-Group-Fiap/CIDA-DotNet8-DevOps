using CIDA.Api.Models;
using CIDA.Api.Models.Metadatas;
using CIDA.Api.Services;
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

        insightGroup.MapGet("/search", (CidaDbContext db, int page = 1, int pagesize = 30) =>
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
            .WithDescription("Retorna uma lista de insights por paginação")
            .WithOpenApi();

        insightGroup.MapGet("/{id:int}", async (CidaDbContext db, int id) =>
            {
                var insight = await db.Insights.FindAsync(id);
                return insight == null ? Results.NotFound("Insight não encontrado") : Results.Ok(insight);
            })
            .Produces<Insight>()
            .Produces(StatusCodes.Status404NotFound)
            .WithName("GetInsightById")
            .WithDescription("Retorna um insight por id")
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
                if (usuario == null) return Results.NotFound("Usuário não encontrado");

                var insight = await db.Insights.FirstOrDefaultAsync(x => x.IdUsuario == usuario.IdUsuario);

                return insight == null ? Results.NotFound("Nenhum insight encontrado") : Results.Ok(insight);
            })
            .Produces<Insight>()
            .Produces(StatusCodes.Status404NotFound)
            .WithName("GetInsightByEmail")
            .WithTags("Insight")
            .WithDescription("Retorna um insight por email")
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

        insightGroup.MapPut("/{id:int}", async (CidaDbContext db, int id, InsightAddOrUpdateModel model) =>
            {
                var insightDb = await db.Insights.FindAsync(id);
                if (insightDb == null) return Results.NotFound("Insight não encontrado");

                var usuario = await db.Usuarios.FindAsync(model.IdUsuario);
                if (usuario == null) return Results.BadRequest("Usuário não encontrado");

                var resumo = await db.Resumos.FindAsync(model.IdResumo);
                if (resumo == null) return Results.BadRequest("Resumo não encontrado");

                if (insightDb.IdResumo != model.IdResumo)
                {
                    var insightExists = await db.Insights.FirstOrDefaultAsync(x => x.IdResumo == model.IdResumo);
                    if (insightExists != null) return Results.BadRequest("Já existe um insight para esse resumo");
                }

                var insight = model.MapToInsightUpdate(insightDb);

                await db.SaveChangesAsync();
                return Results.Ok(insight);
            })
            .Accepts<InsightAddOrUpdateModel>("application/json")
            .Produces(StatusCodes.Status400BadRequest)
            .Produces<Insight>()
            .WithName("PutInsight")
            .WithTags("Insight")
            .WithDescription("Altera um insight")
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
                if (insight == null) return Results.NotFound("Insight não encontrado");

                db.Insights.Remove(insight);
                await db.SaveChangesAsync();
                return Results.NoContent();
            })
            .Produces(StatusCodes.Status404NotFound)
            .Produces<Insight>(StatusCodes.Status204NoContent)
            .WithName("DeleteInsight")
            .WithTags("Insight")
            .WithDescription("Deleta um insight")
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