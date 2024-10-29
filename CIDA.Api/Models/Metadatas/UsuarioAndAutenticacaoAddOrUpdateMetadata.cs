using Swashbuckle.AspNetCore.Filters;

namespace CIDA.Api.Models.Metadatas;

public class UsuarioAndAutenticacaoAddOrUpdateMetadata : IExamplesProvider<UsuarioAndAutenticacaoAddOrUpdateModel>
{
    public UsuarioAndAutenticacaoAddOrUpdateModel GetExamples()
    {
        return new UsuarioAndAutenticacaoAddOrUpdateModel
        (
            "example@example.com",
            "password123",
            "Fulano de Tal",
            0,
            "12345678900",
            "123456789"
        );
    }
}