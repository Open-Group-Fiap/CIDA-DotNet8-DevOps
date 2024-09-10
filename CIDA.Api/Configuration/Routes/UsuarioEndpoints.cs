using CIDA.Api.Models;
using Cida.Data;
using CIDA.Domain.Entities;

namespace CIDA.Api.Configuration.Routes;

public static class UsuarioEndpoints
{
    
    public static void MapUsuarioEndpoints(this WebApplication app)
    {
        
        var usuarioGroup = app.MapGroup("/usuario");

        
        usuarioGroup.MapGet("/{id:int}", async (int id, CidaDbContext db) =>
        {
            var usuario = await db.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return Results.NotFound();
            }

            return Results.Ok(usuario);
        });
        
        usuarioGroup.MapPost("/", async (UsuarioAndAutenticacaoAddModel model, CidaDbContext db) =>
            {
                var autenticacao = new Autenticacao
                {
                    Email = model.Email,
                    HashSenha = model.Senha
                };
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
            .Accepts<UsuarioAndAutenticacaoAddModel>("application/json")
            .WithName("AddUsuario")
            .WithOpenApi();
        
    }
}