using CIDA.Api.Models;
using CIDA.Api.Models.Metadatas;
using CIDA.Api.Services;
using Cida.Data;
using CIDA.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Filters;

namespace CIDA.Api.Configuration.Routes;

public static class ResumoEndpoints
{
    public static void MapResumoEndpoints(this WebApplication app)
    {
        var resumoGroup = app.MapGroup("/resumo");

        #region Queries

        resumoGroup.MapGet("/search", (CidaDbContext db, int page = 1, int pagesize = 30) =>
            {
                var skip = (page - 1) * pagesize;
                var results = db.Resumos.Skip(skip).Take(pagesize).ToList();
                return new ResumosListModel(
                    page,
                    pagesize,
                    results.Count,
                    results);
            })
            .Produces<ResumosListModel>()
            .WithName("GetResumos")
            .WithDescription("Retorna uma lista de resumos por paginação")
            .WithTags("Resumo")
            .WithOpenApi();

        resumoGroup.MapGet("/{id:int}", async (CidaDbContext db, int id) =>
            {
                var resumo = await db.Resumos.FindAsync(id);
                return resumo == null ? Results.NotFound("Resumo não encontrado") : Results.Ok(resumo);
            })
            .Produces<Resumo>()
            .Produces(StatusCodes.Status404NotFound)
            .WithName("GetResumoById")
            .WithTags("Resumo")
            .WithDescription("Retorna um resumo por id")
            .WithOpenApi(
                generatedOperation =>
                {
                    var parameter = generatedOperation.Parameters[0];
                    parameter.Description = "Id do resumo a ser consultado";
                    parameter.Required = true;
                    return generatedOperation;
                }
            );

        resumoGroup.MapGet("/email/{email}", async (CidaDbContext db, string email, int page = 1, int pagesize = 30) =>
            {
                var usuario = await db.Usuarios.Where(u => u.Autenticacao.Email == email).FirstOrDefaultAsync();

                if (usuario == null) return Results.NotFound("Email não encontrado");


                var results = await db.Resumos.Where(r => r.IdUsuario == usuario.IdUsuario).ToListAsync();
                return Results.Ok(new ResumosListModel(
                    page,
                    pagesize,
                    results.Count,
                    results));
            })
            .Produces<ResumosListModel>()
            .Produces(StatusCodes.Status404NotFound)
            .WithName("GetResumosByUsuarioEmail")
            .WithTags("Resumo")
            .WithDescription("Retorna uma lista de resumos por email de usuário por paginação")
            .WithOpenApi(
                generatedOperation =>
                {
                    var parameter = generatedOperation.Parameters[0];
                    parameter.Description = "Email do resumo a ser consultado";
                    parameter.Required = true;
                    return generatedOperation;
                }
            );

        #endregion

        #region Commands

        resumoGroup.MapPut("/{id:int}", async (CidaDbContext db, int id, ResumoAddOrUpdateModel model) =>
            {
                var usuario = await db.Usuarios.FindAsync(model.IdUsuario);
                if (usuario == null) return Results.BadRequest("Usuario não encontrado");


                var resumoDb = await db.Resumos.FindAsync(id);
                if (resumoDb == null) return Results.NotFound("Resumo não encontrado");

                var resumo = model.MapToInsightWithoutDate();

                await db.SaveChangesAsync();
                return Results.Ok(resumo);
            })
            .Accepts<ResumoAddOrUpdateModel>("application/json")
            .Produces<Resumo>()
            .Produces(StatusCodes.Status404NotFound)
            .WithName("PutResumo")
            .WithDescription("Atualiza um resumo")
            .WithTags("Resumo")
            .WithMetadata(new SwaggerRequestExampleAttribute(typeof(ResumoAddOrUpdateMetadata),
                typeof(ResumoAddOrUpdateMetadata)))
            .WithOpenApi();

        resumoGroup.MapDelete("/{id:int}", async (CidaDbContext db, int id) =>
            {
                var resumo = await db.Resumos.FindAsync(id);
                if (resumo == null) return Results.NotFound("Resumo não encontrado");

                db.Resumos.Remove(resumo);
                await db.SaveChangesAsync();
                return Results.NoContent();
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("DeleteResumo")
            .WithDescription("Deleta um resumo")
            .WithTags("Resumo")
            .WithOpenApi(
                generatedOperation =>
                {
                    var parameter = generatedOperation.Parameters[0];
                    parameter.Description = "Id do resumo a ser deletado";
                    parameter.Required = true;
                    return generatedOperation;
                }
            );

        #endregion
    }
}