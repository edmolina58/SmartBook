using Org.BouncyCastle.Asn1.Ocsp;
using SmartBook.Aplicacion.Extensions;
using SmartBook.Application.Services.Email.Interfaces;
using SmartBook.Application.Services.PDF.Interfaces;
using SmartBook.Application.Services.Ventas.Interfaces;
using SmartBook.Domain.Dtos.Reponses.VentasReponses;
using SmartBook.Domain.Dtos.Requests.VentasRequest;
using SmartBook.Domain.Entities.DatabaseEntities;
using SmartBook.Domain.Exceptions;
using SmartBook.Persistence.Repositories.Implementation;
using SmartBook.Persistence.Repositories.Interface;

namespace SmartBook.Application.Services.Ventas.Implementations
{
    public class VentaService : IVentaService
    {
        private readonly IVentaRepository _ventaRepository;
        private readonly IVentaPdfFormatter _ventaPdfFormatter;
        private readonly IEmailService _emailService;
        private readonly IClienteRepository _clienteRepository;
        private readonly ILibroRepository _libroRepository;
        private static readonly Random _random = new Random();
        private static readonly object _lock = new object();

        public VentaService( ILibroRepository libroRepository,
            IVentaRepository ventaRepository,
            IVentaPdfFormatter ventaPdfFormatter,
            IEmailService emailService,
            IClienteRepository clienteRepository)
        {
            _libroRepository = libroRepository;
            _ventaRepository = ventaRepository;
            _ventaPdfFormatter = ventaPdfFormatter;
            _emailService = emailService;
            _clienteRepository = clienteRepository;
        }
       
        public VentaReponse Crear(CrearVentaRequest request)
        {
            var unidades = _libroRepository.Consultar(request.LibroId);

            if (unidades.stock< request.Unidades)
            {                 throw new BusinessRoleException("No hay suficiente stock para completar la venta.");
            }
            var precio = _ventaRepository.ConsultarPrecio(request.LibroId);

            var venta= new Venta
            {
                Id = Guid.NewGuid().ToString(),
                NumeroReciboPago = GenerarCodigoNumerico(),
                Fecha = DateTime.Now,
                ClienteId = request.ClienteId.Sanitize().RemoveAccents(),
                UsuarioId = request.UsuarioId.Sanitize().RemoveAccents(),
                LibroId = request.LibroId.Sanitize().RemoveAccents(),
                Unidades = request.Unidades,
                Precio_unidad = precio.precio_unidad,
                Observaciones = request.Observaciones?.Sanitize().RemoveAccents() ?? string.Empty
            };

           var libro=  _libroRepository.Consultar(venta.LibroId);
            if (venta.Unidades < 1)
            {
                throw new BusinessRoleException("Las unidades deben ser mayores a cero.");
            } 

                _ventaRepository.Crear(venta);

            
            var pdf = _ventaPdfFormatter.FormatearVenta(venta);

            
            var cliente = _clienteRepository.Consultarid(venta.ClienteId)
                         ?? throw new BusinessRoleException("Cliente no existe");

            
            _emailService.EnviarReciboVenta(cliente.email, venta.NumeroReciboPago, pdf);

            return new VentaReponse(venta.Id, venta.Fecha);
        }

        public ConsultarVentaReponse Consultar(string id)
        {
            return _ventaRepository.Consultar(id);
        }

        public IEnumerable<ConsultarVentaReponse> ConsultarVentasCampos(ConsultarVentaFiltradoRequest request)
        {
            return _ventaRepository.ConsultarPorCampos(
                request.Desde,
                request.Hasta,
                request.ClienteId,
                request.LibroId
            );
        }



        public static long GenerarCodigoNumerico()
        {
            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss"); 
            int randomNum;

            lock (_lock)
            {
                randomNum = _random.Next(10000, 99999); 
            }

            return long.Parse($"{timestamp}{randomNum}");
        }
    }
}