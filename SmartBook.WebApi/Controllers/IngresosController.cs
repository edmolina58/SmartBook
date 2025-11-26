using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using SmartBook.Application.Services.Ingresos.Interfaces;
using SmartBook.Domain.Dtos.Requests.IngresosRequest;
using SmartBook.Domain.Exceptions;


namespace SmartBook.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class IngresosController : ControllerBase
{
    private readonly IIngresoService _ingresoService;

    public IngresosController(IIngresoService ingresoService)
    {
        _ingresoService = ingresoService;
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Vendedor")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public ActionResult Crear(CrearIngresoRequest request)
    {
        try
        {
            var Ingresar = _ingresoService.Crear(request);

            if (Ingresar is null)
            {
                return BadRequest(new { mensaje = "No se pudo crear el Ingreso" });
            }

            return Created(string.Empty, Ingresar);
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
    [Authorize(Roles = "Admin,Vendedor")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public ActionResult Consultar([FromQuery] ConsultarIngresoFiltrado request)
    {


            var ingresosfiltrados = _ingresoService.ConsultarFiltrados(request);
            if (ingresosfiltrados is null)
            {
                return NotFound(new { mensaje = "No se encontraron libros" });
            }
            return Ok(ingresosfiltrados);

        }


    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Vendedor")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public ActionResult ConsultarPorId(string id)
    {
        var ingresos = _ingresoService.Consultar(id);
        if (ingresos is null)
        {
            return NotFound(new { mensaje = "No se encontraron libros" });
        }
        return Ok(ingresos);
    } }



