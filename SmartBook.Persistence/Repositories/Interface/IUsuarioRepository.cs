using SmartBook.Domain.Dtos.Reponses.UsuariosReponses;
using SmartBook.Domain.Entities.DatabaseEntities;
using SmartBook.Domain.Enums;

namespace SmartBook.Persistence.Repositories.Interface
{
    public interface IUsuarioRepository
    {
        bool ValidarCreacionUsuario(string identificacion);
        void Crear(Usuario usuario);
        Usuario ObtenerPorIdentificacion(string identificacion);
        void ActualizarDatos(Usuario usuario);
        Usuario IniciarUsuario(string email, string passwordHash);
        Usuario ObtenerPorEmail(string email);
        bool Actualizar(Usuario usuario);
        IEnumerable<ConsultarUsuarioReponse> ConsultarPorNombre(string nombreCompleto, RolUsuario? rolUsuario);
        bool BorrarNoVerificado(string id);
        bool ExistePorCorreo(string correo);
    }
}