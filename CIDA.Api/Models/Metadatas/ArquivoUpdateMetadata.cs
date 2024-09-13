using Swashbuckle.AspNetCore.Filters;

namespace CIDA.Api.Models.Metadatas;

public class ArquivoUpdateMetadata : IExamplesProvider<ArquivoUpdateModel>
{
    public ArquivoUpdateModel GetExamples()
    {
        return new ArquivoUpdateModel(1);
    }
}