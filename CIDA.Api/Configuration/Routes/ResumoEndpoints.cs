using System.ComponentModel.DataAnnotations;
using CIDA.Api.Models;
using CIDA.Api.Models.Metadatas;
using Cida.Data;
using CIDA.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Filters;

namespace CIDA.Api.Configuration.Routes;

public static class ResumoEndpoints
{
    public static void MapResumoEndpoints(this WebApplication app)
    {
        var resumoGroup = app.MapGroup("/resumo");

        #region Queries

        resumoGroup.MapGet("/search", async (CidaDbContext db, int page = 1, int pagesize = 30) =>
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
            .WithTags("Resumo")
            .WithOpenApi();

        resumoGroup.MapGet("/{id:int}", async (CidaDbContext db, int id) =>
            {
                var resumo = await db.Resumos.FindAsync(id);
                if (resumo == null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(resumo);
            })
            .Produces<Resumo>()
            .Produces(StatusCodes.Status404NotFound)
            .WithName("GetResumoById")
            .WithTags("Resumo")
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
                var autenticacaoDb = await db.Autenticacoes.FirstOrDefaultAsync(a => a.Email == email);
                if (autenticacaoDb == null)
                {
                    return Results.NotFound("Email de usuário não encontrado");
                }

                var idUsuario = await db.Usuarios.Where(u => u.IdAutenticacao == autenticacaoDb.IdAutenticacao)
                    .Select(u => u.IdUsuario).FirstOrDefaultAsync();


                var results = await db.Resumos.Where(r => r.IdUsuario == idUsuario).ToListAsync();
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

        resumoGroup.MapPost("/", async (CidaDbContext db, ResumoAddOrUpdateModel model) =>
            {
                var resumo = new Resumo
                {
                    IdUsuario = model.IdUsuario,
                    DataGeracao = DateTime.Now,
                    Descricao = model.Descricao,
                };
                db.Resumos.Add(resumo);
                await db.SaveChangesAsync();
                return Results.Created($"/resumo/{resumo.IdResumo}", resumo);
            })
            .Accepts<ResumoAddOrUpdateModel>("application/json")
            .Produces<Resumo>(StatusCodes.Status201Created)
            .WithName("PostResumo")
            .WithTags("Resumo")
            .WithMetadata(new SwaggerRequestExampleAttribute(typeof(ResumoAddOrUpdateMetadata),
                typeof(ResumoAddOrUpdateMetadata)))
            .WithOpenApi();

        resumoGroup.MapPut("/{id:int}", async (CidaDbContext db, int id, ResumoAddOrUpdateModel model) =>
            {
                var usuario = await db.Usuarios.FindAsync(model.IdUsuario);
                if (usuario == null)
                {
                    return Results.BadRequest("Usuario não encontrado");
                }


                var resumo = await db.Resumos.FindAsync(id);
                if (resumo == null)
                {
                    return Results.NotFound();
                }

                resumo.IdUsuario = model.IdUsuario;
                resumo.Descricao = model.Descricao;
                await db.SaveChangesAsync();
                return Results.Ok(resumo);
            })
            .Produces<Resumo>()
            .Produces(StatusCodes.Status404NotFound)
            .WithName("PutResumo")
            .WithTags("Resumo")
            .WithMetadata(new SwaggerRequestExampleAttribute(typeof(ResumoAddOrUpdateMetadata),
                typeof(ResumoAddOrUpdateMetadata)))
            .WithOpenApi();

        resumoGroup.MapDelete("/{id:int}", async (CidaDbContext db, int id) =>
            {
                var resumo = await db.Resumos.FindAsync(id);
                if (resumo == null)
                {
                    return Results.NotFound();
                }

                db.Resumos.Remove(resumo);
                await db.SaveChangesAsync();
                return Results.NoContent();
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("DeleteResumo")
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