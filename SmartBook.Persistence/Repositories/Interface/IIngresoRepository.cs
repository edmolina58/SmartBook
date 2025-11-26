using SmartBook.Domain.Dtos.Reponses.IngresosReponses;
using SmartBook.Domain.Entities.DatabaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBook.Persistence.Repositories.Interface;
public interface IIngresoRepository
{
    bool ExisteIngreso(string identificacion);
    public void Crear(Ingreso ingreso);
    public ConsultarIngresosResponse? Consultar(string idIngreso);
    IEnumerable<ConsultarIngresosResponse> ConsultarPorFecha(DateOnly fechainicio, DateOnly fechaFin);

}
