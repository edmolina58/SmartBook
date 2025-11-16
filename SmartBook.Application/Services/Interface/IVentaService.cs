using SmartBook.Domain.Dtos.Reponses.VentasReponse;
using SmartBook.Domain.Dtos.Reponses.VentasReponses;
using SmartBook.Domain.Dtos.Requests.VentasRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBook.Application.Services.Interface;
public interface IVentaService
{

     VentaReponse? Crear(CrearVentaRequest request);
     ConsultarVentaReponse Consultar(string id);
}
