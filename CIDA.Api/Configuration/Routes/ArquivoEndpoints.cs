using System.ComponentModel.DataAnnotations;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using CIDA.Api.Models;
using CIDA.Api.Models.Metadatas;
using Cida.Data;
using CIDA.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Filters;

namespace CIDA.Api.Configuration.Routes;

public static class ArquivoEndpoints
{
    public static void MapArquivoEndpoints(this WebApplication app)
    {
        const string urlApi = "http://127.0.0.1:8000";

        var arquivoGroup = app.MapGroup("/arquivo");

        #region Queries

        arquivoGroup.MapGet("/search", (CidaDbContext db, int page = 1, int pagesize = 30) =>
            {
                var skip = (page - 1) * pagesize;
                var results = db.Arquivos.Skip(skip).Take(pagesize).ToList();
                return new ArquivosListModel(
                    page,
                    pagesize,
                    results.Count,
                    results);
            })
            .Produces<ArquivosListModel>()
            .WithName("GetArquivos")
            .WithTags("Arquivo")
            .WithOpenApi();

        arquivoGroup.MapGet("/{id:int}", async (CidaDbContext db, int id) =>
            {
                var arquivo = await db.Arquivos.FindAsync(id);
                return arquivo == null ? Results.NotFound("Arquivo não encontrado") : Results.Ok(arquivo);
            })
            .Produces<Arquivo>()
            .Produces(StatusCodes.Status404NotFound)
            .WithName("GetArquivoById")
            .WithTags("Arquivo")
            .WithOpenApi(
                generatedOperation =>
                {
                    var parameter = generatedOperation.Parameters[0];
                    parameter.Description = "Id do arquivo a ser consultado";
                    parameter.Required = true;
                    return generatedOperation;
                }
            );

        arquivoGroup.MapGet("email/{email}/search",
                async (CidaDbContext db, string email, int page = 1, int pagesize = 30) =>
                {
                    var usuario = await db.Usuarios.FirstOrDefaultAsync(x => x.Autenticacao.Email == email);
                    if (usuario == null)
                    {
                        return Results.NotFound("Email do usuário não existe");
                    }

                    var skip = (page - 1) * pagesize;

                    var arquivos = await db.Arquivos.Where(x => x.IdUsuario == usuario.IdUsuario).Skip(skip)
                        .Take(pagesize)
                        .ToListAsync();


                    return arquivos.Count == 0
                        ? Results.NotFound("Nenhum arquivo encontrado")
                        : Results.Ok(new ArquivosListModel(page, pagesize, arquivos.Count, arquivos));
                })
            .Produces<ArquivosListModel>()
            .Produces(StatusCodes.Status404NotFound)
            .WithName("GetArquivoByEmail")
            .WithTags("Arquivo")
            .WithOpenApi(
                generatedOperation =>
                {
                    var parameter = generatedOperation.Parameters[0];
                    parameter.Description = "Email do usuario que quer consultar os arquivos";
                    parameter.Required = true;
                    return generatedOperation;
                });

        arquivoGroup.MapGet("idUsuario/{id:int}/search",
                async (CidaDbContext db, int id, int page = 1, int pagesize = 30)
                    =>
                {
                    var usuario = await db.Usuarios.FindAsync(id);
                    if (usuario == null)
                    {
                        return Results.NotFound("Id do usuário não existe");
                    }

                    var skip = (page - 1) * pagesize;

                    var arquivos = await db.Arquivos.Where(x => x.IdUsuario == id).Skip(skip).Take(pagesize)
                        .ToListAsync();

                    return arquivos.Count == 0
                        ? Results.NotFound("Nenhum arquivo encontrado")
                        : Results.Ok(new ArquivosListModel(page, pagesize, arquivos.Count, arquivos));
                })
            .Produces<ArquivosListModel>()
            .Produces(StatusCodes.Status404NotFound)
            .WithName("GetArquivoByIdUsuario")
            .WithTags("Arquivo")
            .WithOpenApi(
                generatedOperation =>
                {
                    var parameter = generatedOperation.Parameters[0];
                    parameter.Description = "Id do usuario que quer consultar os arquivos";
                    parameter.Required = true;
                    return generatedOperation;
                });

        #endregion

        #region Commands

        arquivoGroup.MapPost("/idUsuario/{idUsuario:int}/upload",
                async ([Required] IFormFileCollection arquivosRequest, int idUsuario, CidaDbContext db,
                    IConfiguration configuration, BlobServiceClient blobServiceClient, HttpClient httpClient) =>
                {
                    //possibles types
                    var possiblesTypes = new List<string>
                    {
                        "application/msword",
                        "vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "vnd.openxmlformats-officedocument.wordprocessingml.document",
                        "application/pdf",
                        "text/csv"
                    };

                    if (arquivosRequest.Count == 0)
                    {
                        return Results.BadRequest("Nenhum arquivo foi enviado");
                    }

                    foreach (var file in arquivosRequest)
                    {
                        if (!possiblesTypes.Contains(file.ContentType) || file.Length > 2097152)
                        {
                            return Results.BadRequest("Tipo de arquivo não permitido");
                        }
                    }


                    var usuario = await db.Usuarios.FindAsync(idUsuario);
                    if (usuario == null)
                    {
                        return Results.BadRequest("Usuario não existe");
                    }

                    //Create a unique name for the container
                    var containerName = "cida-container-" + Guid.NewGuid().ToString("N").ToLower();

                    // Create the container and return a container client object
                    BlobContainerClient containerClient =
                        await blobServiceClient.CreateBlobContainerAsync(containerName, PublicAccessType.BlobContainer);


                    var arquivos = new List<Arquivo>();

                    var arquivosNomes = new List<string>();

                    foreach (var file in arquivosRequest)
                    {
                        //Create a unique name for the file
                        var filename = Guid.NewGuid().ToString("N").ToLower() + "-" + file.FileName;

                        // Get a reference to a blob
                        var blobClient = containerClient.GetBlobClient(filename);

                        // Upload data from a stream
                        await blobClient.UploadAsync(file.OpenReadStream(), true);


                        var arquivo = new Arquivo
                        {
                            IdUsuario = idUsuario,
                            Nome = filename,
                            Url = blobClient.Uri.ToString(),
                            DataUpload = DateTime.Now,
                            Extensao = file.ContentType,
                            Tamanho = (int)file.Length,
                        };

                        arquivos.Add(arquivo);
                        arquivosNomes.Add(filename);
                    }

                    await db.Arquivos.AddRangeAsync(arquivos);
                    await db.SaveChangesAsync();

                    var idsArquivos = arquivos.Select(x => x.IdArquivo).ToList();

                    var arquivosResponse = new ArquivosListModel(1, arquivos.Count, arquivos.Count, arquivos);

                    //Send request to analyse (python api)
                    /*
                    var response = await httpClient.PostAsJsonAsync($"{urlApi}/analyze", new
                    {
                        containerClient.Uri,
                        arquivosNomes,
                        idUsuario,
                        idsArquivos
                    });


                    return !response.IsSuccessStatusCode
                        ? Results.BadRequest("Erro ao enviar arquivos para análise")
                        : Results.Created($"/arquivo/idUsuario/{idUsuario}/search", arquivosResponse);
                        */
                    return Results.Created($"/arquivo/idUsuario/{idUsuario}/search", arquivosResponse);
                })
            .DisableAntiforgery()
            .Accepts<IFormFileCollection>("multipart/form-data")
            .Produces(StatusCodes.Status400BadRequest)
            .Produces<ArquivosListModel>(StatusCodes.Status201Created)
            .WithName("PostArquivo")
            .WithTags("Arquivo")
            .WithOpenApi();

        arquivoGroup.MapPut("/{id:int}", async (int id, CidaDbContext db, ArquivoUpdateModel model) =>
            {
                var resumo = await db.Resumos.FindAsync(model.IdResumo);
                if (resumo == null)
                {
                    return Results.BadRequest("Não existe resumo com o id informado");
                }

                var arquivo = await db.Arquivos.FindAsync(id);
                if (arquivo == null)
                {
                    return Results.NotFound("Arquivo não encontrado");
                }

                arquivo.IdResumo = model.IdResumo;

                await db.SaveChangesAsync();
                return Results.Ok(arquivo);
            })
            .Produces(StatusCodes.Status400BadRequest)
            .Produces<Arquivo>()
            .WithName("PutArquivo")
            .WithTags("Arquivo")
            .WithMetadata(new SwaggerRequestExampleAttribute(typeof(ArquivoUpdateModel),
                typeof(ArquivoUpdateMetadata)))
            .WithOpenApi(
                generatedOperation =>
                {
                    var parameter = generatedOperation.Parameters[0];
                    parameter.Description = "Id do arquivo a ser alterado";
                    parameter.Required = true;
                    return generatedOperation;
                }
            );

        arquivoGroup.MapDelete("/{id:int}",
                async (int id, CidaDbContext db, IConfiguration configuration, BlobServiceClient blobServiceClient) =>
                {
                    var arquivo = await db.Arquivos.FindAsync(id);
                    if (arquivo == null)
                    {
                        return Results.NotFound("Arquivo não encontrado");
                    }

                    // delete container and all blobs
                    var containerName = arquivo.Url.Split('/')[3];
                    var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                    await containerClient.DeleteIfExistsAsync();

                    db.Arquivos.Remove(arquivo);
                    await db.SaveChangesAsync();

                    return Results.NoContent();
                })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("DeleteArquivo")
            .WithTags("Arquivo")
            .WithOpenApi(
                generatedOperation =>
                {
                    var parameter = generatedOperation.Parameters[0];
                    parameter.Description = "Id do arquivo a ser deletado";
                    parameter.Required = true;
                    return generatedOperation;
                });

        #endregion
    }
}