using CIDA.Api.Models;
using CIDA.Domain.Entities;

namespace CIDA.Api.Services;

public static class UsuarioService
{
    public static Usuario MapToUsuario(this UsuarioAndAutenticacaoAddOrUpdateModel model, Autenticacao autenticacao)
    {
        return new Usuario()
        {
            Nome = model.Nome, DataCriacao = DateTime.Now, NumDocumento = model.NumDocumento, Status = 0,
            Telefone = model.Telefone, TipoDocumento = model.TipoDocumento, IdAutenticacao = autenticacao.IdAutenticacao
        };
    }
}