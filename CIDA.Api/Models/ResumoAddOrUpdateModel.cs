using System.ComponentModel.DataAnnotations;

namespace CIDA.Api.Models;

public record ResumoAddOrUpdateModel(
    [Required] int IdUsuario,
    [Required] DateTime DataGeracao,
    [Required] string Descricao
);