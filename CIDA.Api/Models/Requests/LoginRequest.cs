namespace CIDA.Api.Models;

public record LoginRequest(
    string Email,
    string Senha
);