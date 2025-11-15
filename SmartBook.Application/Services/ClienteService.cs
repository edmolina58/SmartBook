
using Microsoft.Extensions.Configuration;
using SmartBook.Aplicacion.Extensions;
using SmartBook.Application.Interface;
using SmartBook.Domain.Dtos.Reponses.ClienteReponse;
using SmartBook.Domain.Dtos.Requests.ClienteRequest;
using SmartBook.Domain.Dtos.Requests.ClientesRequest;
using SmartBook.Domain.Entities;
using SmartBook.Domain.Exceptions;
using SmartBook.Persistence.Repositories;
using SmartBook.Persistence.Repositories.Interface;
using System.Configuration;
namespace SmartBook.Services;

public class ClienteService : IClienteService
{

    private readonly IClienteRepository _clienteRepository;
    private readonly IConfiguration _configuration;


    public ClienteService(IClienteRepository clienteRepository, IConfiguration configuration)
    {
        _configuration = configuration;
        _clienteRepository = clienteRepository;

    }
    public ClienteReponse? Crear(CrearClienteRequest request)
    {
        // Validar que no exista cliente con misma identificación
        if (_clienteRepository.ExisteCliente(request.NombreCliente,request.IdentificacionCliente))
        {
            throw new BusinessRoleException("Ya existe un cliente con esta identificación");
        }



        var cliente = new Cliente
        {
            IdCliente = DateTime.Now.Ticks.ToString(),
            Identificacion = request.IdentificacionCliente,
            Nombres = request.NombreCliente.Sanitize().RemoveAccents(),
            Email = request.EmailCliente,
            Celular = request.Celular,
            FechaNacimiento = request.FechaNacimiento


        };

        _clienteRepository.Crear(cliente);

 

        var respouesta = new ClienteReponse(cliente.Identificacion,cliente.Nombres,cliente.Email);


        return respouesta;
    }

    public bool Borrar(string id)
    {
        return _clienteRepository.Borrar(id);
    }




    public bool Actualizar(string id, ActualizarClienteRequest request)
    {
        var cliente = _clienteRepository.Actulizar(id,request);
        if (cliente is false) return false;

        // Validar edad mínima
        var edad = DateTime.Today.Year - request.FechaNacimientoCliente.Year;
        if (edad < 14)
        {
            throw new BusinessRoleException("El cliente debe tener al menos 14 años");
        }
 
        return _clienteRepository.Actulizar(id, request);
    }

    public ConsultarClienteReponse Consultar(string id)
    {
        return _clienteRepository.Consultar(id);
    }

    public IEnumerable<ClienteReponse> ConsultarPorIdentificacion(ConsultarClienteFiltradoNombreRequest request)
    {

        return (IEnumerable<ClienteReponse>)_clienteRepository.ConsultarPorNombre(request.nombrecompleto);
    }


}