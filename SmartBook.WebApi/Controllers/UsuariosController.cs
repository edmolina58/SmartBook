using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartBook.Application.Services.Usuarios.Interfaces;
using SmartBook.Domain.Dtos.Reponses.UsuariosReponses;
using SmartBook.Domain.Dtos.Requests.LoginRequest;
using SmartBook.Domain.Dtos.Requests.UsuarioRequest;
using SmartBook.Domain.Exceptions;

namespace SmartBook.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<object> Login(LoginRequest request)
        {
            try
            {
                var loginResponse = _usuarioService.Login(request);
                return Ok(new
                {
                    success = true,
                    mensaje = "Inicio de sesión exitoso",
                    token = loginResponse.Tokem,
                    expiracion = DateTime.UtcNow.AddHours(1)
                });
            }
            catch (BusinessRoleException exb)
            {
                return Unauthorized(new { success = false, mensaje = exb.Message });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { success = false, mensaje = "Ocurrió un error al procesar la solicitud" });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<UsuarioReponse>> Crear(CrearUsuarioRequest request)
        {
            try
            {
                var rolActual = User.FindFirst("rol")?.Value ?? string.Empty;
                var idUsuarioActual = User.FindFirst("id")?.Value ?? string.Empty;

                var usuario = await _usuarioService.Crear(request, rolActual, idUsuarioActual);
                return Created(string.Empty, usuario);
            }
            catch (BusinessRoleException exb)
            {
                return UnprocessableEntity(new { mensaje = exb.Message });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { mensaje = "Ocurrió un error al procesar la solicitud" });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<IEnumerable<ConsultarUsuarioReponse>> Consultar([FromQuery] ConsultarUsuarioRequest request)
        {
            try
            {
                var usuarios = _usuarioService.ConsultarUsuario(request);
                return Ok(usuarios);
            }
            catch (BusinessRoleException exb)
            {
                return BadRequest(new { mensaje = exb.Message });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { mensaje = "Ocurrió un error al procesar la solicitud" });
            }
        }

        [HttpPut("{identificacion}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<UsuarioReponse>> Actualizar(string identificacion, ActualizarUsuarioRequest request)
        {
            try
            {
                var rolActual = User.FindFirst("rol")?.Value ?? string.Empty;
                var idUsuarioActual = User.FindFirst("id")?.Value ?? string.Empty;

                var resultado = await _usuarioService.Actualizar(identificacion, request, rolActual, idUsuarioActual);
                return Ok(resultado);
            }
            catch (BusinessRoleException exb)
            {
                return UnprocessableEntity(new { mensaje = exb.Message });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { mensaje = "Ocurrió un error al procesar la solicitud" });
            }
        }


        [HttpGet("verificar-email")]
        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> VerificarEmail([FromQuery] string token)
        {
            try
            {
                await _usuarioService.VerificarEmail(token);

                return Content(@"
                    <!DOCTYPE html>
                    <html>
                    <head>
                        <meta charset='utf-8'>
                        <title>Cuenta Verificada - SmartBook</title>
                        <style>
                            body { 
                                font-family: Arial, sans-serif; 
                                background-color: #f4f4f4; 
                                display: flex; 
                                justify-content: center; 
                                align-items: center; 
                                height: 100vh; 
                                margin: 0;
                            }
                            .container { 
                                background: white; 
                                padding: 40px; 
                                border-radius: 10px; 
                                box-shadow: 0 2px 10px rgba(0,0,0,0.1); 
                                text-align: center;
                                max-width: 500px;
                            }
                            .success { color: #28a745; font-size: 64px; margin-bottom: 20px; }
                            h2 { color: #333; margin: 20px 0; }
                            p { color: #666; line-height: 1.6; }
                            .logo { color: #007bff; margin-bottom: 20px; font-size: 24px; }
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <div class='logo'><strong>📚 SmartBook - CDI CECAR</strong></div>
                            <div class='success'>✓</div>
                            <h2>¡Cuenta Verificada Exitosamente!</h2>
                            <p>Tu cuenta ha sido confirmada.</p>
                            <p>Ya puedes iniciar sesión en SmartBook.</p>
                            <p style='font-size: 12px; color: #999; margin-top: 30px;'>Puedes cerrar esta ventana</p>
                        </div>
                    </body>
                    </html>
                ", "text/html");
            }
            catch (BusinessRoleException exb)
            {
                return Content($@"
                    <!DOCTYPE html>
                    <html>
                    <head>
                        <meta charset='utf-8'>
                        <title>Error - SmartBook</title>
                        <style>
                            body {{ 
                                font-family: Arial, sans-serif; 
                                background-color: #f4f4f4; 
                                display: flex; 
                                justify-content: center; 
                                align-items: center; 
                                height: 100vh; 
                                margin: 0;
                            }}
                            .container {{ 
                                background: white; 
                                padding: 40px; 
                                border-radius: 10px; 
                                box-shadow: 0 2px 10px rgba(0,0,0,0.1); 
                                text-align: center;
                                max-width: 500px;
                            }}
                            .error {{ color: #dc3545; font-size: 64px; margin-bottom: 20px; }}
                            h2 {{ color: #333; }}
                            p {{ color: #666; line-height: 1.6; }}
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <div class='error'>✗</div>
                            <h2>Error al Verificar</h2>
                            <p>{exb.Message}</p>
                        </div>
                    </body>
                    </html>
                ", "text/html");
            }
            catch
            {
                return Content(@"
                    <!DOCTYPE html>
                    <html>
                    <head>
                        <meta charset='utf-8'>
                        <title>Error - SmartBook</title>
                        <style>
                            body { 
                                font-family: Arial, sans-serif; 
                                background-color: #f4f4f4; 
                                display: flex; 
                                justify-content: center; 
                                align-items: center; 
                                height: 100vh; 
                                margin: 0;
                            }
                            .container { 
                                background: white; 
                                padding: 40px; 
                                border-radius: 10px; 
                                box-shadow: 0 2px 10px rgba(0,0,0,0.1); 
                                text-align: center;
                            }
                            .error { color: #dc3545; font-size: 64px; margin-bottom: 20px; }
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <div class='error'>✗</div>
                            <h2>Error del Servidor</h2>
                        </div>
                    </body>
                    </html>
                ", "text/html");
            }
        }

        [HttpPost("solicitar-restablecer-password")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> SolicitarRestablecerPassword([FromQuery] string email)
        {
            try
            {
                await _usuarioService.EnviarTokenRestablecimientoPasswordAsync(email);
                return Ok(new { mensaje = "Si el email existe, recibirás un token de restablecimiento" });
            }
            catch
            {
                return Ok(new { mensaje = "Si el email existe, recibirás un token de restablecimiento" });
            }
        }

        [HttpPatch("restablecer-password")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> RestablecerPassword([FromQuery] string token, [FromQuery] string nuevaPassword)
        {
            try
            {
                await _usuarioService.RestablecerPasswordAsync(token, nuevaPassword);
                return Ok(new { mensaje = "Contraseña restablecida correctamente" });
            }
            catch (BusinessRoleException exb)
            {
                return BadRequest(new { mensaje = exb.Message });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { mensaje = "Ocurrió un error al procesar la solicitud" });
            }
        }
    }
}