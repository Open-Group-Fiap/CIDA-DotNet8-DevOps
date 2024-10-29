using System.Net.Http.Headers;
using System.Text;
using CIDA.Api.Models;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using Azure;
using Azure.Identity;
using Azure.AI.Inference;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Reflection;
using iTextSharp.text.pdf.parser;
using Path = System.IO.Path;

namespace CIDA.Api.Services;

public static class InsightResumoService
{
    private static string readPdf(IFormFile file)
    {
        var reader = new PdfReader(file.OpenReadStream());
        var text = new StringBuilder();
        for (var i = 1; i <= reader.NumberOfPages; i++) text.Append(PdfTextExtractor.GetTextFromPage(reader, i));

        return text.ToString();
    }

    private static string readDocx(IFormFile file)
    {
        var text = "";
        using (var stream = file.OpenReadStream())
        {
            using (var doc = WordprocessingDocument.Open(stream, false))
            {
                var body = doc.MainDocumentPart.Document.Body;
                text = body.InnerText;
            }
        }

        return text;
    }

    private static string readXlsx(IFormFile file)
    {
        var text = "";
        using (var stream = file.OpenReadStream())
        {
            using (var doc = SpreadsheetDocument.Open(stream, false))
            {
                var workbookPart = doc.WorkbookPart;
                var sheets = workbookPart.Workbook.Descendants<Sheet>();
                var sheet = (WorksheetPart)workbookPart.GetPartById(sheets.First().Id);
                var sheetData = sheet.Worksheet.Elements<SheetData>().First();
                text = sheetData.InnerText;
            }
        }

        return text;
    }

    private static string readTxt(IFormFile file)
    {
        var text = "";
        using (var stream = file.OpenReadStream())
        {
            using (var reader = new StreamReader(stream))
            {
                text = reader.ReadToEnd();
            }
        }

        return text;
    }

    private static string readCsv(IFormFile file)
    {
        var text = "";
        using (var stream = file.OpenReadStream())
        {
            using (var reader = new StreamReader(stream))
            {
                text = reader.ReadToEnd();
            }
        }

        return text;
    }

    private static string ReadFile(IFormFile file)
    {
        var extension = Path.GetExtension(file.FileName);
        switch (extension)
        {
            case ".pdf":
                return readPdf(file);
            case ".docx":
                return readDocx(file);
            case ".xlsx":
                return readXlsx(file);
            case ".txt":
                return readTxt(file);
            case ".csv":
                return readCsv(file);
            default:
                throw new Exception("Formato de arquivo não suportado");
        }
    }

    private static async Task<string> SendRequestGemini(string geminiPrompt, IConfiguration configuration)
    {
        var geminiApiKey = configuration.GetConnectionString("GeminiApiKey");

        using var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Post,
            $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash-latest:generateContent?key={geminiApiKey}");

        var jsonBody = $@"{{
                ""contents"": [
                    {{
                        ""parts"": [
                            {{
                                ""text"": ""{geminiPrompt}""
                            }}
                        ]
                    }}
                ]
            }}";

        request.Content = new StringContent(jsonBody, Encoding.UTF8);
        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        var response = await client.SendAsync(request).ConfigureAwait(false);

        if (response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            return responseBody.Substring(responseBody.IndexOf("\"text\": \"") + 9,
                responseBody.IndexOf("\"", responseBody.IndexOf("\"text\": \"") + 10) -
                responseBody.IndexOf("\"text\": \"") - 9);
        }
        else
        {
            return $"Error: {response.StatusCode} - {response.ReasonPhrase}";
        }
    }


    public static async Task<InsightResumo> GenerateInsightResumo(IFormFileCollection arquivosRequest, IConfiguration configuration)
    {
        var text = "";
        foreach (var arquivo in arquivosRequest) text += ReadFile(arquivo);

        var start_phase =
            "Resuma o seguinte documento e diminua seu tamanho total, mantenha a coesão, os dados e as estatisticas: ";

        var azureAIEndpoint = configuration.GetConnectionString("AzureAIEndpoint");
        var azureAIApiKey = configuration.GetConnectionString("AzureAIApiKey");
        
        var client = new ChatCompletionsClient(
            new Uri(azureAIEndpoint),
            new AzureKeyCredential(azureAIApiKey)
        );

        var requestOptions = new ChatCompletionsOptions()
        {
            Messages =
            {
                new ChatRequestSystemMessage(
                    "Resuma o seguinte documento e diminua seu tamanho total, mantenha a coesão, os dados e as estatisticas: "),
                new ChatRequestUserMessage(text)
            }
        };

        Response<ChatCompletions> response = client.Complete(requestOptions);


        var resumo = response.Value.Choices[0].Message.Content;


        var geminiPrompt =
            "Você é a CIDA, Consulting Insights With Deep Analysis, você tem como função analisar relatorios empresariais e ajudar as empresas gerando insights, analises e recomendações com base nos dados apresentados. Com base nesse relatorio empresarial, analise os dados e a situação da empresa e gere insights destacando os dados mais relevantes, pontos fortes e pontos mais fracos:";
        geminiPrompt += "\n\n";
        geminiPrompt += resumo;

        var output = await SendRequestGemini(geminiPrompt, configuration);

        var insight = output.Replace("\\n", Environment.NewLine)
            .Replace("\n", "")
            .Replace("**", "");

        return new InsightResumo
        {
            Insight = insight,
            Resumo = resumo
        };
    }
}