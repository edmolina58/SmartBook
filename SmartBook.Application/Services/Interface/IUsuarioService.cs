using SmartBook.Domain.Dtos.Reponses.LoginsReponse;
using SmartBook.Domain.Dtos.Reponses.UsuariosReponses;
using SmartBook.Domain.Dtos.Requests.LoginRequest;
using SmartBook.Domain.Dtos.Requests.UsuarioRequest;

namespace SmartBook.Application.Services.Interface
{
    public interface IUsuarioService
    {
        Task EnviarTokenRestablecimientoPasswordAsync(string email);
        Task RestablecerPasswordAsync(string token, string nuevaPassword);
        Task<UsuarioReponse> Crear(CrearUsuarioRequest request);
        LoginReponse Login(LoginRequest request);
        Task VerificarEmail(string token);
        IEnumerable<ConsultarUsuarioReponse> ConsultarUsuario(ConsultarUsuarioRequest request);
    }
}