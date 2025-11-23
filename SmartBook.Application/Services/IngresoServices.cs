using Microsoft.Extensions.Configuration;
using SmartBook.Aplicacion.Extensions;
using SmartBook.Application.Services.Interface;
using SmartBook.Domain.Dtos.Reponses.IngresosReponses;
using SmartBook.Domain.Dtos.Requests.IngresosRequest;
using SmartBook.Domain.Entities;
using SmartBook.Persistence.Repositories.Interface;
using System.Configuration;

namespace SmartBook.WebApi.Services
{


    public class IngresoService: IIngresoService
    {
        private readonly IIngresoRepository _ingresoRepository;
        private readonly ILibroRepository _libroRepository;
        private readonly IConfiguration _configuration;
        public IngresoService(IIngresoRepository ingresoRepository, ILibroRepository libroRepository, IConfiguration configuration)
        {   
            _configuration = configuration;
            _ingresoRepository = ingresoRepository;
            _libroRepository = libroRepository;
        }

        public IngresosReponse? Crear(CrearIngresoRequest request)
        {
          

            string lote ;
            if (DateTime.Now.Month <=6)
            {
                lote = $"{DateTime.Now.Year}-02";
            }
            else
            {
                lote = $"{DateTime.Now.Year}-01";
            }

            var ingresos = new Ingreso
            {
                IdIngresos = DateTime.Now.Ticks.ToString(),
                Unidades = request.Unidades,
                Lote = lote,
                ValorCompra = request.ValorCompra,
                ValorVenta = request.ValoVenta,
                Fecha = DateOnly.FromDateTime(DateTime.Now),
                libro = request.libros.Sanitize().RemoveAccents()

            };

            _ingresoRepository.Crear(ingresos);

            return new IngresosReponse(ingresos.IdIngresos, ingresos.Lote, ingresos.libro, ingresos.Unidades, ingresos.ValorCompra, ingresos.ValorVenta, ingresos.Fecha);




        }


        public IEnumerable<ConsultarIngresosResponse> ConsultarFiltrados(ConsultarIngresoFiltrado request)
        {

            return _ingresoRepository.ConsultarPorFecha(request.Desde, request.Hasta).AsEnumerable();
        }



        public ConsultarIngresosResponse Consultar(string idIngreso)
        {
            return _ingresoRepository.Consultar(idIngreso)!;

        }


    }



}
