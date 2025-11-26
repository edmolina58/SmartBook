using SmartBook.Aplicacion.Extensions;
using SmartBook.Application.Services.Authentication.Interfaces;
using SmartBook.Application.Services.Email.Interfaces;
using SmartBook.Application.Services.Usuarios.Interfaces;
using SmartBook.Application.Services.ValidationServices.Implementations;
using SmartBook.Domain.Dtos.Reponses.LoginsReponse;
using SmartBook.Domain.Dtos.Reponses.UsuariosReponses;
using SmartBook.Domain.Dtos.Requests.LoginRequest;
using SmartBook.Domain.Dtos.Requests.UsuarioRequest;
using SmartBook.Domain.Entities.DatabaseEntities;
using SmartBook.Domain.Enums;
using SmartBook.Domain.Exceptions;
using SmartBook.Persistence.Repositories.Interface;

namespace SmartBook.Application.Services.Usuarios.Implementations
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IEmailService _emailService;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IVerificacionTokenService _verificacionTokenService;
        private readonly IPasswordService _passwordService;
        private readonly IAutorizacionService _autorizacionService;
        private const string ID_ADMIN_DEFECTO = "ADMIN_DEFAULT_001";
        public UsuarioService(
            IUsuarioRepository usuarioRepository,
            IEmailService emailService,
            IJwtTokenService jwtTokenService,
            IVerificacionTokenService verificacionTokenService,
            IPasswordService passwordService,
            IAutorizacionService autorizacionService)
        {
            _usuarioRepository = usuarioRepository;
            _emailService = emailService;
            _jwtTokenService = jwtTokenService;
            _verificacionTokenService = verificacionTokenService;
            _passwordService = passwordService;
            _autorizacionService = autorizacionService;
        }

        public LoginReponse Login(LoginRequest request)
        {
            var email = request.Email.Sanitize().RemoveAccents();
            var usuario = _usuarioRepository.ObtenerPorEmail(email);

            if (usuario is null)
                throw new BusinessRoleException("Credenciales inválidas");

            if (!_passwordService.VerificarPassword(request.PassWord.Sanitize().RemoveAccents(), usuario.Password))
                throw new BusinessRoleException("Credenciales inválidas");

            if (usuario.RolUsuario == RolUsuario.Suspendida)
            {
                throw new BusinessRoleException("Tu cuenta ha sido suspendida. Contacta al administrador.");
            }

            if (!EstaVerificado(usuario.Email))
            {
                throw new BusinessRoleException("Debes verificar tu cuenta antes de iniciar sesión. Revisa tu correo electrónico.");
            }

            var token = _jwtTokenService.GenerarTokenJWT(usuario);

            return new LoginReponse(token);
        }

        public async Task<UsuarioReponse> Crear(CrearUsuarioRequest request, string rolActual, string idUsuarioActual)
        {
            var identificacion = request.identificacion!.Sanitize().RemoveAccents();
            var nombre = request.NombreUsuario!.Sanitize().RemoveAccents();
            var email = request.EmailUsuario!.Sanitize().RemoveAccents();

            var rolActualEnum = Enum.Parse<RolUsuario>(rolActual);
            _autorizacionService.ValidarPermisoCreacion(rolActualEnum, request.RolUsuario);

            if (!_usuarioRepository.ValidarCreacionUsuario(identificacion))
            {
                throw new BusinessRoleException("Ya existe un usuario");
            }

            if (!email.EndsWith("@cecar.edu.co"))
            {
                throw new BusinessRoleException("Solo se aceptan correos institucionales");
            }

            if (!_passwordService.ValidarPassword(request.PasswordUsuario!))
            {
                throw new BusinessRoleException("Digite una contraseña segura porfavor");
            }

            var tokenVerificacion = _verificacionTokenService.GenerarTokenVerificacion(email);

            var usuario = new Usuario
            {
                IdUsuario = DateTime.Now.Ticks.ToString(),
                Identificacion = identificacion,
                NombreCompleto = nombre,
                Email = email.Trim(),
                Password = _passwordService.HashPassword(request.PasswordUsuario!),
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
            if (!_verificacionTokenService.ValidarTokenVerificacion(token, out string email))
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

            var token = _verificacionTokenService.GenerarTokenVerificacion(email);

            _emailService.EnviarEmailRestablecimientoPasswordAsync(
                usuario.Email,
                usuario.NombreCompleto,
                token
            );
        }

        public async Task RestablecerPasswordAsync(string token, string nuevaPassword)
        {
            if (!_verificacionTokenService.ValidarTokenVerificacion(token, out string email))
                throw new BusinessRoleException("Token inválido o expirado");

            var usuario = _usuarioRepository.ObtenerPorEmail(email.Sanitize().RemoveAccents());
            if (usuario == null)
                throw new BusinessRoleException("Usuario no encontrado");

            if (!_passwordService.ValidarPassword(nuevaPassword.Sanitize().RemoveAccents()))
                throw new BusinessRoleException("La contraseña debe tener al menos 8 caracteres, una mayúscula, una minúscula, un número y un carácter especial");

            var usuarioActualizado = new Usuario
            {
                Identificacion = usuario.Identificacion,
                NombreCompleto = usuario.NombreCompleto,
                Email = usuario.Email,
                Password = _passwordService.HashPassword(nuevaPassword),
                fecha_actualizacion = DateTime.Now
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

        public void ValidarProteccionAdministrador(string idUsuario)
        {
            if (idUsuario == ID_ADMIN_DEFECTO)
            {
                throw new BusinessRoleException(
                    "No se puede modificar el administrador por defecto del sistema");
            }
        }
        public async Task<UsuarioReponse> Actualizar(
        string identificacion,
        ActualizarUsuarioRequest request,
        string rolActual,
        string idUsuarioActual)
        {
            var usuarioExistente = _usuarioRepository.ObtenerPorIdentificacion(
                identificacion.Sanitize().RemoveAccents());

            if (usuarioExistente == null)
                throw new BusinessRoleException("Usuario no encontrado");

            ValidarProteccionAdministrador(usuarioExistente.IdUsuario);

            _autorizacionService.ValidarPermisoActualizacion(rolActual, idUsuarioActual, usuarioExistente.IdUsuario);

            if (!EstaVerificado(usuarioExistente.Email))
            {
                throw new BusinessRoleException(
                    "No se puede actualizar una cuenta que no ha sido verificada");
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
                IdUsuario = usuarioExistente.IdUsuario,
                Identificacion = usuarioExistente.Identificacion,
                Password = usuarioExistente.Password,
                NombreCompleto = nombreFinal,
                Email = emailFinal,
                RolUsuario = request.RolUsuario,
                EmailVerificado = usuarioExistente.EmailVerificado,
                fecha_creacion = usuarioExistente.fecha_creacion,
                fecha_actualizacion = DateTime.Now
            };

            _usuarioRepository.ActualizarDatos(usuarioActualizado);

            return new UsuarioReponse("Usuario actualizado exitosamente");
        }

        private bool EstaVerificado(string email)
        {
            return LimpiadorCuentasPendientesService.EstaVerificado(email);
        }
    }
}