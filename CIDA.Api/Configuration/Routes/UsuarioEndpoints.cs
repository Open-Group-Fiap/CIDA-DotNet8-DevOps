using System.ComponentModel.DataAnnotations;
using CIDA.Api.Models;
using CIDA.Api.Models.Metadatas;
using Cida.Data;
using CIDA.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Filters;
using CIDA.Api.Services;

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
                    .FirstOrDefaultAsync();

                return usuario == null ? Results.NotFound("Nenhum usuario encontrado") : Results.Ok(usuario);
            })
            .Produces<Usuario>()
            .Produces(StatusCodes.Status404NotFound)
            .WithName("GetUsuarioByEmail")
            .WithDescription("Retorna um usuário por email")
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
                return usuario == null ? Results.NotFound("Nenhum usuario encontrado") : Results.Ok(usuario);
            })
            .Produces<Usuario>()
            .Produces(StatusCodes.Status404NotFound)
            .WithName("GetUsuarioById")
            .WithTags("Usuario")
            .WithDescription("Retorna um usuário por id")
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
                    if (!model.TipoDocumento.Equals(TipoDocumento.CPF) &&
                        !model.TipoDocumento.Equals(TipoDocumento.CNPJ))
                    {
                        return Results.BadRequest("TipoDocumento deve ser um dos seguintes valores: 0(CPF), 1(CNPJ)");
                    }

                    var numDocumentoExists = await db.Usuarios
                        .Where(u => u.NumDocumento == model.NumDocumento)
                        .FirstOrDefaultAsync();
                    if (numDocumentoExists != null)
                    {
                        return Results.BadRequest("Número de documento já cadastrado");
                    }

                    var autenticacao = model.MapToAutenticacao();

                    var existingAutenticacao =
                        await db.Autenticacoes.Where(a => a.Email == model.Email).FirstOrDefaultAsync();
                    if (existingAutenticacao != null)
                    {
                        return Results.BadRequest("Email já cadastrado");
                    }

                    db.Autenticacoes.Add(autenticacao);
                    await db.SaveChangesAsync();


                    var usuario = model.MapToUsuario(autenticacao);

                    db.Usuarios.Add(usuario);
                    await db.SaveChangesAsync();

                    return Results.Created($"/usuario/${usuario.IdUsuario}", usuario);
                })
            .Accepts<UsuarioAndAutenticacaoAddOrUpdateModel>("application/json")
            .Produces<Usuario>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithMetadata(new SwaggerRequestExampleAttribute(typeof(UsuarioAndAutenticacaoAddOrUpdateModel),
                typeof(UsuarioAndAutenticacaoAddOrUpdateMetadata)))
            .WithName("AddUsuario")
            .WithTags("Usuario")
            .WithDescription(
                "Adiciona um novo usuário, o campo TipoDocumento deve ser um dos seguintes valores: 0(CPF), 1(CNPJ)")
            .WithOpenApi();

        usuarioGroup.MapPut("/{id:int}",
                async (int id, [FromBody, Required] UsuarioAndAutenticacaoAddOrUpdateModel model, CidaDbContext db) =>
                {
                    if (!model.TipoDocumento.Equals(TipoDocumento.CPF) &&
                        !model.TipoDocumento.Equals(TipoDocumento.CNPJ))
                    {
                        return Results.BadRequest("TipoDocumento deve ser um dos seguintes valores: 0(CPF), 1(CNPJ)");
                    }

                    var usuario = await db.Usuarios.FindAsync(id);
                    if (usuario == null)
                    {
                        return Results.NotFound("Usuário não encontrado");
                    }

                    if (usuario.NumDocumento != model.NumDocumento)
                    {
                        var numDocumentoExists = await db.Usuarios
                            .Where(u => u.NumDocumento == model.NumDocumento)
                            .FirstOrDefaultAsync();
                        if (numDocumentoExists != null)
                        {
                            return Results.BadRequest("Número de documento já cadastrado");
                        }
                    }

                    var autenticacao = await db.Autenticacoes.FindAsync(usuario.IdAutenticacao);
                    var existingAutenticacao =
                        await db.Autenticacoes.Where(a => a.Email == model.Email).FirstOrDefaultAsync();
                    if (existingAutenticacao != null && existingAutenticacao.IdAutenticacao != autenticacao.IdAutenticacao) return Results.BadRequest("Email já cadastrado");

                    autenticacao.Email = model.Email;
                    autenticacao.HashSenha = AutenticacaoService.QuickHash(model.Senha);

                    db.Autenticacoes.Update(autenticacao);
                    await db.SaveChangesAsync();

                    usuario.Nome = model.Nome;
                    usuario.NumDocumento = model.NumDocumento;
                    usuario.Telefone = model.Telefone;
                    usuario.TipoDocumento = model.TipoDocumento;

                    db.Usuarios.Update(usuario);
                    await db.SaveChangesAsync();

                    return Results.Ok(usuario);
                })
            .Accepts<UsuarioAndAutenticacaoAddOrUpdateModel>("application/json")
            .Produces<Usuario>()
            .Produces(StatusCodes.Status404NotFound)
            .WithMetadata(new SwaggerRequestExampleAttribute(typeof(UsuarioAndAutenticacaoAddOrUpdateModel),
                typeof(UsuarioAndAutenticacaoAddOrUpdateMetadata)))
            .WithName("UpdateUsuario")
            .WithTags("Usuario")
            .WithDescription(
                "Atualiza um usuário, o campo TipoDocumento deve ser um dos seguintes valores: 0(CPF), 1(CNPJ)")
            .WithOpenApi();

        usuarioGroup.MapDelete("/{id:int}", async ([Required] int id, CidaDbContext db) =>
            {
                var usuario = await db.Usuarios.FindAsync(id);
                if (usuario == null)
                {
                    return Results.NotFound("Usuário não encontrado");
                }

                var autenticacao = await db.Autenticacoes.FindAsync(usuario.IdAutenticacao);
                if (autenticacao == null)
                {
                    return Results.NotFound("Autenticação não encontrada");
                }

                db.Autenticacoes.Remove(autenticacao);
                db.Usuarios.Remove(usuario);
                await db.SaveChangesAsync();

                return Results.NoContent();
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("DeleteUsuario")
            .WithDescription("Deleta um usuário")
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