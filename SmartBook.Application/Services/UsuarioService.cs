using SmartBook.Aplicacion.Extensions;
using SmartBook.Application.Services.Interface;
using SmartBook.Domain.Dtos.Reponses.LoginsReponse;
using SmartBook.Domain.Dtos.Reponses.UsuariosReponses;
using SmartBook.Domain.Dtos.Requests.LoginRequest;
using SmartBook.Domain.Dtos.Requests.UsuarioRequest;
using SmartBook.Domain.Entities;
using SmartBook.Domain.Enums;
using SmartBook.Domain.Exceptions;
using SmartBook.Persistence.Repositories.Interface;
using System.Text.RegularExpressions;

namespace SmartBook.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IEmailService _emailService;
        private readonly ITokenService _tokenService;

        public UsuarioService(
            IUsuarioRepository usuarioRepository,
            IEmailService emailService,
            ITokenService tokenService)
        {
            _usuarioRepository = usuarioRepository;
            _emailService = emailService;
            _tokenService = tokenService;
        }

        public LoginReponse Login(LoginRequest request)
        {
            var email = request.Email.Sanitize().RemoveAccents();
            var usuario = _usuarioRepository.ObtenerPorEmail(email);

            if (usuario is null)
                throw new BusinessRoleException("Credenciales inválidas");


            if (VerificarPassword(request.PassWord.Sanitize().RemoveAccents(), usuario.Password) is false)
                throw new BusinessRoleException("Credenciales inválidas");


            if (usuario.RolUsuario == RolUsuario.Suspendida)
            {
                throw new BusinessRoleException("Tu cuenta ha sido suspendida. Contacta al administrador.");
            }

            if (!EstaVerificado(usuario.Email))
            {
                throw new BusinessRoleException("Debes verificar tu cuenta antes de iniciar sesión. Revisa tu correo electrónico.");
            }

            var token = _tokenService.GenerarTokenJWT(usuario);

            return new LoginReponse(token);
        }

        public async Task<UsuarioReponse> Crear(CrearUsuarioRequest request, string rolActual, string idUsuarioActual)
        {
            var identificacion = request.identificacion!.Sanitize().RemoveAccents();
            var nombre = request.NombreUsuario!.Sanitize().RemoveAccents();
            var email = request.EmailUsuario!.Sanitize().RemoveAccents();

            if (rolActual != RolUsuario.Admin.ToString())
            {
                throw new BusinessRoleException("Solo los administradores pueden crear usuarios");
            }

            if (request.RolUsuario == RolUsuario.Admin && rolActual != RolUsuario.Admin.ToString())
            {
                throw new BusinessRoleException("Solo los administradores pueden crear cuentas de administradores");
            }

            if (!_usuarioRepository.ValidarCreacionUsuario(identificacion))
            {
                throw new BusinessRoleException("Ya existe un usuario");
            }

            if (!email.EndsWith("@cecar.edu.co"))
            {
                throw new BusinessRoleException("Solo se aceptan correos institucionales");
            }

            if (!ValidarPassword(request.PasswordUsuario!))
            {
                throw new BusinessRoleException("Digite una contraseña segura porfavor");
            }

            var tokenVerificacion = _tokenService.GenerarTokenVerificacion(email);


            var usuario = new Usuario
            {
                IdUsuario = DateTime.Now.Ticks.ToString(),
                Identificacion = identificacion,
                NombreCompleto = nombre,
                Email = email.Trim(),
                Password = HashPassword(request.PasswordUsuario!),
                RolUsuario = request.RolUsuario,
                fecha_creacion = DateTime.Now
            };

            _usuarioRepository.Crear(usuario);

            LimpiadorCuentasPendientesService.AgregarCuentaPendiente(identificacion, email);

            _emailService.EnviarEmailVerificacionAsync(email, nombre, tokenVerificacion);

            return new UsuarioReponse("Usuario creado exitosamente. Se ha enviado un enlace de verificación al correo (válido por 20 minutos).");
        }

        public async Task VerificarEmail(string token)
        {
            if (!_tokenService.ValidarTokenVerificacion(token, out string email))
            {
                throw new BusinessRoleException("Enlace de verificación inválido o expirado (más de 20 minutos).");
            }

            var usuario = _usuarioRepository.ObtenerPorEmail(email);
            if (usuario == null)
            {
                throw new BusinessRoleException("Usuario no encontrado");
            }

            LimpiadorCuentasPendientesService.MarcarComoVerificada(email);

            _emailService.EnviarEmailConfirmacionCuentaAsync(usuario.Email, usuario.NombreCompleto);
        }

        public async Task EnviarTokenRestablecimientoPasswordAsync(string email)
        {
            email = email.Sanitize().RemoveAccents();

            var usuario = _usuarioRepository.ObtenerPorEmail(email);
            if (usuario == null)
                return;

            var token = _tokenService.GenerarTokenVerificacion(email);

            _emailService.EnviarEmailRestablecimientoPasswordAsync(
                usuario.Email,
                usuario.NombreCompleto,
                token
            );
        }

        public async Task RestablecerPasswordAsync(string token, string nuevaPassword)
        {
            if (!_tokenService.ValidarTokenVerificacion(token, out string email))
                throw new BusinessRoleException("Token inválido o expirado");

            var usuario = _usuarioRepository.ObtenerPorEmail(email);
            if (usuario == null)
                throw new BusinessRoleException("Usuario no encontrado");

            if (!ValidarPassword(nuevaPassword))
                throw new BusinessRoleException("La contraseña debe tener al menos 8 caracteres, una mayúscula, una minúscula, un número y un carácter especial");


            var usuarioActualizado = new Usuario
            {
                IdUsuario = usuario.IdUsuario,
                Identificacion = usuario.Identificacion,
                NombreCompleto = usuario.NombreCompleto,
                Email = usuario.Email,
                Password = HashPassword(nuevaPassword),
                RolUsuario = usuario.RolUsuario
            };

            _usuarioRepository.Actualizar(usuarioActualizado);

            _emailService.EnviarEmailConfirmacionRestablecimientoAsync(
                usuario.Email,
                usuario.NombreCompleto
            );
        }

        public IEnumerable<ConsultarUsuarioReponse> ConsultarUsuario(ConsultarUsuarioRequest request)
        {
            var nombre = request.NombreUsuario?.Sanitize().RemoveAccents();
            return _usuarioRepository.ConsultarPorNombre(nombre, request.RolUsuario);
        }

        public async Task<UsuarioReponse> Actualizar(string identificacion, ActualizarUsuarioRequest request, string rolActual, string idUsuarioActual)
        {
            if (rolActual != RolUsuario.Admin.ToString())
            {
                throw new BusinessRoleException("Solo los administradores pueden actualizar usuarios");
            }

            var usuarioExistente = _usuarioRepository.ObtenerPorIdentificacion(identificacion.Sanitize());
            if (usuarioExistente == null)
                throw new BusinessRoleException("Usuario no encontrado");

            if (usuarioExistente.IdUsuario == idUsuarioActual)
            {
                throw new BusinessRoleException("No puedes modificar tu propia cuenta");
            }

            if (!EstaVerificado(usuarioExistente.Email))
            {
                throw new BusinessRoleException("No se puede actualizar una cuenta que no ha sido verificada");
            }

            var nombreFinal = !string.IsNullOrWhiteSpace(request.NombreUsuario)
               ? request.NombreUsuario.Sanitize().RemoveAccents()
               : usuarioExistente.NombreCompleto;

            var emailFinal = usuarioExistente.Email;
            if (!string.IsNullOrWhiteSpace(request.EmailUsuario))
            {
                var email = request.EmailUsuario.Sanitize().RemoveAccents();
                if (!email.EndsWith("@cecar.edu.co"))
                    throw new BusinessRoleException("Solo se aceptan correos institucionales");
                emailFinal = email;
            }

            var usuarioActualizado = new Usuario
            {
                IdUsuario = usuarioExistente.IdUsuario.Sanitize().RemoveAccents(),
                Identificacion = usuarioExistente.Identificacion,
                Password = usuarioExistente.Password,
                NombreCompleto = nombreFinal.Sanitize().RemoveAccents(),
                Email = emailFinal.Sanitize().RemoveAccents(),
                RolUsuario = request.RolUsuario
            };

            _usuarioRepository.ActualizarDatos(usuarioActualizado);

            return new UsuarioReponse("Usuario actualizado exitosamente");
        }

        private bool EstaVerificado(string email)
        {
            return LimpiadorCuentasPendientesService.EstaVerificado(email);
        }

        private bool ValidarPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
                return false;

            var regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).{8,}$");
            return regex.IsMatch(password);
        }


        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
        }


        private bool VerificarPassword(string passwordIngresado, string passwordHasheado)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(passwordIngresado, passwordHasheado);
            }
                 catch 
                {

                return false;
                 }
                
            }
        }
    }