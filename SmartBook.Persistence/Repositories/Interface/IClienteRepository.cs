
using SmartBook.Domain.Dtos.Reponses.ClienteReponse;
using SmartBook.Domain.Dtos.Requests.ClienteRequest;
using SmartBook.Domain.Entities;


namespace SmartBook.Persistence.Repositories.Interface;
public interface IClienteRepository 
{

    public bool ExisteCliente( string identificacion);
    public bool Borrar(string id);

    public bool Actulizar(string id, ActualizarClienteRequest request);

    public void Crear(Cliente cliente);

     ConsultarClienteReponse Consultar(string identificacion);

     IEnumerable<ConsultarClienteReponse> ConsultarPorNombre(string nombreCompleto);
}
