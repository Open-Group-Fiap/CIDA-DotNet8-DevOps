using CIDA.Domain.Entities;

namespace CIDA.Api.Models;

public record ArquivosListModel(
    int Page,
    int PageSize,
    int Total,
    IEnumerable<Arquivo> Arquivos
);