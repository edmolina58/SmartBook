using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartBook.Application.Services.Ventas.Interfaces;
using SmartBook.Domain.Dtos.Requests.VentasRequest;
using SmartBook.Domain.Exceptions;

namespace SmartBook.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class VentasController : ControllerBase
{

    private readonly IVentaService _ventasService;
    public VentasController(IVentaService ventaService)
    {
        _ventasService = ventaService;

    }
    [HttpPost]
    //[Authorize(Roles = "Admin,Vendedor")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public ActionResult Crear(CrearVentaRequest request)
    {
        try
        {
            var libro = _ventasService.Crear(request);

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
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Vendedor")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public ActionResult Consultar(string id)
    {
        var libro = _ventasService.Consultar(id);
        if (libro is null)
        {
            return NotFound();
        }
        return Ok(libro);
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Vendedor")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public ActionResult Consultar([FromQuery] ConsultarVentaFiltradoRequest request)
    {

        var libro = _ventasService.ConsultarVentasCampos(request);
        if (libro is null )
        {
            return NotFound();
        }

        return Ok(libro);
    }
}