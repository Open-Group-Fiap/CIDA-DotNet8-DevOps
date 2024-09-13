using CIDA.Api.Models;
using CIDA.Domain.Entities;

namespace CIDA.Api.Services;

public static class ResumoService
{
    public static Resumo MapToInsight(this ResumoAddOrUpdateModel model)
    {
        return new Resumo()
        {
            IdUsuario = model.IdUsuario, Descricao = model.Descricao, DataGeracao = DateTime.Now
        };
    }

    public static Resumo MapToInsightWithoutDate(this ResumoAddOrUpdateModel model)
    {
        return new Resumo()
        {
            IdUsuario = model.IdUsuario, Descricao = model.Descricao
        };
    }
}