using SmartBook.Domain.Dtos.Reponses.ClienteReponse;
using SmartBook.Domain.Dtos.Requests.ClienteRequest;
using SmartBook.Domain.Dtos.Requests.ClientesRequest;
using SmartBook.WebApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBook.Application.Services.Interface;
public interface IClienteService
{

     ClienteReponse? Crear(CrearClienteRequest request);
     bool Actualizar(string id, ActualizarClienteRequest request);

     ConsultarClienteReponse Consultar(string id);
     IEnumerable<ConsultarClienteReponse> ConsultarPorIdentificacion(ConsultarClienteFiltradoNombreRequest request);

}
