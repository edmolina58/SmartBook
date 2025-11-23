using SmartBook.Domain.Dtos.Reponses.UsuariosReponses;
using SmartBook.Domain.Entities;
using SmartBook.Domain.Enums;

namespace SmartBook.Persistence.Repositories.Interface
{
    public interface IUsuarioRepository
    {
        bool ValidarCreacionUsuario(string identificacion);
        void Crear(Usuario usuario);
        Usuario IniciarUsuario(string email, string passwordHash);
        Usuario ObtenerPorEmail(string email);
        Usuario ObtenerPorIdentificacion(string identificacion);
        void Actualizar(Usuario usuario);
        void ActualizarDatos(Usuario usuario);

        public bool BorrarNoVerificado(string id);
        IEnumerable<ConsultarUsuarioReponse> ConsultarPorNombre(string nombreCompleto, RolUsuario? rolUsuario);

    }
}