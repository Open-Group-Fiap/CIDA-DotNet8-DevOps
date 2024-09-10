using Swashbuckle.AspNetCore.Filters;

namespace CIDA.Api.Models.Metadatas;

public class ResumoAddOrUpdateMetadata() : IExamplesProvider<ResumoAddOrUpdateModel>
{
    public ResumoAddOrUpdateModel GetExamples()
    {
        return new ResumoAddOrUpdateModel
        (
            0,
            DateTime.Now,
            "Descrição do resumo"
        );
    }
}