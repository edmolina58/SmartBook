
using Microsoft.Extensions.Configuration;
using SmartBook.Aplicacion.Extensions;
using SmartBook.Application.Services.Interface;
using SmartBook.Domain.Dtos.Reponses.ClienteReponse;
using SmartBook.Domain.Dtos.Requests.ClienteRequest;
using SmartBook.Domain.Dtos.Requests.ClientesRequest;
using SmartBook.Domain.Entities;
using SmartBook.Domain.Exceptions;
using SmartBook.Persistence.Repositories.Interface;

namespace SmartBook.Services;

public class ClienteService : IClienteService
{

    private readonly IClienteRepository _clienteRepository;
    private readonly IConfiguration _configuration;

    private const string FORMATO_FECHA = "yyyy-MM-dd";

    public ClienteService(IClienteRepository clienteRepository, IConfiguration configuration)
    {
        _configuration = configuration;
        _clienteRepository = clienteRepository;

    }


    private bool EsMayorDeEdad(DateOnly fechaNacimiento)
    {
        var validar = DateOnly.FromDateTime(DateTime.Today).Year - fechaNacimiento.Year;
        if (validar > 14)
        { return false; }
        else
        { return true; }

    }


    public ClienteReponse? Crear(CrearClienteRequest request)
    {

        var identificacionNormalizada = request.IdentificacionCliente.Sanitize().RemoveAccents();

        if (_clienteRepository.ExisteCliente(identificacionNormalizada))
        {
            throw new BusinessRoleException("Ya existe un cliente con esta identificación");
        }
        bool edad = EsMayorDeEdad(request.FechaNacimiento);

        if (edad is false)
        {
            throw new BusinessRoleException("El cliente debe tener al menos 14 años");
        }

        var cliente = new Cliente
        {
            IdCliente = DateTime.Now.Ticks.ToString(),
            Identificacion = request.IdentificacionCliente.Sanitize().RemoveAccents(),
            Nombres = request.NombreCliente.Sanitize().RemoveAccents(),
            Email = request.EmailCliente.Sanitize().RemoveAccents(),
            Celular = request.Celular.Sanitize().RemoveAccents(),
            FechaNacimiento = request.FechaNacimiento,
            fecha_creacion = DateTime.Now
        };

        _clienteRepository.Crear(cliente);



        var respouesta = new ClienteReponse(cliente.Identificacion, cliente.Nombres, cliente.Email);


        return respouesta;
    }






    public bool Actualizar(string id, ActualizarClienteRequest request)
    {


        var cliente = new ActualizarClienteRequest
            (
            identificacion: request.identificacion.Sanitize().RemoveAccents(),
            NombreCliente: request.NombreCliente.Sanitize().RemoveAccents(),
            EmailCliente: request.EmailCliente.Sanitize().RemoveAccents(),
            CelularCliente: request.CelularCliente.Sanitize().RemoveAccents(),
            FechaNacimientoCliente: request.FechaNacimientoCliente

        );
        if (cliente is null) {
            return false;

        }

        var edad = EsMayorDeEdad(request.FechaNacimientoCliente);
        if (edad is false)
        {
            throw new BusinessRoleException("El cliente debe tener al mas de 14 años");
        }
         _clienteRepository.Actulizar(id, cliente);

        return _clienteRepository.Actulizar(id, request);
    }

    public ConsultarClienteReponse Consultar(string id)
    {
        return _clienteRepository.Consultar(id);
    }

    public IEnumerable<ConsultarClienteReponse> ConsultarPorIdentificacion(ConsultarClienteFiltradoNombreRequest request)
    {

        return (IEnumerable<ConsultarClienteReponse>)_clienteRepository.ConsultarPorNombre(request.nombrecompleto);
    }


}