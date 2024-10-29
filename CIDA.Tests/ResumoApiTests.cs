using System.Net;
using System.Net.Http.Json;
using CIDA.Api;
using CIDA.Api.Models;
using CIDA.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Testing;

namespace CIDA.Tests;

[Collection("Api Test Collection")]
public class ResumoApiTests
{
    private readonly HttpClient _client;

    public ResumoApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task PutResumo_ReturnsResumo_WhenResumoExists()
    {
        // Act
        var response = await _client.PutAsJsonAsync("/resumo/1", new ResumoAddOrUpdateModel(
            1,
            "Descrição do resumo"
        ));

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var updatedResume = await response.Content.ReadFromJsonAsync<Resumo>();
        Assert.NotNull(updatedResume);
        Assert.Equal("Descrição do resumo", updatedResume.Descricao);
    }

    [Fact]
    public async Task DeleteResumo_ReturnsNoContent_WhenResumoExists()
    {
        // Act
        var response = await _client.DeleteAsync("/resumo/2");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task GetResumosByUsuarioEmail_ReturnsResumo_WhenResumoExists()
    {
        // Act
        var response = await _client.GetAsync("/resumo/email/example@example.com");

        // Assert
        response.EnsureSuccessStatusCode();
        var resumo = await response.Content.ReadFromJsonAsync<ResumosListModel>();

        Assert.NotNull(resumo);
    }

    [Fact]
    public async Task GetResumosSearch_ReturnsResumo_WhenResumoExists()
    {
        // Act
        var response = await _client.GetAsync("/resumo/search");

        // Assert
        response.EnsureSuccessStatusCode();
        var resumo = await response.Content.ReadFromJsonAsync<ResumosListModel>();

        Assert.NotNull(resumo);
    }

    [Fact]
    public async Task GetResumoById_ReturnsResumo_WhenResumoExists()
    {
        // Act
        var response = await _client.GetAsync("/resumo/1");

        // Assert
        response.EnsureSuccessStatusCode();
        var resumo = await response.Content.ReadFromJsonAsync<Resumo>();

        Assert.NotNull(resumo);
    }

    [Fact]
    public async Task PutResumo_ReturnsNotFound_WhenResumoNotExists()
    {
        // Act
        var response = await _client.PutAsJsonAsync("/resumo/999", new ResumoAddOrUpdateModel(
            1,
            "Descrição do resumo"
        ));

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task PutResumo_ReturnsBadRequestResumo_WhenUsuarioNotExists()
    {
        // Act
        var response = await _client.PutAsJsonAsync("/resumo/1", new ResumoAddOrUpdateModel(
            999,
            "Descrição do resumo"
        ));

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task PutResumo_ReturnsBadRequest_WhenSendRandomJson()
    {
        // Act
        var response = await _client.PutAsJsonAsync("/resumo/1", new { });

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}