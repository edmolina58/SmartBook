using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartBook.Application.Services.ClientesServices.Interfaces;
using SmartBook.Domain.Dtos.Requests.ClienteRequest;
using SmartBook.Domain.Dtos.Requests.ClientesRequest;
using SmartBook.Domain.Exceptions;

namespace SmartBook.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] 
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClientesController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }


        [HttpPost]
        [Authorize(Roles = "Admin,Vendedor")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public ActionResult Crear(CrearClienteRequest request)
        {
            try
            {
                var cliente = _clienteService.Crear(request);

                if (cliente is null)
                {
                    return BadRequest(new { mensaje = "No se pudo crear el cliente" });
                }

                return Created(string.Empty, cliente);
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

        [HttpGet("{identificacion}")]
        [Authorize(Roles = "Admin,Vendedor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Consultar(string identificacion)
        {
            try
            {
                var cliente = _clienteService.Consultar(identificacion);

                if (cliente is null)
                {
                    return NotFound(new { mensaje = "Cliente no encontrado" });
                }

                return Ok(cliente);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { mensaje = "Ocurrió un error al procesar la solicitud" });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Vendedor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult Consultar([FromQuery] ConsultarClienteFiltradoNombreRequest request)
        {
            try
            {
                var clientes = _clienteService.ConsultarPorIdentificacion(request);
                return Ok(clientes);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { mensaje = "Ocurrió un error al procesar la solicitud" });
            }
        }

       
        [HttpPut("{identificacion}")]
        [Authorize(Roles = "Admin,Vendedor")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Actualizar(string identificacion, ActualizarClienteRequest request)
        {
            try
            {
                var actualizado = _clienteService.Actualizar(identificacion, request);

                if (!actualizado)
                {
                    return NotFound(new { mensaje = "Cliente no encontrado" });
                }

                return NoContent();
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


    }
}