using SmartBook.Application.Services.Interface;
using SmartBook.Domain.Entities;
using System.Security.Cryptography;

namespace SmartBook.Application.Services
{
    public class TokenService : ITokenService
    {
        private static readonly List<TokenVerificacion> _tokens = new();
        private static readonly object _lock = new();

        public string GenerarTokenVerificacion(string email)
        {
            var randomBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
                var token = Convert.ToBase64String(randomBytes);

                lock (_lock)
                {
                    // Limpiar tokens antiguos del mismo email
                    _tokens.RemoveAll(t => t.Email == email);

                    // Agregar nuevo token con init
                    _tokens.Add(new TokenVerificacion
                    {
                        Email = email,
                        Token = token,
                        Expiracion = DateTime.UtcNow.AddHours(1)
                    });
                }

                return token;
            }
        }

        public bool ValidarToken(string token, out string email)
        {
            email = null;

            lock (_lock)
            {
                var tokenData = _tokens.FirstOrDefault(t => t.Token == token);

                if (tokenData == null)
                    return false;

                if (tokenData.Expiracion < DateTime.UtcNow)
                {
                    _tokens.Remove(tokenData);
                    return false;
                }

                email = tokenData.Email;
                _tokens.Remove(tokenData);

                return true;
            }
        }

        public void LimpiarTokensExpirados()
        {
            lock (_lock)
            {
                _tokens.RemoveAll(t => t.Expiracion < DateTime.UtcNow);
            }
        }
    }
}