using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartBook.Application.Services.LogsServices.Interface;
using SmartBook.Application.Services.Usuarios.Interfaces;
using SmartBook.Domain.Dtos.Reponses.LogsReponses;
using SmartBook.Domain.Dtos.Requests.LibrosRequest;
using SmartBook.Domain.Dtos.Requests.LogsRequest;

namespace SmartBook.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class LogsController : ControllerBase
{
    private readonly ILogService _logService;
    private readonly ILogger<LogsController> _logger;

    public LogsController(ILogService logService)
    {
        _logService = logService;
    }


    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult Consultar(string id)
    {
        var logs = _logService.ConsultarPorId(id);
        if (logs is null)
        {
            return NotFound(new { mensaje = "logs no encontrado" });
        }
        return Ok(logs);
    }


    [HttpGet]

    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public ActionResult Consultar([FromQuery] ConsultarLogUsuarios request)
    {
        var logs = _logService.ConsultarPorFiltros(request);
        if (logs is null)
        {
            return NotFound(new { mensaje = "No se encontraron logs" });
        }
        return Ok(logs);
    }

}



