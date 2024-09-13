using Swashbuckle.AspNetCore.Filters;

namespace CIDA.Api.Models.Metadatas;

public class ResumoAddOrUpdateMetadata() : IExamplesProvider<ResumoAddOrUpdateModel>
{
    public ResumoAddOrUpdateModel GetExamples()
    {
        return new ResumoAddOrUpdateModel
        (
            1,
            "Descrição do resumo"
        );
    }
}