using SmartBook.Domain.Dtos.Reponses.VentasReponses;
using SmartBook.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBook.Persistence.Repositories.Interface;
public interface IVentaRepository
{


    public void Crear(Venta venta);

    public ConsultarVentaReponse Consultar(string Id);

    //IEnumerable<ConsultarVentaReponse> ConsultarFiltros(DateTime Fecha, string IdCliente, string IdLibro);
}
