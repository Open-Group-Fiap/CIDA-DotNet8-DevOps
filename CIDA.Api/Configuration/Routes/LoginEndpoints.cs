using System.Security.Cryptography;
using System.Text;
using CIDA.Api.Models;
using CIDA.Api.Models.Metadatas;
using Cida.Data;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Filters;

namespace CIDA.Api.Configuration.Routes;

public static class LoginEndpoints
{
    private static string QuickHash(string input)
    {
        var inputBytes = Encoding.UTF8.GetBytes(input);
        var inputHash = SHA256.HashData(inputBytes);
        return Convert.ToHexString(inputHash);
    }

    public static void MapLoginEndpoints(this WebApplication app)
    {
        var loginGroup = app.MapGroup("/login");

        loginGroup.MapPost("/", async (CidaDbContext db, LoginRequest request) =>
            {
                var autenticacao = await db.Autenticacoes.FirstOrDefaultAsync(x => x.Email == request.Email);
                if (autenticacao == null)
                {
                    return Results.Unauthorized();
                }

                return QuickHash(request.Senha) == autenticacao.HashSenha ? Results.Ok() : Results.Unauthorized();
            })
            .Accepts<LoginRequest>("application/json")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithName("Autenticacao")
            .WithTags("Login")
            .WithDescription("Autentica um usuário")
            .WithMetadata(new SwaggerRequestExampleAttribute(typeof(LoginRequestMetadata),
                typeof(LoginRequestMetadata)))
            .WithOpenApi();
    }
}