using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using SmartBook.Application.Interface;
using SmartBook.Domain.Dtos.Requests.LibroRequest;
using SmartBook.Domain.Dtos.Requests.LibrosRequest;
using SmartBook.Domain.Dtos.Requests.UsuarioRequest;
using SmartBook.Domain.Exceptions;
using SmartBook.WebApi.Services;

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




    
        [HttpGet]
        public ActionResult Consultar([FromQuery] ConsultarUsuarioRequest request)
        {

            var usuario = _usuarioService.ConsultarUsuario(request);
            if (usuario  is null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }

        /*
        [HttpPut("{id}")]
        public ActionResult Actualizar(string id, ActualizarUsuarioRequest request)
        {
            var usuario = _usuarioService.Actualizar(id, request);

            if (!usuario)
            {

                return NotFound();
            }
            return NoContent();
        }*/


    }
}