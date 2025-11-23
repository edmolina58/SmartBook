using SmartBook.Domain.Dtos.Reponses.IngresosReponses;
using SmartBook.Domain.Dtos.Requests.IngresosRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBook.Application.Services.Interface;
public interface IIngresoService
{
     IngresosReponse? Crear(CrearIngresoRequest request);

    IEnumerable<ConsultarIngresosResponse> ConsultarFiltrados(ConsultarIngresoFiltrado request);
    ConsultarIngresosResponse? Consultar(string idIngreso);
}
