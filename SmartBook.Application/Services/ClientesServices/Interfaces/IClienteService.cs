using SmartBook.Domain.Dtos.Reponses.ClienteReponse;
using SmartBook.Domain.Dtos.Requests.ClienteRequest;
using SmartBook.Domain.Dtos.Requests.ClientesRequest;

namespace SmartBook.Application.Services.ClientesServices.Interfaces
{
    public interface IClienteService
    {
        ClienteReponse? Crear(CrearClienteRequest request);
        bool Actualizar(string id, ActualizarClienteRequest request);
        ConsultarClienteReponse Consultar(string id);
        IEnumerable<ConsultarClienteReponse> ConsultarPorIdentificacion(ConsultarClienteFiltradoNombreRequest request);
    }
}