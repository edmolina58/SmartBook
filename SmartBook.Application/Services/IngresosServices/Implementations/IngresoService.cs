using SmartBook.Aplicacion.Extensions;
using SmartBook.Application.Services.Ingresos.Interfaces;
using SmartBook.Domain.Dtos.Reponses.IngresosReponses;
using SmartBook.Domain.Dtos.Requests.IngresosRequest;
using SmartBook.Domain.Entities;
using SmartBook.Domain.Entities.DatabaseEntities;
using SmartBook.Domain.Exceptions;
using SmartBook.Persistence.Repositories.Interface;

namespace SmartBook.Application.Services.Ingresos.Implementations
{
    public class IngresoService : IIngresoService
    {
        private readonly IIngresoRepository _ingresoRepository;
        private readonly ILoteGeneradorService _loteGeneradorService;

        public IngresoService(
            IIngresoRepository ingresoRepository,
            ILoteGeneradorService loteGeneradorService)
        {
            _ingresoRepository = ingresoRepository;
            _loteGeneradorService = loteGeneradorService;
        }

        public IngresosReponse Crear(CrearIngresoRequest request)
        {
            var lote = _loteGeneradorService.GenerarLote();

            var ingreso = new Ingreso
            {
                IdIngresos = DateTime.Now.Ticks.ToString(),
                Unidades = request.Unidades,
                Lote = lote,
                ValorCompra = request.ValorCompra,
                ValorVenta = request.ValoVenta,
                Fecha = DateOnly.FromDateTime(DateTime.Now),
                libro = request.libros.Sanitize().RemoveAccents()
            };

            _ingresoRepository.Crear(ingreso);

            return new IngresosReponse(
                ingreso.IdIngresos,
                ingreso.Lote,
                ingreso.libro,
                ingreso.Unidades,
                ingreso.ValorCompra,
                ingreso.ValorVenta,
                ingreso.Fecha
            );
        }

        public IEnumerable<ConsultarIngresosResponse> ConsultarFiltrados(ConsultarIngresoFiltrado request)
        {
            return _ingresoRepository.ConsultarPorFecha(request.Desde, request.Hasta).AsEnumerable();
        }

        public ConsultarIngresosResponse Consultar(string idIngreso)
        {
            var idNormalizado = idIngreso.Sanitize().RemoveAccents();

            if (!_ingresoRepository.ExisteIngreso(idNormalizado))
            {
                throw new BusinessRoleException("El ingreso no existe");
            }

            return _ingresoRepository.Consultar(idNormalizado)!;
        }
    }
}