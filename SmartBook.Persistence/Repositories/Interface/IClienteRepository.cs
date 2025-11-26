
using SmartBook.Domain.Dtos.Reponses.ClienteReponse;
using SmartBook.Domain.Dtos.Requests.ClienteRequest;
using SmartBook.Domain.Entities.DatabaseEntities;


namespace SmartBook.Persistence.Repositories.Interface;
public interface IClienteRepository 
{

    public bool ExisteCliente( string identificacion);

    public bool Actulizar(string id, ActualizarClienteRequest request);

    public void Crear(Cliente cliente);
    ConsultarClienteReponse Consultarid(string id_cliente);
     ConsultarClienteReponse Consultar(string identificacion);

     IEnumerable<ConsultarClienteReponse> ConsultarPorNombre(string nombreCompleto);
}
