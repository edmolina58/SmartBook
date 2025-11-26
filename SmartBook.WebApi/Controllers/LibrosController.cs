using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartBook.Application.Services.Libros.Interfaces;
using SmartBook.Domain.Dtos.Requests.LibroRequest;
using SmartBook.Domain.Dtos.Requests.LibrosRequest;
using SmartBook.Domain.Exceptions;

namespace SmartBook.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize] 
public class LibrosController : ControllerBase
{
    private readonly ILibroService _libroService;

    public LibrosController(ILibroService libroService)
    {
        _libroService = libroService;
    }


    [HttpPost]
    [Authorize(Roles = "Admin,Vendedor")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
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
            return UnprocessableEntity(new { mensaje = exb.Message });
        }
        catch (Exception exg)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { mensaje = "Error al procesar la solicitud" });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Vendedor")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult Borrar(string id)
    {
        var borrado = _libroService.Borrar(id);
        if (!borrado)
        {
            return NotFound(new { mensaje = "Libro no encontrado" });
        }
        return NoContent();
    }

   
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Vendedor")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult Consultar(string id)
    {
        var libro = _libroService.Consultar(id);
        if (libro is null)
        {
            return NotFound(new { mensaje = "Libro no encontrado" });
        }
        return Ok(libro);
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Vendedor")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult Consultar([FromQuery] ConsultarLibroFiltradosRequest request)
    {
        var libros = _libroService.ConsultarProductosCompletos(request);
        if (libros is null)
        {
            return NotFound(new { mensaje = "No se encontraron libros" });
        }
        return Ok(libros);
    }

   
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Vendedor")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult Actualizar(string id, ActualizarLibrosRequest request)
    {
        var actualizado = _libroService.Actualizar(id, request);
        if (!actualizado)
        {
            return NotFound(new { mensaje = "Libro no encontrado" });
        }
        return NoContent();
    }
}