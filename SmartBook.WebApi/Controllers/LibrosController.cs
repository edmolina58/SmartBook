using Microsoft.AspNetCore.Mvc;
using SmartBook.Application.Interface;
using SmartBook.Domain.Dtos.Requests.LibroRequest;
using SmartBook.Domain.Dtos.Requests.LibrosRequest;
using SmartBook.Domain.Exceptions;
using SmartBook.Services;
using SmartBook.WebApi.Services;


namespace SmartBook.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class LibrosController : ControllerBase
{
    private readonly ILibroService _libroService ;
    public LibrosController(ILibroService libroService)
    {
        _libroService = libroService;

    }

    [HttpPost]
    public ActionResult Crear(CrearLibroRequest request)
    {
        try
        {

            var libro = _libroService.Crear(request);

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
        var borrado = _libroService.Borrar(id);
        if (!borrado)
        {
            return NotFound();
        }
        return NoContent();
    }


    [HttpGet("{id}")]
    public ActionResult Consultar(string id)
    {
        var libro = _libroService.Consultar(id);
        if (libro is null)
        {
            return NotFound();
        }
        return Ok(libro);
    }
    [HttpGet]
    public ActionResult Consultar([FromQuery] ConsultarLibroFiltrados request)
    {

        var libro = _libroService.ConsultarProductosCompletos(request);
        if (libro is null)
        { 
        return NotFound();
        }

        return Ok(libro);
    }

    [HttpPut("{id}")]
    public ActionResult Actualizar(string id, ActualizarLibrosRequest request)
    {
        var libro = _libroService.Actualizar(id, request);

        if (!libro)
        {

            return NotFound();
        }
        return NoContent();
    }
}

