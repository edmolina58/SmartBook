using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartBook.Application.Services.Interface;
using SmartBook.Domain.Dtos.Reponses.UsuariosReponses;
using SmartBook.Domain.Dtos.Requests.LoginRequest;
using SmartBook.Domain.Dtos.Requests.UsuarioRequest;
using SmartBook.Domain.Enums;
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




        [Authorize] 
        [HttpPost]
        public async Task<ActionResult<UsuarioReponse>> Crear(CrearUsuarioRequest request)
        {
            try
            {
                var rolActual = User.Claims.FirstOrDefault(c => c.Type == "rol")?.Value;

                if (request.RolUsuario == RolUsuario.Admin && rolActual != "Admin")
                {
                    return Forbid("Solo los administradores pueden crear cuentas de administradores.");
                }

                var usuario = await _usuarioService.Crear(request);

              
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



        [HttpGet("verificar-email")]
        public async Task<ActionResult> VerificarEmail([FromQuery] string token)
        {
            try
            {
                await _usuarioService.VerificarEmail(token);
                return Ok(new { mensaje = "Email verificado y cuenta confirmada exitosamente." });
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

        [HttpPost("solicitar-restablecer-password")]
        public async Task<ActionResult> SolicitarRestablecerPassword([FromQuery] string email)
        {
            try
            {
                await _usuarioService.EnviarTokenRestablecimientoPasswordAsync(email);
                return Ok(new { mensaje = "Se ha enviado un enlace de restablecimiento a tu correo." });
            }
            catch (BusinessRoleException exb)
            {
                return BadRequest(new { mensaje = exb.Message });
            }
        }

        [HttpPost("restablecer-password")]
        public async Task<ActionResult> RestablecerPassword([FromQuery] string token, [FromQuery] string nuevaPassword)
        {
            try
            {
                await _usuarioService.RestablecerPasswordAsync(token, nuevaPassword);
                return Ok(new { mensaje = "Contraseña restablecida correctamente." });
            }
            catch (BusinessRoleException exb)
            {
                return BadRequest(new { mensaje = exb.Message });
            }
        }

    }
}