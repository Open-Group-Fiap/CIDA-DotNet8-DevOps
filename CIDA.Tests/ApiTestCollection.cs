using CIDA.Api;
using Microsoft.AspNetCore.Mvc.Testing;

namespace CIDA.Tests;

[CollectionDefinition("Api Test Collection")]
public class ApiTestCollection : ICollectionFixture<WebApplicationFactory<Program>>
{
}