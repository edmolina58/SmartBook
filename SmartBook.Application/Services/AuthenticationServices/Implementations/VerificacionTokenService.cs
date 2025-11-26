using SmartBook.Application.Services.Authentication.Interfaces;

using SmartBook.Domain.Entities.DomainEntities;
using SmartBook.Persistence.Stores.Interface;
using System.Security.Cryptography;

namespace SmartBook.Application.Services.Authentication.Implementations
{
    public class VerificacionTokenService : IVerificacionTokenService
    {
        private readonly ITokenVerificacionStore _tokenStore;
        private const int MINUTOS_EXPIRACION = 20;

        public VerificacionTokenService(ITokenVerificacionStore tokenStore)
        {
            _tokenStore = tokenStore;
        }

        public string GenerarTokenVerificacion(string email)
        {
            var randomBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
                var token = Convert.ToBase64String(randomBytes)
                    .Replace("+", "-")
                    .Replace("/", "_")
                    .Replace("=", "");

                _tokenStore.EliminarPorEmail(email);

                _tokenStore.Guardar(new TokenVerificacion
                {
                    Email = email,
                    Token = token,
                    Expiracion = DateTime.UtcNow.AddMinutes(MINUTOS_EXPIRACION)
                });

                return token;
            }
        }

        public bool ValidarTokenVerificacion(string token, out string email)
        {
            email = null;

            var tokenData = _tokenStore.ObtenerPorToken(token);

            if (tokenData == null)
                return false;

            if (tokenData.Expiracion < DateTime.UtcNow)
            {
                _tokenStore.Eliminar(token);
                return false;
            }

            email = tokenData.Email;
            _tokenStore.Eliminar(token);

            return true;
        }
    }
}