using CIDA.Domain.Entities;

namespace CIDA.Api.Models;

public record InsightsListModel(
    int Page,
    int PageSize,
    int Total,
    IEnumerable<Insight> Insights
);