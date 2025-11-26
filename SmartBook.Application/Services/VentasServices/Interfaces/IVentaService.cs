using SmartBook.Domain.Dtos.Reponses.VentasReponses;
using SmartBook.Domain.Dtos.Requests.VentasRequest;

namespace SmartBook.Application.Services.Ventas.Interfaces
{
    public interface IVentaService
    {
        VentaReponse Crear(CrearVentaRequest request);
        ConsultarVentaReponse Consultar(string id);
        IEnumerable<ConsultarVentaReponse> ConsultarVentasCampos(ConsultarVentaFiltradoRequest request);
    }
}