using System.ComponentModel.DataAnnotations;
using CIDA.Api.Models;
using CIDA.Api.Models.Metadatas;
using Cida.Data;
using CIDA.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Filters;

namespace CIDA.Api.Configuration.Routes;

public static class UsuarioEndpoints
{
    public static void MapUsuarioEndpoints(this WebApplication app)
    {
        var usuarioGroup = app.MapGroup("/usuario");

        #region Queries

        usuarioGroup.MapGet("/email/{email}", async ([Required] string email, CidaDbContext db) =>
            {
                var usuario = await db.Usuarios
                    .Where(u => u.Autenticacao.Email == email)
                    .Select(u => new
                    {
                        u.IdUsuario,
                        u.Nome,
                        u.TipoDocumento,
                        u.NumDocumento,
                        u.Telefone,
                        u.DataCriacao,
                        u.Status
                    })
                    .FirstOrDefaultAsync();

                if (usuario == null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(usuario);
            })
            .Produces<Usuario>()
            .Produces(StatusCodes.Status404NotFound)
            .WithName("GetUsuarioByEmail")
            .WithTags("Usuario")
            .WithOpenApi(
                generatedOperation =>
                {
                    var parameter = generatedOperation.Parameters[0];
                    parameter.Description = "Email do usuário a ser consultado";
                    parameter.Required = true;
                    return generatedOperation;
                }
            );

        usuarioGroup.MapGet("/{id:int}", async ([Required] int id, CidaDbContext db) =>
            {
                var usuario = await db.Usuarios.FindAsync(id);
                if (usuario == null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(usuario);
            })
            .Produces<Usuario>()
            .Produces(StatusCodes.Status404NotFound)
            .WithName("GetUsuarioById")
            .WithTags("Usuario")
            .WithOpenApi(
                generatedOperation =>
                {
                    var parameter = generatedOperation.Parameters[0];
                    parameter.Description = "Id do usuário a ser consultado";
                    parameter.Required = true;
                    return generatedOperation;
                }
            );

        #endregion

        #region commands

        usuarioGroup.MapPost("/",
                async ([FromBody, Required] UsuarioAndAutenticacaoAddOrUpdateModel model, CidaDbContext db) =>
                {
                    var autenticacao = new Autenticacao
                    {
                        Email = model.Email,
                        HashSenha = model.Senha
                    };

                    var existingAutenticacao =
                        await db.Autenticacoes.Where(a => a.Email == model.Email).FirstOrDefaultAsync();
                    if (existingAutenticacao != null)
                    {
                        return Results.BadRequest("Já existe um usuário com o email informado");
                    }

                    db.Autenticacoes.Add(autenticacao);
                    await db.SaveChangesAsync();


                    var usuario = new Usuario
                    {
                        IdAutenticacao = autenticacao.IdAutenticacao,
                        Nome = model.Nome,
                        TipoDocumento = model.TipoDocumento,
                        NumDocumento = model.NumDocumento,
                        Telefone = model.Telefone,
                        DataCriacao = model.DataCriacao,
                        Status = model.Status
                    };

                    db.Usuarios.Add(usuario);
                    await db.SaveChangesAsync();

                    var usuarioResponse = new
                    {
                        usuario.IdUsuario,
                        usuario.Nome,
                        usuario.TipoDocumento,
                        usuario.NumDocumento,
                        usuario.Telefone,
                        usuario.DataCriacao,
                        usuario.Status
                    };

                    return Results.Created($"/usuario/${usuarioResponse.IdUsuario}", usuarioResponse);
                })
            .Accepts<UsuarioAndAutenticacaoAddOrUpdateModel>("application/json")
            .Produces<Usuario>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithMetadata(new SwaggerRequestExampleAttribute(typeof(UsuarioAndAutenticacaoAddOrUpdateModel),
                typeof(UsuarioAndAutenticacaoAddOrUpdateMetadata)))
            .WithName("AddUsuario")
            .WithTags("Usuario")
            .WithDescription(
                "Adiciona um novo usuário, o campo TipoDocumento deve ser um dos seguintes valores: 0(CPF), 1(CNPJ) e o campo Status deve ser um dos seguintes valores: 0(Ativo), 1(Inativo)")
            .WithOpenApi();

        usuarioGroup.MapPut("/{id:int}",
                async (int id, [FromBody, Required] UsuarioAndAutenticacaoAddOrUpdateModel model, CidaDbContext db) =>
                {
                    var usuario = await db.Usuarios.FindAsync(id);
                    if (usuario == null)
                    {
                        return Results.NotFound();
                    }

                    var autenticacao = await db.Autenticacoes.FindAsync(usuario.IdAutenticacao);
                    if (autenticacao == null)
                    {
                        return Results.NotFound();
                    }

                    autenticacao.Email = model.Email;
                    autenticacao.HashSenha = model.Senha;
                    db.Autenticacoes.Update(autenticacao);
                    await db.SaveChangesAsync();

                    usuario.Nome = model.Nome;
                    usuario.TipoDocumento = model.TipoDocumento;
                    usuario.NumDocumento = model.NumDocumento;
                    usuario.Telefone = model.Telefone;
                    usuario.DataCriacao = model.DataCriacao;
                    usuario.Status = model.Status;
                    db.Usuarios.Update(usuario);
                    await db.SaveChangesAsync();

                    var usuarioResponse = new
                    {
                        usuario.IdUsuario,
                        usuario.Nome,
                        usuario.TipoDocumento,
                        usuario.NumDocumento,
                        usuario.Telefone,
                        usuario.DataCriacao,
                        usuario.Status
                    };

                    return Results.Ok(usuarioResponse);
                })
            .Produces<Usuario>()
            .Produces(StatusCodes.Status404NotFound)
            .Accepts<UsuarioAndAutenticacaoAddOrUpdateModel>("application/json")
            .WithMetadata(new SwaggerRequestExampleAttribute(typeof(UsuarioAndAutenticacaoAddOrUpdateModel),
                typeof(UsuarioAndAutenticacaoAddOrUpdateMetadata)))
            .WithName("UpdateUsuario")
            .WithTags("Usuario")
            .WithDescription(
                "Atualiza um usuário, o campo TipoDocumento deve ser um dos seguintes valores: 0(CPF), 1(CNPJ) e o campo Status deve ser um dos seguintes valores: 0(Ativo), 1(Inativo)")
            .WithOpenApi();

        usuarioGroup.MapDelete("/{id:int}", async ([Required] int id, CidaDbContext db) =>
            {
                var usuario = await db.Usuarios.FindAsync(id);
                if (usuario == null)
                {
                    return Results.NotFound();
                }

                var autenticacao = await db.Autenticacoes.FindAsync(usuario.IdAutenticacao);
                if (autenticacao == null)
                {
                    return Results.NotFound();
                }

                db.Autenticacoes.Remove(autenticacao);
                db.Usuarios.Remove(usuario);
                await db.SaveChangesAsync();

                return Results.NoContent();
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("DeleteUsuario")
            .WithTags("Usuario")
            .WithOpenApi(
                generatedOperation =>
                {
                    var parameter = generatedOperation.Parameters[0];
                    parameter.Description = "Id do usuário a ser deletado";
                    parameter.Required = true;
                    return generatedOperation;
                }
            );

        #endregion
    }
}