using SmartBook.Domain.Entities.DatabaseEntities;
using System.Security.Claims;

namespace SmartBook.Application.Services.Authentication.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerarTokenJWT(Usuario usuario);
        bool ValidarTokenJWT(string token, out ClaimsPrincipal? principal);
        string ObtenerRolDeToken(string token);
        string ObtenerIdUsuarioDeToken(string token);
    }
}