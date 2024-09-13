using CIDA.Api.Models;
using CIDA.Domain.Entities;

namespace CIDA.Api.Services;

public static class InsightService
{
    public static Insight MapToInsight(this InsightAddOrUpdateModel model)
    {
        return new Insight()
        {
            IdUsuario = model.IdUsuario, IdResumo = model.IdResumo, Descricao = model.Descricao,
            DataGeracao = DateTime.Now
        };
    }

    public static Insight MapToInsightUpdate(this InsightAddOrUpdateModel model, Insight insight)
    {
        return new Insight()
        {
            IdUsuario = model.IdUsuario, IdResumo = model.IdResumo, Descricao = model.Descricao,
            IdInsight = insight.IdInsight, DataGeracao = insight.DataGeracao
        };
    }
}