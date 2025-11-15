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


            var cliente = _clienteService.Crear(request);

            if (cliente is null)
            {
                return BadRequest();
            }
            return Created(string.Empty, cliente);
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
        var cliente = _clienteService.Consultar(id);
        if (cliente is null)
        {
            return NotFound();
        }
        return Ok(cliente);
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

