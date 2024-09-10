using CIDA.Domain.Entities;

namespace CIDA.Api.Models;

[Serializable]
public record UsuarioAndAutenticacaoAddModel
(
    string Email,
    string Senha,
    string Nome,
    TipoDocumento TipoDocumento,
    string NumDocumento,
    string Telefone,
    DateTime DataCriacao,
    Status Status
);