using CIDA.Api.Models;
using CIDA.Domain.Entities;

namespace CIDA.Api.Services;

public static class UsuarioService
{
    public static Usuario MapToUsuario(this UsuarioAndAutenticacaoAddOrUpdateModel model, Autenticacao autenticacao)
    {
        return new Usuario()
        {
            Nome = model.Nome, DataCriacao = DateTime.Now, NumDocumento = model.NumDocumento, Status = model.Status,
            Telefone = model.Telefone, TipoDocumento = model.TipoDocumento, IdAutenticacao = autenticacao.IdAutenticacao
        };
    }

    public static Usuario MapToUsuarioUpdate(this UsuarioAndAutenticacaoAddOrUpdateModel model, Usuario usuario)
    {
        return new Usuario()
        {
            Nome = model.Nome, DataCriacao = DateTime.Now, NumDocumento = model.NumDocumento, Status = model.Status,
            Telefone = model.Telefone, TipoDocumento = model.TipoDocumento, IdUsuario = usuario.IdUsuario,
            IdAutenticacao = usuario.IdAutenticacao
        };
    }
}