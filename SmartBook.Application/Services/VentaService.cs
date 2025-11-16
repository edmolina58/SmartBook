using Microsoft.Extensions.Configuration;
using SmartBook.Aplicacion.Extensions;
using SmartBook.Application.Services.Interface;
using SmartBook.Domain.Dtos.Reponses.VentasReponse;
using SmartBook.Domain.Dtos.Reponses.VentasReponses;
using SmartBook.Domain.Dtos.Requests.VentasRequest;
using SmartBook.Domain.Entities;
using SmartBook.Persistence.Repositories.Interface;


namespace SmartBook.WebApi.Services;

public class VentaService : IVentaService
{
    private static readonly List<Venta> _venta = []; 
    private readonly IVentaRepository _ventaRepository;


    private readonly IConfiguration _configuration;
    public VentaService(IVentaRepository ventaRepository, IConfiguration configuration)
    {
        _configuration = configuration;
        _ventaRepository = ventaRepository;

    }


    public VentaReponse? Crear(CrearVentaRequest request)
    { 
            var venta = new Venta
            {
                Id = DateTime.Now.Ticks.ToString(),
                NumeroReciboPago = (int)(DateTime.Now.Ticks & 0x7FFFFFFF),
                Fecha = DateTime.Now,
                ClienteId = request.ClienteId.Sanitize().RemoveAccents(),
                UsuarioId = request.UsuarioId.Sanitize().RemoveAccents(),
                LibroId = request.LibroId.Sanitize().RemoveAccents(),
                Observaciones = request.Observaciones.Sanitize().RemoveAccents(),
                 

            };
        _ventaRepository.Crear(venta);

            return new VentaReponse(venta.Id,venta.Fecha);

       
    }
    public ConsultarVentaReponse Consultar(string id)
    {
        return _ventaRepository.Consultar(id);
    }
}
