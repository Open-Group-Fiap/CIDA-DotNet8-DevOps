using CIDA.Domain.Entities;

namespace CIDA.Api.Models;

public record ResumosListModel(
    int Page,
    int PageSize,
    int Total,
    IEnumerable<Resumo> Resumos
);