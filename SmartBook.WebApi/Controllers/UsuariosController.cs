using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using SmartBook.Domain.Dtos.Requests.UsuarioRequest;
using SmartBook.Domain.Exceptions;
using SmartBook.WebApi.Services;

namespace SmartBook.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        //Recordar que REST es sin estado
        private readonly UsuarioService _usuarioService;

        [HttpPost]
        public ActionResult Crear(CrearUsuarioRequest request)
        {
            try
            {
                var usuario = _usuarioService.Crear(request);

                if (usuario is null)
                {
                    return BadRequest();
                }
                return Created(string.Empty, usuario);
            }
            catch (BusinessRoleException exb)
            {
                return UnprocessableEntity(exb.Message);
            }
            catch (Exception exg)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exg.Message);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Borrar(string id)
        {
            var borrado = _usuarioService.Borrar(id);
            if (!borrado)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpGet("{id}")]
        public ActionResult Consultar(string id)
        {
            var usuario = _usuarioService.Consultar(id);
            if (usuario is null)
            {
                return NotFound();
            }
            return Ok(usuario);
        }

        [HttpGet]
        public ActionResult ConsultarTodos([FromQuery] ConsultarUsuarioRequest request)
        {
            var usuarios = _usuarioService.Consultar(request).ToList();
            return Ok(usuarios);
        }

        [HttpPut("{id}")]
        public ActionResult Actualizar(string id, ActualizarUsuarioRequest request)
        {
            var actualizado = _usuarioService.Actualizar(id, request);

            if (!actualizado)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPost("login")]
        public ActionResult Login(LoginRequest request)
        {
            try
            {
                // Aquí va tu lógica de login cuando la tengas
                return Ok();
            }
            catch (BusinessRoleException exb)
            {
                return Unauthorized(exb.Message);
            }
            catch (Exception exg)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exg.Message);
            }
        }

        [HttpPost("{id}/activar")]
        public ActionResult Activar(string id)
        {
            var activado = _usuarioService.Activar(id);
            if (!activado)
            {
                return NotFound();
            }
            return Ok(new { message = "Usuario activado correctamente" });
        }
    }
}