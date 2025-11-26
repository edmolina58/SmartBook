using SmartBook.Aplicacion.Extensions;
using SmartBook.Application.Services.Clientes.Interfaces;
using SmartBook.Application.Services.ClientesServices.Interfaces;

using SmartBook.Domain.Dtos.Reponses.ClienteReponse;
using SmartBook.Domain.Dtos.Requests.ClienteRequest;
using SmartBook.Domain.Dtos.Requests.ClientesRequest;
using SmartBook.Domain.Entities.DatabaseEntities;
using SmartBook.Domain.Exceptions;
using SmartBook.Persistence.Repositories.Interface;

namespace SmartBook.Application.Services.Clientes.Implementations
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IValidacionEdadService _validacionEdadService;

        public ClienteService(
            IClienteRepository clienteRepository,
            IValidacionEdadService validacionEdadService)
        {
            _clienteRepository = clienteRepository;
            _validacionEdadService = validacionEdadService;
        }

        public ClienteReponse? Crear(CrearClienteRequest request)
        {
            var identificacionNormalizada = request.IdentificacionCliente.Sanitize().RemoveAccents();

            if (_clienteRepository.ExisteCliente(identificacionNormalizada))
            {
                throw new BusinessRoleException("Ya existe un cliente con esta identificación");
            }

            _validacionEdadService.ValidarEdadMinima(request.FechaNacimiento, 14);

            var cliente = new Cliente
            {
                IdCliente = DateTime.Now.Ticks.ToString(),
                Identificacion = identificacionNormalizada,
                Nombres = request.NombreCliente.Sanitize().RemoveAccents(),
                Email = request.EmailCliente.Sanitize().RemoveAccents(),
                Celular = request.Celular.Sanitize().RemoveAccents(),
                FechaNacimiento = request.FechaNacimiento,
                fecha_creacion = DateTime.Now
            };

            _clienteRepository.Crear(cliente);

            return new ClienteReponse(cliente.Identificacion, cliente.Nombres, cliente.Email);
        }

        public bool Actualizar(string id, ActualizarClienteRequest request)
        {
            
            _validacionEdadService.ValidarEdadMinima(request.FechaNacimientoCliente, 14);

            var clienteActualizado = new ActualizarClienteRequest
            (
                identificacion: request.identificacion.Sanitize().RemoveAccents(),
                NombreCliente: request.NombreCliente.Sanitize().RemoveAccents(),
                EmailCliente: request.EmailCliente.Sanitize().RemoveAccents(),
                CelularCliente: request.CelularCliente.Sanitize().RemoveAccents(),
                FechaNacimientoCliente: request.FechaNacimientoCliente
            );

            return _clienteRepository.Actulizar(id, clienteActualizado);
        }

        public ConsultarClienteReponse Consultar(string id)
        {
            return _clienteRepository.Consultar(id.Sanitize().RemoveAccents());
        }

        public IEnumerable<ConsultarClienteReponse> ConsultarPorIdentificacion(ConsultarClienteFiltradoNombreRequest request)
        {
            return _clienteRepository.ConsultarPorNombre(request.nombrecompleto.Sanitize().RemoveAccents());
        }
    }
}
