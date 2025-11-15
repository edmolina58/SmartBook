using Microsoft.AspNetCore.Mvc;
using SmartBook.Application.Interface;
using SmartBook.Domain.Dtos.Requests.VentasRequest;
using SmartBook.Domain.Exceptions;
using SmartBook.WebApi.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmartBook.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class VentasController : ControllerBase
{

    private readonly IVentaService _ventasService;
    public VentasController(IVentaService ventaService)
    {
        _ventasService = ventaService;

    }
    [HttpPost]
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
    public ActionResult Consultar(string id)
    {
        var libro = _ventasService.Consultar(id);
        if (libro is null)
        {
            return NotFound();
        }
        return Ok(libro);
    }

    /* seguir editando (NO TERMINADO)
     


    [HttpGet]
    public ActionResult Consultar([FromQuery] ConsultarClienteFiltradoNombreRequest request)
    {

        var cliente = _ventasService.ConsultarPorIdentificacion(request);
        return Ok(cliente);
    }*/

}
