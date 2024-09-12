using Swashbuckle.AspNetCore.Filters;

namespace CIDA.Api.Models.Metadatas;

public class InsightAddOrUpdateMetadata : IExamplesProvider<InsightAddOrUpdateModel>
{
    public InsightAddOrUpdateModel GetExamples()
    {
        return new InsightAddOrUpdateModel
        (
            1,
            1,
            "Descrição do resumo"
        );
    }
}