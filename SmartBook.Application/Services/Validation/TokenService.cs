using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SmartBook.Application.Services.Interface;
using SmartBook.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SmartBook.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private static readonly List<TokenVerificacion> _tokens = new();
        private static readonly object _lock = new();

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerarTokenJWT(Usuario usuario)
        {
            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)
            );
            var credentials = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256
            );

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.IdUsuario),
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),

                new Claim("id", usuario.IdUsuario),
                new Claim("identificacion", usuario.Identificacion),
                new Claim("nombreCompleto", usuario.NombreCompleto),
                new Claim("rol", usuario.RolUsuario.ToString()),

                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.RolUsuario.ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = credentials,
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                NotBefore = DateTime.UtcNow
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public bool ValidarTokenJWT(string token, out ClaimsPrincipal? principal)
        {
            principal = null;

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidAudience = _configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                };

                principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return true;
            }
            catch
            {
                return false;
            }
        }

        // ✅ CAMBIADO A 20 MINUTOS
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

                lock (_lock)
                {
                    _tokens.RemoveAll(t => t.Email == email);

                    _tokens.Add(new TokenVerificacion
                    {
                        Email = email,
                        Token = token,
                        Expiracion = DateTime.UtcNow.AddMinutes(20) // ✅ 20 MINUTOS
                    });
                }

                return token;
            }
        }

        public bool ValidarTokenVerificacion(string token, out string email)
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

        public string ObtenerRolDeToken(string token)
        {
            if (ValidarTokenJWT(token, out ClaimsPrincipal principal))
            {
                return principal?.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
            }
            return string.Empty;
        }

        public string ObtenerIdUsuarioDeToken(string token)
        {
            if (ValidarTokenJWT(token, out ClaimsPrincipal principal))
            {
                return principal?.FindFirst("id")?.Value ?? string.Empty;
            }
            return string.Empty;
        }
    }
}