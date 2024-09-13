﻿using System.ComponentModel.DataAnnotations;
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

                return usuario == null ? Results.NotFound("Nenhum usuario encontrado") : Results.Ok(usuario);
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
                return usuario == null ? Results.NotFound("Nenhum usuario encontrado") : Results.Ok(usuario);
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
                "Adiciona um novo usuário, o campo TipoDocumento deve ser um dos seguintes valores: 0(CPF), 1(CNPJ) e o campo Status deve ser um dos seguintes valores: 0(Ativo), 1(Inativo)")
            .WithOpenApi();

        usuarioGroup.MapPut("/{id:int}",
                async (int id, [FromBody, Required] UsuarioAndAutenticacaoAddOrUpdateModel model, CidaDbContext db) =>
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

                    autenticacao = model.MapToAutenticacaoUpdate(autenticacao);

                    db.Autenticacoes.Update(autenticacao);
                    await db.SaveChangesAsync();

                    usuario = model.MapToUsuarioUpdate(usuario);

                    db.Usuarios.Update(usuario);
                    await db.SaveChangesAsync();

                    return Results.Ok(usuario);
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