using SmartBook.Domain.Dtos.Reponses.UsuariosReponses;
using SmartBook.Domain.Entities;
using SmartBook.Domain.Enums;

namespace SmartBook.Persistence.Repositories.Interface
{
    public interface IUsuarioRepository
    {
        bool ValidarCreacionUsuario(string identificacion);
        void Crear(Usuario usuario);
        void Actualizar(Usuario usuario);
        Usuario IniciarUsuario(string email, string passwordHash);
        Usuario ObtenerPorEmail(string email);
        IEnumerable<ConsultarUsuarioReponse> ConsultarPorNombre(string nombreCompleto, RolUsuario? rolUsuario);
        bool Borrar(string id);
    }
}