using SmartBook.Domain.Dtos.Reponses.IngresosReponses;
using SmartBook.Domain.Dtos.Requests.IngresosRequest;

namespace SmartBook.Application.Services.Ingresos.Interfaces
{
    public interface IIngresoService
    {
        IngresosReponse Crear(CrearIngresoRequest request);
        IEnumerable<ConsultarIngresosResponse> ConsultarFiltrados(ConsultarIngresoFiltrado request);
        ConsultarIngresosResponse Consultar(string idIngreso);
    }
}