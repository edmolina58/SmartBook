using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartBook.Application.Interface;
using SmartBook.Domain.Dtos.Requests.ClienteRequest;
using SmartBook.Domain.Dtos.Requests.ClientesRequest;
using SmartBook.Domain.Exceptions;

namespace SmartBook.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ClientesController : ControllerBase
{
    private readonly IClienteService _clienteService;

    public ClientesController(IClienteService clienteService)
    {
        _clienteService = clienteService;

    }

    [HttpPost]
    public ActionResult Crear(CrearClienteRequest request)
    {
        try
        {


            var libro = _clienteService.Crear(request);

            if (libro is null)
            {
                return BadRequest();
            }
            return Created(string.Empty, libro);
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
        var borrado = _clienteService.Borrar(id);
        if (!borrado)
        {
            return NotFound();
        }
        return NoContent();
    }


    [HttpGet("{id}")]
    public ActionResult Consultar(string id)
    {
        var libro = _clienteService.Consultar(id);
        if (libro is null)
        {
            return NotFound();
        }
        return Ok(libro);
    }

    [HttpGet]
    public ActionResult Consultar([FromQuery] ConsultarClienteFiltradoNombreRequest request)
    {

        var cliente = _clienteService.ConsultarPorIdentificacion(request);
        return Ok(cliente);
    }

    [HttpPut("{id}")]
    public ActionResult Actualizar(string id, ActualizarClienteRequest request)
    {
        var libro = _clienteService.Actualizar(id, request);

        if (!libro)
        {

            return NotFound();
        }
        return NoContent();
    }
}

