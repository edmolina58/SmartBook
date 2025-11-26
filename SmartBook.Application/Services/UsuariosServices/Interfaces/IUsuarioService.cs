using SmartBook.Domain.Dtos.Reponses.LoginsReponse;
using SmartBook.Domain.Dtos.Reponses.UsuariosReponses;
using SmartBook.Domain.Dtos.Requests.LoginRequest;
using SmartBook.Domain.Dtos.Requests.UsuarioRequest;

namespace SmartBook.Application.Services.Usuarios.Interfaces
{
    public interface IUsuarioService
    {
        LoginReponse Login(LoginRequest request);
        Task<UsuarioReponse> Crear(CrearUsuarioRequest request, string rolActual, string idUsuarioActual);
        Task VerificarEmail(string token);
        Task EnviarTokenRestablecimientoPasswordAsync(string email);
        Task RestablecerPasswordAsync(string token, string nuevaPassword);
        IEnumerable<ConsultarUsuarioReponse> ConsultarUsuario(ConsultarUsuarioRequest request);
        Task<UsuarioReponse> Actualizar(string identificacion, ActualizarUsuarioRequest request, string rolActual, string idUsuarioActual);
        void ValidarProteccionAdministrador(string idUsuario);
    }
}