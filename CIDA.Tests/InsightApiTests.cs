using System.Net;
using System.Net.Http.Json;
using CIDA.Api;
using CIDA.Api.Models;
using CIDA.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Testing;

namespace CIDA.Tests;

[Collection("Api Test Collection")]
public class InsightApiTests
{
    private readonly HttpClient _client;

    public InsightApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task PutInsight_ReturnsInsight_WhenInsightExists()
    {
        // Arrange
        var insight = new InsightAddOrUpdateModel(
            1,
            1,
            "Descrição do insight"
        );

        // Act
        var response = await _client.PutAsJsonAsync("/insight/1", insight);

        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var updatedInsight = await response.Content.ReadFromJsonAsync<Insight>();
        Assert.NotNull(updatedInsight);
        Assert.Equal(insight.Descricao, updatedInsight.Descricao);
    }

    [Fact]
    public async Task GetInsightById_ReturnsInsight_WhenInsightExists()
    {
        // Act
        var response = await _client.GetAsync("/insight/1");

        // Assert
        response.EnsureSuccessStatusCode();
        var insight = await response.Content.ReadFromJsonAsync<Insight>();

        Assert.NotNull(insight);
    }

    [Fact]
    public async Task GetInsightByEmail_ReturnsInsight_WhenInsightExists()
    {
        // Act
        var response = await _client.GetAsync("/insight/email/example@example.com");

        // Assert
        response.EnsureSuccessStatusCode();
        var insight = await response.Content.ReadFromJsonAsync<Insight>();

        Assert.NotNull(insight);
    }

    [Fact]
    public async Task DeleteInsight_ReturnsNoContent_WhenInsightExists()
    {
        // Act
        var response = await _client.DeleteAsync("/insight/2");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task GetInsightsByUsuarioEmail_ReturnsInsight_WhenInsightExists()
    {
        // Act
        var response = await _client.GetAsync("/insight/search");

        // Assert
        response.EnsureSuccessStatusCode();
        var insight = await response.Content.ReadFromJsonAsync<InsightsListModel>();

        Assert.NotNull(insight);
    }

    [Fact]
    public async Task GetInsightsSearch_ReturnsInsight_WhenInsightExists()
    {
        // Act
        var response = await _client.GetAsync("/insight/search");

        // Assert
        response.EnsureSuccessStatusCode();
        var insight = await response.Content.ReadFromJsonAsync<InsightsListModel>();

        Assert.NotNull(insight);
    }

    [Fact]
    public async Task PutInsight_ReturnsBadRequest_WhenUsuarioNotExists()
    {
        // Arrange
        var insight = new InsightAddOrUpdateModel(
            123,
            1,
            "Descrição do insight"
        );

        // Act
        var response = await _client.PutAsJsonAsync("/insight/1", insight);

        // check if returns a bad request and correct message
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Usuário não encontrado", content.Trim('"'));
    }

    [Fact]
    public async Task PutInsight_ReturnsBadRequest_WhenResumoNotExists()
    {
        // Arrange
        var insight = new InsightAddOrUpdateModel(
            1,
            123,
            "Descrição do insight"
        );

        // Act
        var response = await _client.PutAsJsonAsync("/insight/1", insight);

        // check if returns a bad request and correct message
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Resumo não encontrado", content.Trim('"'));
    }

    [Fact]
    public async Task PutInsight_ReturnsNotFound_WhenInsightNotExists()
    {
        // Arrange
        var insight = new InsightAddOrUpdateModel(
            1,
            1,
            "Descrição do insight"
        );

        // Act
        var response = await _client.PutAsJsonAsync("/insight/123", insight);

        // check if returns a bad request and correct message
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task PutInsight_ReturnsBadRequest_WhenResumoIsAlreadyAssigned()
    {
        // Arrange
        var insight = new InsightAddOrUpdateModel(
            1,
            3,
            "Descrição do insight"
        );

        // Act
        var response = await _client.PutAsJsonAsync("/insight/1", insight);

        // check if returns a bad request and correct message
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Já existe um insight para esse resumo", content.Trim('"'));
    }

    [Fact]
    public async Task PutInsight_ReturnsBadRequest_WhenSendRandomJson()
    {
        // Act
        var response = await _client.PutAsJsonAsync("/insight/1", new { });

        // check if returns a bad request
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}