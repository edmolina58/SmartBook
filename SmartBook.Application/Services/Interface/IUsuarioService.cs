using SmartBook.Domain.Dtos.Reponses.LoginsReponse;
using SmartBook.Domain.Dtos.Reponses.UsuariosReponses;
using SmartBook.Domain.Dtos.Requests.LoginRequest;
using SmartBook.Domain.Dtos.Requests.UsuarioRequest;

namespace SmartBook.Application.Services.Interface
{
    public interface IUsuarioService
    {
        LoginReponse Login(LoginRequest request);
        Task<UsuarioReponse> Crear(CrearUsuarioRequest request, string rolActual, string idUsuarioActual);
        IEnumerable<ConsultarUsuarioReponse> ConsultarUsuario(ConsultarUsuarioRequest request);
        Task VerificarEmail(string token);
        Task EnviarTokenRestablecimientoPasswordAsync(string email);
        Task RestablecerPasswordAsync(string token, string nuevaPassword);
        Task<UsuarioReponse> Actualizar(string identificacion, ActualizarUsuarioRequest request, string rolActual, string idUsuarioActual);
    }
}