using System.ComponentModel.DataAnnotations;

namespace CIDA.Api.Models;

public record ResumoAddOrUpdateModel(
    [Required] int IdUsuario,
    [Required] string Descricao
);