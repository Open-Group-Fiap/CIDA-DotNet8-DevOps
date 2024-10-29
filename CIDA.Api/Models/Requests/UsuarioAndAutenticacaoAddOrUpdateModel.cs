using System.ComponentModel.DataAnnotations;
using CIDA.Domain.Entities;
using Newtonsoft.Json;

namespace CIDA.Api.Models;

public record UsuarioAndAutenticacaoAddOrUpdateModel(
    [Required] string Email,
    [Required] string Senha,
    [Required] string Nome,
    [Required] TipoDocumento TipoDocumento,
    [Required] string NumDocumento,
    [Required] string Telefone
);