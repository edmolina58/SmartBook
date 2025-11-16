using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SmartBook.Aplicacion.Extensions;
using SmartBook.Application.Services.Interface;
using SmartBook.Domain.Dtos.Reponses.LoginsReponse;
using SmartBook.Domain.Dtos.Reponses.UsuariosReponses;
using SmartBook.Domain.Dtos.Requests.LoginRequest;
using SmartBook.Domain.Dtos.Requests.UsuarioRequest;
using SmartBook.Domain.Entities;
using SmartBook.Domain.Exceptions;
using SmartBook.Persistence.Repositories.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace SmartBook.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly ITokenService _tokenService;

        public UsuarioService(
            IUsuarioRepository usuarioRepository,
            IConfiguration configuration,
            IEmailService emailService,
            ITokenService tokenService)
        {
            _configuration = configuration;
            _usuarioRepository = usuarioRepository;
            _emailService = emailService;
            _tokenService = tokenService;
        }

        public async Task<UsuarioReponse> Crear(CrearUsuarioRequest request)
        {
            bool validar = _usuarioRepository.ValidarCreacionUsuario(request.identificacion!);
            if (!validar)
            {
                throw new BusinessRoleException("Ya existe un usuario con esta identificación");
            }

            if (!request.EmailUsuario!.EndsWith("@cecar.edu.co"))
            {
                throw new BusinessRoleException("Solo se aceptan correos institucionales (@cecar.edu.co)");
            }

            if (ValidarPassword(request.PasswordUsuario!) is false)
            {
                throw new BusinessRoleException("La contraseña debe tener al menos 8 caracteres");
            }

            // Generar token de verificación
            var tokenVerificacion = _tokenService.GenerarTokenVerificacion(request.EmailUsuario);

            // Enviar email con el token directamente
            await _emailService.EnviarEmailVerificacionAsync(
                request.EmailUsuario,
                request.NombreUsuario,
                tokenVerificacion
            );

            // Crear usuario (pero no guardarlo hasta que se verifique el email)
            var usuario = new Usuario
            {
                IdUsuario = DateTime.Now.Ticks.ToString(),
                Identificacion = request.identificacion!.Sanitize().RemoveAccents(),
                NombreCompleto = request.NombreUsuario!.Sanitize().RemoveAccents(),
                Email = request.EmailUsuario!.Sanitize().RemoveAccents(),
                Password = encriptarpassword(request.PasswordUsuario!),
                RolUsuario = request.RolUsuario
            };

            // Guardar usuario temporalmente (podrías tener un campo "EmailConfirmado" en tu entidad)
            _usuarioRepository.Crear(usuario);

            return new UsuarioReponse("Usuario creado exitosamente. Revisa tu correo para obtener el token de verificación.");
        }

        public async Task VerificarEmail(string token)
        {
            if (!_tokenService.ValidarToken(token, out string email))
            {
                throw new BusinessRoleException("Token de verificación inválido o expirado");
            }

            var usuario = _usuarioRepository.ObtenerPorEmail(email);
            if (usuario == null)
            {
                throw new BusinessRoleException("Usuario no encontrado");
            }

            // Aquí podrías actualizar el campo "EmailConfirmado" a true en tu base de datos
            // _usuarioRepository.ActualizarEmailConfirmado(usuario.IdUsuario, true);

            await _emailService.EnviarEmailConfirmacionCuentaAsync(
                usuario.Email,
                usuario.NombreCompleto
            );
        }

        public async Task EnviarTokenRestablecimientoPasswordAsync(string email)
        {
            var usuario = _usuarioRepository.ObtenerPorEmail(email);
            if (usuario == null)
                throw new BusinessRoleException("Usuario no encontrado");

            var token = _tokenService.GenerarTokenVerificacion(email);

            await _emailService.EnviarEmailRestablecimientoPasswordAsync(
                usuario.Email,
                usuario.NombreCompleto,
                token
            );
        }

        public async Task RestablecerPasswordAsync(string token, string nuevaPassword)
        {
            if (!_tokenService.ValidarToken(token, out string email))
                throw new BusinessRoleException("Token inválido o expirado");

            var usuario = _usuarioRepository.ObtenerPorEmail(email);
            if (usuario == null)
                throw new BusinessRoleException("Usuario no encontrado");

            if (ValidarPassword(nuevaPassword) is false)
                throw new BusinessRoleException("La contraseña debe tener al menos 8 caracteres");

            usuario.Password = encriptarpassword(nuevaPassword);
            _usuarioRepository.Actualizar(usuario);

            await _emailService.EnviarEmailConfirmacionRestablecimientoAsync(
                usuario.Email,
                usuario.NombreCompleto
            );
        }

        // Resto de los métodos existentes...
        public LoginReponse Login(LoginRequest request)
        {
            var passwordHash = encriptarpassword(request.PassWord);
            var iniciarusuario = _usuarioRepository.IniciarUsuario(request.Email, passwordHash);

            if (iniciarusuario is null)
                throw new BusinessRoleException("Credenciales inválidas");

            // Aquí podrías verificar si el email está confirmado
            // if (!iniciarusuario.EmailConfirmado) 
            //     throw new BusinessRoleException("Debes verificar tu email antes de iniciar sesión");

            var token = generarJWT(iniciarusuario);
            return new LoginReponse(token);
        }

        public IEnumerable<ConsultarUsuarioReponse> ConsultarUsuario(ConsultarUsuarioRequest request)
        {
            return _usuarioRepository.ConsultarPorNombre(request.NombreUsuario!, request.RolUsuario);
        }


        private bool ValidarPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;
            var regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).{8,}$");

            return regex.IsMatch(password);
        }

        public string encriptarpassword(string texto)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(texto));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public string generarJWT(Usuario modelo)
        {
            var userClaims = new[]
            {
                new Claim("id", modelo.IdUsuario),
                new Claim("identificacion", modelo.Identificacion),
                new Claim("email", modelo.Email),
                new Claim("rol", modelo.RolUsuario.ToString()),
                new Claim(ClaimTypes.NameIdentifier, modelo.IdUsuario),
                new Claim(ClaimTypes.Email, modelo.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256Signature);

            var jwtConfig = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtConfig);
        }
    }
}