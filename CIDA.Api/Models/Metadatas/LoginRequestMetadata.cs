using Swashbuckle.AspNetCore.Filters;

namespace CIDA.Api.Models.Metadatas;

public class LoginRequestMetadata : IExamplesProvider<LoginRequest>
{
    public LoginRequest GetExamples()
    {
        return new LoginRequest
        (
            "example@example.com",
            "password123"
        );
    }
}